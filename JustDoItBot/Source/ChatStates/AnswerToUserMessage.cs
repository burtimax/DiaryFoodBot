using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using DbInteractionMethods;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using JustDoItBot.Source.Db;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    class AnswerToUserMessage : ParentState
    {
        public AnswerToUserMessage(State state) : base(state)
        {

        }

        /// <summary>
        /// Вводит сообщение для другого пользователя
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    string text = mes.Data as string;

                    //нажал на кнопку отменить
                    if (IsKeyboardCommand(text, mes.ChatId))
                    {
                        return this.State.HopOnFailure;
                    }

                    //Получаем данные (формат данных [senderChatId|senderName??null])
                    //Мне нужно только senderChatId
                    List<string> data = this.State.Data.Split('|')?.ToList();

                    if (data?.Count == 0)
                    {
                        throw new NullReferenceException("Не получил данные");
                    }

                    string strChatId = data[0];
                    long receiverChatId;
                    if (long.TryParse(strChatId, out receiverChatId) == false)
                    {
                        throw new Exception("Не могу преобразовать string to long!");
                    }
                    
                    DataBase.Models.User me = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    BotMethods.SendMessageToUser(bot, receiverChatId, me, text);

                    Hop hop = this.State.HopOnSuccess.GetCopy();
                    hop.IntroductionString = Answer.AlreadySended;
                    return hop;
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}