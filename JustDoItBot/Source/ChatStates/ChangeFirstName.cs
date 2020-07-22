using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    class ChangeFirstName : ParentState
    {
        public ChangeFirstName(State state) : base(state)
        {

        }

        /// <summary>
        /// Введет имя.
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    //Нажал на кнопку (отменить)
                    if (IsKeyboardCommand(mes.Data as string, mes.ChatId)&&
                        string.Equals((mes.Data as string), Answer.BtnCancel) == true)
                    {
                        return this.State.HopOnFailure;
                    }

                    //Переписать имя пользователя в базе данных
                    DataBase.Models.User user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
                    user.FirstName = (mes.Data as string).Trim(' ');
                    this.Db.SaveChanges();

                    Hop hop = this.State?.HopOnSuccess?.GetCopy();
                    if (hop != null)
                    {
                        hop.IntroductionString = Answer.AlreadyFirstNameChanged;
                        hop.Type = HopType.RootLevelHop;
                    }
                    return hop ?? this.State?.HopOnSuccess;
                    break;
                default:
                    bot.SendTextMessageAsync(mes.ChatId, Answer.FailInputFirstName);
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}