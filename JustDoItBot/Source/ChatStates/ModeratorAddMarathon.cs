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
    public class ModeratorAddMarathon : ParentState
    {
        public ModeratorAddMarathon(State state) : base(state)
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
                        if (string.Equals(text, Answer.BtnCancel))
                        {
                            return this.State.HopOnFailure;
                        }
                        return this.State.HopOnSuccess;
                    }

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        bot.SendTextMessageAsync(mes.ChatId, Answer.FailInputMarathonName);
                        return null;
                    }

                    //Создать марафон и привязать к консультанту
                    DbMethods.CreateMarathonEntity(this.Db, mes.ChatId, text);

                    Hop hop = new Hop();
                    hop.NextStateName = "ModeratorMarathons";
                    hop.Type = HopType.RootLevelHop;
                    hop.IntroductionString = Answer.AlreadyMarathonCreated;
                    return hop;
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}