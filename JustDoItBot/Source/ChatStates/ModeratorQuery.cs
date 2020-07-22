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
    class ModeratorQuery : ParentState
    {
        public ModeratorQuery(State state) : base(state)
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
                        switch (mes.Data as string)
                        {
                            case Answer.BtnYes:
                                DataBase.Models.User user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                                DbMethods.CreateConsultantQueryToSupportEntity(this.Db, mes.ChatId);
                                BotMethods.MakeIAmConsultantQueryToSupport(bot, user);

                                Hop hop = this.State?.HopOnSuccess?.GetCopy();
                                if (hop != null)
                                {
                                    hop.IntroductionString = Answer.AlreadyModeratorQuerySended;
                                }
                                return hop ?? this.State?.HopOnSuccess;
                                break;

                            case Answer.BtnNo:
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