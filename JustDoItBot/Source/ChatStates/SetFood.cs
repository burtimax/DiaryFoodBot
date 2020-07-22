using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using DbInteractionMethods;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.DataBase.Models;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    public class SetFood : ParentState
    {
        public SetFood(State state) : base(state)
        {

        }

        /// <summary>
        /// Пользователь прислал описание приема пищи
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    //Если нажал на кнопку отменить
                    if (IsKeyboardCommand(mes.Data as string, mes.ChatId) &&
                        string.Equals((mes.Data as string), Answer.BtnCancel) == true)
                    {
                        Hop hopFail = this.State?.HopOnSuccess?.GetCopy();
                        if (hopFail != null)
                        {
                            hopFail.IntroductionString = Answer.AlreadyCancelled;
                        }
                        return hopFail;
                    }

                    Food food = new Food()
                    {
                        ChatId = mes.ChatId,
                        Description = mes.Data as string,
                    };

                    this.Db.Foods.Add(food);
                    this.Db.SaveChanges();

                    //Сделаем копию HopOnSuccess
                    //Поменяем IntroductionString in Hop
                    Hop hop = this.State.HopOnSuccess.GetCopy();
                    if (hop != null)
                    {
                        hop.IntroductionString = Answer.AlreadySended;
                    }
                    
                    return hop ?? this.State.HopOnSuccess;
                    break;
                default:
                    bot.SendTextMessageAsync(mes.ChatId, Answer.AskFood);
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}
