﻿using System;
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
    class TEMPLATE : ParentState
    {
        public TEMPLATE(State state) : base(state)
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

                        return this.State.HopOnSuccess;
                    }

                    return this.State.HopOnSuccess;
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}