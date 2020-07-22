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
    class ModeratorEditMarathonName : ParentState
    {
        public ModeratorEditMarathonName(State state) : base(state)
        {

        }

        /// <summary>
        /// Вводим новое название марафона (передаем данные о марафоне через State.Data)
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

                        return null;
                    }

                    //Получаем переданные данные (publicKey редактируемого марафона)
                    string publicKey = this.State.Data;
                    if (string.IsNullOrWhiteSpace(publicKey))
                    {
                        throw new Exception("Публичный ключ не был передан в состояние редактирования");
                    }

                    DbMethods.RenameMarathonByMarathonPublicKey(this.Db, publicKey, text);
                    
                    Hop hop = this.State?.HopOnSuccess?.GetCopy();
                    if(Equals(hop, null) == false)
                    {
                        hop.IntroductionString = Answer.AlreadyMarathonNameChanged;
                        hop.Type = HopType.RootLevelHop;
                    }

                    return hop ?? this.State.HopOnSuccess;
                   
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}