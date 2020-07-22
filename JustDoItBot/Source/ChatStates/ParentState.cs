using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using BotLibrary.Classes.Controller;
using DbInteractionMethods;
using FoodDiaryBot;
using FoodDiaryBot.DataBase.Context;
using FoodDiaryBot.Source.Constants;
using JustDoItBot.Source.Constants;
using JustDoItBot.Source.Db;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = FoodDiaryBot.DataBase.Models.User;

namespace JustDoItBot.Source.ChatStates
{
    public class ParentState : BaseState
    {
        protected BotDbContext Db;

        public ParentState(State state) : base(state)
        {
            this.Db = new BotDbContext(HelperDataBase.DB_OPTIONS);
        }

        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            return null;
        }

        /// <summary>
        /// Проверяем, является ли сообщение коммандой клавиатуры
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected bool IsKeyboardCommand(string command, FoodDiaryBot.DataBase.Models.User user)
        {
            if (Equals(this.State, null)) throw new NullReferenceException("State is NULL!");

            var keyboard = Keyboards.GetCurrentUserKeyboard(user, this.State);

            //Не может являться командой, так как в состоянии нет даже клавиатуры
            if (Equals(keyboard, null)){
                return false;
            }
            //Не забываем, что динамическая клавиатура в приоритете на статической.
            foreach (var buttonText in (keyboard?.ButtonTexts()))
            {
                if (string.Equals(buttonText, command))
                {
                    return true;
                }
            }

            return false;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="chatId">chatId пользователя</param>
        /// <returns></returns>
        protected bool IsKeyboardCommand(string command, long chatId)
        {
            FoodDiaryBot.DataBase.Models.User user = DbMethods.GetUserByChatId(this.Db, chatId);
            return IsKeyboardCommand(command, user);
        }


        public override Hop ProcessCallback(TelegramBotClient bot, long chatId, CallbackQuery callback, string data)
        {
            //Обработка callback запроса (Свернуть/скрыть)
            if (data.StartsWith(Answer.CallbackWrapThisMessage))
            {
                bot.DeleteMessageAsync(chatId, callback.Message.MessageId);
            }

            //Обработка callback запроса (Ответить на сообщение)
            if (data.StartsWith(Answer.CallbackAnswerToMessage))
            {
                //Забираем данные об отправители сообщения (формат данных [chatId|username])
                data = data.Replace(Answer.CallbackAnswerToMessage, "");
                var datas = data.Split('|')?.ToList();
                if (datas?.Count == 0)
                {
                    throw new NullReferenceException("Не получил данные");
                }

                string strChatId = datas[0];
                string username = datas.ElementAtOrDefault(1);

                long senderChatId;
                if (long.TryParse(strChatId, out senderChatId) == false)
                {
                    throw new Exception("Не могу преобразовать string to long!");
                }

                //Получили данные, теперь нужно перейти на состояние отправки сообщения
                //hop.Type должен быть NextLevelHop
                //Обязательно запомнить на каком сейчас мы состоянии, нужно будет вернуться обратно.
                Hop hop = new Hop();
                hop.NextStateName = "AnswerToUserMessage";
                hop.Type = HopType.NextLevelHop;//!!!!!!!!!!!
                hop.IntroductionString = Answer.AskInputMessageToUser(username);
                hop.Data = data;
                return hop;
            }

            //callback подтверждения/отклонения статуса консультанта
            if (data.StartsWith(Answer.CallbackConsultantQueryAnswer))
            {
                string consultantChatId = null;
                bool approveConsultant;
                if (data.StartsWith(Answer.CallbackConsultantQueryAnswerYes))
                {
                    consultantChatId = data.Replace(Answer.CallbackConsultantQueryAnswerYes, "");
                    approveConsultant = true;
                }
                else
                {
                    consultantChatId = data.Replace(Answer.CallbackConsultantQueryAnswerNo, "");
                    approveConsultant = false;
                }

                long consultChatId;
                if(long.TryParse(consultantChatId, out consultChatId) == false || 
                   string.IsNullOrWhiteSpace(consultantChatId) == true)
                {
                    throw new Exception("ChatId консультанта не был передан или был передан в неправильном формате!");
                }

                User user = DbMethods.GetUserByChatId(this.Db, consultChatId);
                DbMethods.ChangeUserToConsultant(this.Db, user, approveConsultant);

                if (approveConsultant)
                {
                    BotMethods.SendMessageToUser(bot,consultChatId,user,Answer.AlreadyConsultantQueryApproved);
                    DbMethods.DeleteConsultantQueryByChatId(this.Db, user.ChatId);
                    bot.AnswerCallbackQueryAsync(callback.Id, Answer.AlreadyAdminApprovedConsultantQuery);
                }
                else
                {
                    BotMethods.SendMessageToUser(bot, consultChatId, user, Answer.AlreadyConsultantQueryDenied);
                    DbMethods.DeleteConsultantQueryByChatId(this.Db, user.ChatId);
                    bot.AnswerCallbackQueryAsync(callback.Id, Answer.AlreadyAdminDeniedConsultantQuery);
                }

            }

           


            bot.AnswerCallbackQueryAsync(callback.Id);
            return base.ProcessCallback(bot, chatId, callback, data);
        }
    }
}
