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
    public class ModeratorWriteAllUsers : ParentState
    {
        public ModeratorWriteAllUsers(State state) : base(state)
        {

        }

        /// <summary>
        /// Вводит сообщение для всех пользователей, подписанных под него
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    string text = mes.Data as string;
                    //Нажал на кнопку отменить
                    if (IsKeyboardCommand(text, mes.ChatId))
                    {
                        return this.State.HopOnFailure;
                    }

                    //Получить все марафоны
                    //получить всех пользователей марафонов
                    //отправить сообщение пользователям
                    DataBase.Models.User consultant = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    BotMethods.SendMessageForAllUsersOfConsultant(this.Db, bot, consultant, text);

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