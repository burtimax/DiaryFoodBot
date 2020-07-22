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
using JustDoItBot.Source.Methods;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    class UserStatistics : ParentState
    {
        public UserStatistics(State state) : base(state)
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
                    if (IsKeyboardCommand(mes.Data as string, mes.ChatId))
                    {
                        return ProcessCommand(bot,mes,mes.Data as string);
                    }

                    return this.State.HopOnSuccess;
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }

        /// <summary>
        /// Обработка команды кнопки.
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="mes"></param>
        /// <param name="command"></param>
        private Hop ProcessCommand(TelegramBotClient bot, InboxMessage mes, string command)
        {
            Statistics stat = new Statistics();
            string statRes = null;
            switch (command)
            {
                case Answer.Btn1Day:
                    statRes = stat.GetUserStatistics(this.Db, mes.ChatId, 1);
                    break;
                case Answer.Btn3Day:
                    statRes = stat.GetUserStatistics(this.Db, mes.ChatId, 3);
                    break;
                case Answer.Btn7Day:
                    statRes = stat.GetUserStatistics(this.Db, mes.ChatId, 7);
                    break;
                case Answer.BtnGoToMain:
                    return this.State.HopOnSuccess;
                    break;
            }

            if (string.IsNullOrEmpty(statRes) == false)
            {
                bot.SendTextMessageAsync(mes.ChatId, statRes, replyMarkup:Keyboards.InlineWrapMessage.Value);
            }

            return null;
        }

    }
}