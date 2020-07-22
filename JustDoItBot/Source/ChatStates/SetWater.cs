using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    class SetWater : ParentState
    {
        public SetWater(State state) : base(state)
        {

        }

        /// <summary>
        /// Пользоатель вводит объем воды
        /// </summary>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes)
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    string text = mes.Data as string;
                    //Если отменил операцию.
                    if (IsKeyboardCommand(text, mes.ChatId) &&
                        string.Equals((text), Answer.BtnCancel))
                    {
                        Hop hopFail = this.State?.HopOnSuccess?.GetCopy();
                        if (hopFail != null)
                        {
                            hopFail.IntroductionString = Answer.AlreadyCancelled;
                        }
                        return hopFail;
                    }

                    //Пытаемся спарсить Объем воды.
                    Regex weightRegex = new Regex(@"(\D*)(?<integer>\d+)(\D*)");
                    var res = weightRegex.Match(text);
                    //Если не удалось распознать число
                    if (res.Success == false)
                    {
                        bot.SendTextMessageAsync(mes.ChatId, Answer.AskWaterAgain);
                        return null;
                    }

                    string num = res.Groups["integer"].Value;

                    int volume;
                    //Если не смогли преобразовать в int
                    if (int.TryParse(num, out volume) == false)
                    {
                        bot.SendTextMessageAsync(mes.ChatId, Answer.AskWaterAgainNotParsed);
                        return null;
                    }

                    //Записываем Воду в базу данных
                    DbMethods.SetTodayWater(this.Db, mes.ChatId, volume);

                    Hop hop = this.State?.HopOnSuccess?.GetCopy();
                    if (hop != null)
                    {
                        hop.IntroductionString = Answer.SuccessSetWater(volume);
                    }
                    return hop ?? this.State?.HopOnSuccess;
                    break;
                default:

                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}