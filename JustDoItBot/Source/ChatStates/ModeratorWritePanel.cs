using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using DbInteractionMethods;
using FoodDiaryBot.DataBase.Models;
using FoodDiaryBot.Source.Constants;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    public class ModeratorWritePanel : ParentState
    {
        public ModeratorWritePanel(State state) : base(state)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    string text = mes.Data as string;
                    if (IsKeyboardCommand(text, mes.ChatId))
                    {
                        switch (text)
                        {
                            case Answer.BtnGoBack:
                                return this.State.HopOnFailure;
                                break;

                            //Написать всем участникам
                            case Answer.BtnModeratorWriteAllUsers:
                                Hop hop = new Hop();
                                hop.NextStateName = "ModeratorWriteAllUsers";
                                hop.Type = HopType.RootLevelHop;
                                return hop;
                                break;

                            //Написать участникам марафона.
                            case Answer.BtnModeratorWriteMarathonUsers:
                                //отобразить список марафонов
                                var marathons = DbMethods.GetMarathonsByConsultantChatId(this.Db, mes.ChatId);

                                //Уберем из списка марафоны, где нет участников
                                marathons.RemoveAll(m =>
                                    DbMethods.GetCountParticipantsOfMarathon(this.Db, m.PublicKey) == 0);

                                if (Equals(marathons, null) || marathons.Count == 0)
                                {
                                    bot.SendTextMessageAsync(mes.ChatId, Answer.YouHaventMarathons);
                                    return null;
                                }

                                if (marathons.Count == 1)
                                {
                                    return GoToModeratorWriteMarathonUsers(marathons.First());
                                }

                                

                                var inline = Keyboards.GetShowMarathonsInline(marathons);
                                bot.SendTextMessageAsync(mes.ChatId, Answer.ChoseMarathon, replyMarkup: inline?.Value);
                                return null;
                                break;

                            //Написать конкретному пользователю
                            case Answer.BtnModeratorWriteConcreteUser:
                                //Вывести список пользователей
                                var allMarathons = DbMethods.GetMarathonsByConsultantChatId(this.Db, mes.ChatId);
                                var allUsers = new List<DataBase.Models.User>();

                                //Соберем всех пользователей всех марафонов консультанта
                                foreach (var m in allMarathons)
                                {
                                    allUsers.AddRange(DbMethods.GetAllParticipantsForMarathon(this.Db,m));
                                }

                                //Если нет пользователей. Выведем сообщение, что у консультанта нет участников марафонов.
                                if (allUsers?.Count == 0)
                                {
                                    bot.SendTextMessageAsync(mes.ChatId, Answer.YouHaventParticipants);
                                    return null;
                                }

                                var inlineUsers = Keyboards.GetShowParticipantsInline(allUsers);
                                bot.SendTextMessageAsync(mes.ChatId, Answer.ChoseUser, replyMarkup: inlineUsers.Value);
                                return null;
                                break;
                        }
                        return this.State.HopOnSuccess;
                    }

                    return this.State.HopOnSuccess;
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


        public override Hop ProcessCallback(TelegramBotClient bot, long chatId, CallbackQuery callback, string data)
        {
            //Пользователь выбрал марафон, перейти на состояние отправки сообщения для участников марафона.
            if (data.StartsWith(Answer.CallbackShowMarathon))
            {
                //Данные по марафону
                string publicKey = data.Replace(Answer.CallbackShowMarathon, "");

                var marathon = DbMethods.GetMarathonByPublicKey(this.Db, publicKey);
                if(Equals(marathon, null)) 
                {
                    throw new NullReferenceException("Marathon chosen in Inline can't be null");
                }

                bot.AnswerCallbackQueryAsync(callback.Id);

                //Марафон выбран, перейти в состояние ввода сообщения для участеников марафона
                return GoToModeratorWriteMarathonUsers(marathon);
            }

            //Пользователь выбрал участника, перейти на состояние отправки сообщения для участника.
            if (data.StartsWith(Answer.CallbackShowUser))
            {
                //Данные по участнику
                string userChatId = data.Replace(Answer.CallbackShowUser, "");

                if(long.TryParse(userChatId, out var longChatId) == false)
                {
                    throw new Exception("Не могу спарсить longChatId from string");
                }

                var user = DbMethods.GetUserByChatId(this.Db, longChatId);

                if (Equals(user, null) == true)
                {
                    throw new NullReferenceException("User chosen in Inline can't be null");
                }

                bot.AnswerCallbackQueryAsync(callback.Id);

                //Участник выбран, перейти в состояние ввода сообщения для участников марафона
                return GoToModeratorWriteUser(user);
            }


            return base.ProcessCallback(bot, chatId, callback, data);
        }

        private Hop GoToModeratorWriteMarathonUsers(Marathon marathon)
        {
            Hop hop = new Hop();
            hop.NextStateName = "ModeratorWriteMarathonUsers";
            hop.Type = HopType.RootLevelHop;
            hop.IntroductionString = Answer.IntroductionToModeratorWriteConcreteMarathonUsers(marathon.Name);
            hop.Data = marathon.PublicKey;
            return hop;
        }

        private Hop GoToModeratorWriteUser(DataBase.Models.User user)
        {
            string userName = user.FirstName + " " + user.LastName;

            Hop hop = new Hop();
            hop.NextStateName = "ModeratorWriteUser";
            hop.Type = HopType.RootLevelHop;
            hop.IntroductionString = Answer.IntroductionToModeratorWriteConcreteUser(userName);
            hop.Data = user.ChatId.ToString();
            return hop;
        }


    }
}