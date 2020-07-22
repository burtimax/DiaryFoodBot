using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using BotLibrary.Classes.Controller;
using DbInteractionMethods;
using FoodDiaryBot.Source.Constants;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using JustDoItBot.Source.Db;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    public class InputLastName : ParentState
    {
        public InputLastName(State state) : base(state)
        {

        }

        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    //Пользователь отправляет фамилию, записать фамилию в базу данных
                    var user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    user.LastName = mes.Data as string;
                    this.Db.SaveChanges();

                    //Отправить оповещение админу о регистрации
                    BotMethods.WriteToSupport(bot, user, Answer.RegisteredNewUserMessageForSupport(user.FirstName+ " " + user.LastName));

                    //Переход в следующее состояние
                    return State.HopOnSuccess;
                    break;
                default:
                    bot.SendTextMessageAsync(mes.ChatId, Answer.FailInputLastName);
                    break;
            }

            return base.ProcessMessage(bot, mes);
        }
    }
}
