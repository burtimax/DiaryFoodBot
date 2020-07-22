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
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    class SetSettings : ParentState
    {
        public SetSettings(State state) : base(state)
        {

        }

        /// <summary>
        /// Наберет команду в панели настроек
        /// Нужно обработать команду кнопku
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    if (IsKeyboardCommand(mes.Data as string, mes.ChatId))
                    {
                        return this.ProcessCommand(bot, mes, mes.Data as string);
                    }

                    return this.State.HopOnSuccess;
                    break;
                default:

                    break;
            }
            return base.ProcessMessage(bot, mes);
        }

        private Hop ProcessCommand(TelegramBotClient bot, InboxMessage mes, string command)
        {
            switch (command)
            {
                case Answer.BtnGoToMain:
                    return this.State.HopOnSuccess;
                    break;
                //Кнопка (изменить имя)
                case Answer.BtnChangeFirstName:
                    Hop h1 = new Hop()
                    {
                        NextStateName = "ChangeFirstName",
                        Type = HopType.RootLevelHop,
                    };
                    return h1;
                    break;
                //Кнопка (изменить фамилию)
                case Answer.BtnChangeLastName:
                    Hop h2 = new Hop()
                    {
                        NextStateName = "ChangeLastName",
                        Type = HopType.RootLevelHop,
                    };
                    return h2;
                    break;

                //Кнопка (изменить марафон)
                case Answer.BtnOrderToMarathon:
                    Hop h3 = new Hop()
                    {
                        NextStateName = "ChangeMarathon",
                        Type = HopType.RootLevelHop,
                    };
                    return h3;
                    break;

                //Кнопка (Написать в службу поддержки)
                case Answer.BtnWriteToSupport:
                    Hop h4 = new Hop()
                    {
                        NextStateName = "SupportService",
                        Type = HopType.RootLevelHop,
                    };
                    return h4;
                    break;

                //Кнопка (Написать в службу поддержки)
                case Answer.BtnWriteToConsultant:
                    return WriteToConsultant(bot, mes);
                    break;
            }

            return null;
        }


        /// <summary>
        ///Переход в состояние, где можно писать консультанту сообщение
        /// </summary>
        private Hop WriteToConsultant(TelegramBotClient bot, InboxMessage mes)
        {
            //Перед тем как отправить сообщение консультанту
            //Проверим пользователя на участие в каком нибудь марафоне
            //Затем проверим, активен ли марафон пользователя
            //Если пользователь участвует в марафоне и марафон активен, то можно отправить сообщение консультанту

            DataBase.Models.User user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
            string pKey = user.ActiveMarathonPublicKey;

            if (Equals(user.ActiveMarathonPublicKey, null))
            {
                bot.SendTextMessageAsync(mes.ChatId, Answer.FailWriteToConsultantNotParticipant);
                return null;
            }

            Marathon marathon = DbMethods.GetMarathonByPublicKey(this.Db, pKey);
            
            //Если не нашли марафон в базе или марафон удален, то user.MarathonPublicKey = null и вывести сообщение, что марафон завершен.
            if (marathon == null || marathon?.Status == "deleted")
            {
                user.ActiveMarathonPublicKey = null;
                bot.SendTextMessageAsync(mes.ChatId, Answer.FailWriteToConsultantMarathonFinished(marathon));
                this.Db.SaveChanges();
                return null;
            }

            DataBase.Models.User consultant = DbMethods.GetConsultantByMarathon(this.Db, marathon);


            Hop h5 = new Hop()
            {
                NextStateName = "WriteToConsultant",
                Type = HopType.RootLevelHop,
                IntroductionString = Answer.IntroductionToWriteToConsultant(consultant),
            };
            return h5;
        }
        
    }
}