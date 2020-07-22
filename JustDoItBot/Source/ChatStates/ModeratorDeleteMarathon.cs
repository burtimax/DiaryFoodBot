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
    class ModeratorDeleteMarathon : ParentState
    {
        public ModeratorDeleteMarathon(State state) : base(state)
        {

        }

        /// <summary>
        /// Отвечает (да/нет) Удалить ли марафон?
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    string text = mes.Data as string;
                    //Получаем переданные данные (publicKey удаляемого марафона)
                    string publicKey = this.State.Data;
                    if (string.IsNullOrWhiteSpace(publicKey))
                    {
                        throw new Exception("Публичный ключ не был передан в состояние удаления");
                    }

                    if (IsKeyboardCommand(text, mes.ChatId))
                    {
                        switch (text)
                        {
                            //Пользователь согласился удалить / значит удалить
                            case Answer.BtnYes:
                                DbMethods.DeleteMarathonByMarathonPublicKey(this.Db, publicKey);

                                Hop hop = this.State?.HopOnSuccess?.GetCopy();
                                if (Equals(hop, null) == false)
                                {
                                    hop.IntroductionString = Answer.AlreadyMarathonDeleted;
                                    hop.Type = HopType.RootLevelHop;
                                }

                                return hop ?? this.State.HopOnSuccess;
                                break;
                            //отказался удалять марафон
                            case Answer.BtnNo:
                                return this.State.HopOnFailure;
                                break;
                        }
                        return null;
                    }

                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}