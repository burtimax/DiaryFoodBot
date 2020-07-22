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
    class WriteToSupport : ParentState
    {
        public WriteToSupport(State state) : base(state)
        {

        }

        /// <summary>
        /// Сообщение для разработчика.
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    if (IsKeyboardCommand(mes.Data as string, mes.ChatId) &&
                        string.Equals((mes.Data as string), Answer.BtnCancel)==true)
                    {
                        return this.State.HopOnFailure;
                    }

                    //Отправить сообщение в службу поддержки.
                    DataBase.Models.User user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    bot.WriteToSupport(user, mes.Data as string);

                    Hop hop = this.State?.HopOnSuccess?.GetCopy();
                    if (hop != null)
                    {
                        hop.IntroductionString = Answer.AlreadyMessageToSupportSended;
                    }
                    return hop ?? this.State?.HopOnSuccess;
                    break;
                default:

                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}