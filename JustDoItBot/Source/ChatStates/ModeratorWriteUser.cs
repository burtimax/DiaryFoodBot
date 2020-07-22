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
    public class ModeratorWriteUser : ParentState
    {
        public ModeratorWriteUser(State state) : base(state)
        {

        }

        /// <summary>
        /// Вводит сообщение для пользователя
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

                    //Получить данные о пользователе (передавали userChatId)
                    string userChatId = this.State.Data;
                    if (string.IsNullOrWhiteSpace(userChatId))
                    {
                        throw new NullReferenceException("Не был передан User.ChatId");
                    }

                    if (long.TryParse(userChatId, out var longChatId) == false)
                    {
                        throw new Exception("Неверный формат User.ChatId. Не могу спарсить string to long!");
                    }

                    //Отправить сообщение пользователю
                    DataBase.Models.User consultant = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    BotMethods.ModeratorSendMessageToUser(bot, longChatId, text, consultant);

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