using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using DbInteractionMethods;
using FoodDiaryBot.DataBase.Models;
using FoodDiaryBot.Source.Constants;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FoodDiaryBot.Source.ChatStates
{
    public class ModeratorMarathons : ParentState
    {
        public ModeratorMarathons(State state) : base(state)
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
                        switch (text)
                        {
                            case Answer.BtnAddMarathon:
                                return GoToAddMarathon();
                                break;
                            case Answer.BtnEditDeleteMarathon:
                                ShowInlineMarathonList(bot, mes);
                                return null;
                                break;
                            case Answer.BtnGoBack:
                                return GoToModeratorMain();
                                break;
                            case Answer.BtnMyMarathons:
                                ShowConsultantMarathons(bot,mes);
                                return null;
                                break;
                        }
                        return this.State.HopOnSuccess;
                    }

                    return this.State.HopOnSuccess;
                    break;
                default:
                    
                    break;
            }


            return base.ProcessMessage(bot, mes);
        }


        public override Hop ProcessCallback(TelegramBotClient bot, long chatId, CallbackQuery callback,string data)
        {
            //Нажимаем на один из списка и получаем маленькую клавиатурку с кнопками (редактировать и удалить)
            if (data.StartsWith(Answer.CallbackShowMarathon))
            {
                string publicKey = data.Replace(Answer.CallbackShowMarathon, "");
                Marathon marathon = DbMethods.GetMarathonByPublicKey(this.Db,publicKey);
                if (marathon == null)
                {
                    return null;
                }

                var inline = Keyboards.GetEditDeleteInlineForMarathon(marathon).Value as InlineKeyboardMarkup;
                bot.EditMessageReplyMarkupAsync(chatId, callback.Message.MessageId, inline);
            }

            //Нажали на кнопку редактировать
            if (data.StartsWith(Answer.CallbackEditMarathon))
            {
                string publicKey = data.Replace(Answer.CallbackEditMarathon, "");
                
                Marathon marathon = DbMethods.GetMarathonByPublicKey(this.Db, publicKey);
                
                bot.AnswerCallbackQueryAsync(callback.Id);

                Hop hop = new Hop();
                hop.NextStateName = "ModeratorEditMarathonName";
                hop.Type = HopType.RootLevelHop;
                hop.IntroductionString = Answer.IntroductionToModeratorEditConcreteMarathonName(marathon?.Name);
                hop.Data = marathon.PublicKey;
                return hop;
            }

            //Нажали на кнопку Удалить
            if (data.StartsWith(Answer.CallbackDeleteMarathon))
            {
                string publicKey = data.Replace(Answer.CallbackDeleteMarathon, "");

                Marathon marathon = DbMethods.GetMarathonByPublicKey(this.Db, publicKey);

                bot.AnswerCallbackQueryAsync(callback.Id);

                Hop hop = new Hop();
                hop.NextStateName = "ModeratorDeleteMarathon";
                hop.Type = HopType.RootLevelHop;
                hop.IntroductionString = Answer.IntroductionToModeratorDeleteConcreteMarathon(marathon?.Name);
                hop.Data = marathon.PublicKey;
                return hop;
            }

            if (data.StartsWith(Answer.CallbackWrapThisMessage))
            {
                bot.DeleteMessageAsync(chatId, callback.Message.MessageId);
                return null;
            }

            return base.ProcessCallback(bot, chatId, callback, data);
        }


        private Hop GoToAddMarathon()
        {
            Hop hop = new Hop();
            hop.NextStateName = "ModeratorAddMarathon";
            hop.Type = HopType.RootLevelHop;
            return hop;
        }

        private Hop GoToModeratorMain()
        {
            Hop hop = new Hop();
            hop.NextStateName = "ModeratorMain";
            hop.Type = HopType.RootLevelHop;
            return hop;
        }

        private void ShowInlineMarathonList(TelegramBotClient bot, InboxMessage mes)
        {
            var inline = Keyboards.GetShowMarathonsInline(DbMethods.GetMarathonsByConsultantChatId(this.Db, mes.ChatId));
            if (Equals(inline, null) == true)
            {
                bot.SendTextMessageAsync(mes.ChatId, Answer.YouHaventMarathons);
                return;
            }
            bot.SendTextMessageAsync(mes.ChatId, Answer.MarathonList, replyMarkup:inline.Value);
        }


        private void ShowConsultantMarathons(TelegramBotClient bot, InboxMessage mes)
        {
            List<Marathon> marathons = DbMethods.GetMarathonsByConsultantChatId(this.Db, mes.ChatId);

            if (marathons.Count == 0)
            {
                bot.SendTextMessageAsync(mes.ChatId, Answer.YouHaventMarathons);
                return;
            }

            string res = null;

            foreach (var m in marathons)
            {
                res += $"{m.Name} [ID = {m.PublicKey}]\n";
                int countParticipants = DbMethods.GetCountParticipantsOfMarathon(this.Db, m.PublicKey);
                res += $"Участников: {countParticipants} чел.\n\n";
            }

            bot.SendTextMessageAsync(mes.ChatId, res, replyMarkup: Keyboards.InlineWrapMessage.Value);
            return;
        }
    }
}