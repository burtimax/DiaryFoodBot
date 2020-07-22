using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using FoodDiaryBot.DataBase.Context;
using FoodDiaryBot.DataBase.Models;
using FoodDiaryBot.Source.Constants;
using JustDoItBot.Source.Constants;
using Telegram.Bot;

namespace JustDoItBot.Source.Db
{
    public static class BotMethods
    {
        ///Метод расширения для TelegramBotClient
        public static void WriteToSupport(this TelegramBotClient bot, User sender, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            string mes = $"В службу поддержки:\n{sender.FirstName} {sender.LastName} [{sender.ChatId}]\n" + message;
            bot.SendTextMessageAsync(FoodDiaryBot.Source.Constants.Constants.SupportChatId, mes, replyMarkup:Keyboards.InlineAnswerToMessage(sender.ChatId,
                sender.FirstName + " " + sender.LastName).Value);
        }

        ///Метод расширения для TelegramBotClient
        public static void WriteToConsultant(this TelegramBotClient bot, User sender, User consultant, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (consultant == null || consultant?.Role != "moderator")
            {
                return;
            }

            string mes = $"[{sender.FirstName} {sender.LastName}]\n" + message;
            bot.SendTextMessageAsync(consultant.ChatId, mes, replyMarkup:Keyboards.InlineAnswerToMessage(sender.ChatId,
                sender.FirstName + " " + sender.LastName).Value);
        }

        /// <summary>
        /// Отправить сообщение для всех участников, всех марафонов консультанта
        /// </summary>
        /// <param name="db"></param>
        /// <param name="bot"></param>
        /// <param name="consultantChatId"></param>
        /// <param name="message"></param>
        public static void SendMessageForAllUsersOfConsultant(BotDbContext db, TelegramBotClient bot,
            User consultant, string message)
        {
            var marathons = (from m in db.Marathons
                where m.ModeratorChatId == consultant.ChatId
                select m)?.ToList();

            if (marathons?.Count == 0) return;

            foreach (var m in marathons)
            {
                SendMessageForAllUsersOfMarathon(db,bot,consultant,m,message);
            }
        }

        /// <summary>
        /// Отправить сообщение для всех участников марафона
        /// </summary>
        /// <param name="db"></param>
        /// <param name="bot"></param>
        /// <param name="marathon"></param>
        /// <param name="message"></param>
        public static void SendMessageForAllUsersOfMarathon(BotDbContext db, TelegramBotClient bot, User consultant,
            Marathon marathon, string message)
        {
            if (Equals(db, null) ||
                Equals(bot, null) ||
                Equals(marathon, null) || 
                string.IsNullOrWhiteSpace(message)) return;
            
            var usersMarathon = (from u in db.Users
                where u.ActiveMarathonPublicKey == marathon.PublicKey
                select u)?.ToList();

            if (usersMarathon?.Count == 0) return;

            foreach (var u in usersMarathon)
            {
               ModeratorSendMessageToUser(bot, u.ChatId, message, consultant);
            }
        }


        public static void ModeratorSendMessageToUser(TelegramBotClient bot, long userChatId, string message, User consultant)
        {
            string consultantName = consultant.FirstName + " " + consultant.LastName;
            bot.SendTextMessageAsync(userChatId,
                Answer.ConfigureMessageToUserFromConsultant(message, consultantName), replyMarkup:Keyboards.InlineAnswerToMessage(consultant.ChatId, consultantName).Value);
        }

        public static void SendMessageToUser(TelegramBotClient bot, long receiverChatId, User sender, string message)
        {
            string senderName = sender.FirstName + " " + sender.LastName;
            bot.SendTextMessageAsync(receiverChatId, Answer.ConfigureMessageToUser(message, senderName), replyMarkup:Keyboards.InlineAnswerToMessage(sender.ChatId, senderName).Value);
        }

        public static void MakeIAmConsultantQueryToSupport(TelegramBotClient bot, User sender)
        {
            string mes = $"{sender.FirstName} {sender.LastName} [{sender.ChatId}]\n" + Answer.IAmConsultantQuery;
            bot.SendTextMessageAsync(FoodDiaryBot.Source.Constants.Constants.SupportChatId, mes, replyMarkup: Keyboards.InlineConsultantQueryToSupport(sender.ChatId,
                sender.FirstName + " " + sender.LastName).Value);
        }

    }
}
