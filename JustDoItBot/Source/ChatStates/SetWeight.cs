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
    public class SetWeight : ParentState
    {
        public SetWeight(State state) : base(state)
        {

        }

        /// <summary>
        /// Пользователь прислал вес.
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

                    //Пытаемся спарсить вес.
                    Regex weightRegex = new Regex(@"(\D*)(?<integer>\d+)([\s,.]*)(?<decimal>\d*)(\D*)");
                    var res = weightRegex.Match(text);
                    //Если не удалось распознать число
                    if (res.Success == false)
                    {
                        bot.SendTextMessageAsync(mes.ChatId, Answer.AskWeighAgain);
                        return null;
                    }

                    string num1 = res.Groups["integer"].Value;
                    string num2 = res.Groups["decimal"].Value;
                    string weightNum = num1;
                    if(string.IsNullOrEmpty(num2) == false)
                    {
                        weightNum += ',' + num2;
                    }

                    double weight;
                    //Если не смогли преобразовать в double
                    if (double.TryParse(weightNum, out weight) == false)
                    {
                        bot.SendTextMessageAsync(mes.ChatId, Answer.AskWeighAgainNotParsed);
                        return null;
                    }

                    //Записываем вес в базу данных
                    Weight w = new Weight()
                    {
                        ChatId = mes.ChatId,
                        Mass = weight
                    };
                    this.Db.Weights.Add(w);
                    this.Db.SaveChanges();

                    Hop hop = this.State?.HopOnSuccess?.GetCopy();
                    if (hop != null)
                    {
                        hop.IntroductionString = Answer.AlreadySended;
                    }
                    return hop ?? this.State?.HopOnSuccess;
                    break;
                default:
                    bot.SendTextMessageAsync(mes.ChatId, Answer.AskWeight);
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


    }
}