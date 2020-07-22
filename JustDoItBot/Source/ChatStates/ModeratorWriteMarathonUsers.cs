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
    public class ModeratorWriteMarathonUsers : ParentState
    {
        public ModeratorWriteMarathonUsers(State state) : base(state)
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
                        return this.State.HopOnFailure;
                    }

                    //В состояние передаем данные Marathon.PublicKey
                    string publicKey = this.State.Data;
                    if (string.IsNullOrWhiteSpace(publicKey))
                    {
                        throw new NullReferenceException("PublicKey не был передан. Нужно передать данные о марафоне.");
                    }

                    var marathon = DbMethods.GetMarathonByPublicKey(this.Db, publicKey);
                    DataBase.Models.User consultant = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    BotMethods.SendMessageForAllUsersOfMarathon(this.Db, bot, consultant, marathon, text);

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