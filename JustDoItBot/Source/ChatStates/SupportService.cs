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
    class SupportService : ParentState
    {
        public SupportService(State state) : base(state)
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
                        switch(mes.Data as string)
                        {
                            case Answer.BtnModeratorQuery:
                                Hop GoModeratorQuery = new Hop();
                                GoModeratorQuery.NextStateName = "ModeratorQuery";
                                GoModeratorQuery.Type = HopType.RootLevelHop;
                                return GoModeratorQuery;
                                break;
                            case Answer.BtnWriteAboutProblem:
                                Hop h1 = new Hop();
                                h1.NextStateName = "WriteToSupport";
                                h1.Type = HopType.RootLevelHop;
                                h1.IntroductionString = Answer.AskWriteAboutProblem;
                                return h1;
                                break;
                            case Answer.BtnFeedBack:
                                Hop h2 = new Hop();
                                h2.NextStateName = "WriteToSupport";
                                h2.Type = HopType.RootLevelHop;
                                h2.IntroductionString = Answer.AskWriteFeedBack;
                                return h2;
                                break;
                            case Answer.BtnGoBack:
                                return this.State.HopOnFailure;
                                break;
                        }
                        
                    }

                    
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}