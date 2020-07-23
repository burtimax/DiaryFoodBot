using System.Linq;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using BotLibrary.Classes.Controller;
using DbInteractionMethods;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.DataBase.Models;
using JustDoItBot.Source.ChatStates;
using JustDoItBot.Source.Constants;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace FoodDiaryBot.Source.ChatStates
{
    public class ChangeMarathon : ParentState
    {
        public ChangeMarathon(State state) : base(state)
        {

        }

        /// <summary>
        /// Введет ID (PublicKey) марафона
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public override Hop ProcessMessage(TelegramBotClient bot, InboxMessage mes )
        {
            switch (mes.Type)
            {
                case MessageType.Text:
                    return ProcessTextMessage(bot, mes, mes.Data as string);
                    break;
                default:
                    bot.SendTextMessageAsync(mes.ChatId, Answer.FailInputMarathonId);
                    break;
            }

            return base.ProcessMessage(bot, mes);
        }

       
        private Hop ProcessTextMessage(TelegramBotClient bot, InboxMessage mes, string text)
        {
            //If keyboard Command
            if (this.IsKeyboardCommand(text, mes.ChatId))
            {
                switch (text)
                {
                    case Answer.BtnCancel:
                        return this.State.HopOnFailure;
                        break;
                }

                return null;
            }

            //NoT Keyboard Command
            //Найти номер марафона (если нашли, то подключить к марафону, иначе вывести сообщение, что не нашли марафон с таким ID
            var marathon = DbMethods.GetMarathonByPublicKey(this.Db, text);

            if (Equals(marathon, null))
            {
                bot.SendTextMessageAsync(mes.ChatId, Answer.FailFindInputedMarathon(text));
                return null;
            }

            //Подключить пользователя к марафону.
            var user = DbMethods.GetUserByChatId(this.Db, mes.ChatId);
            
            //Проверим если пользователь уже подключен к марафону
            var mar = (from m in this.Db.Marathons
                where (m.PublicKey == text.Trim(' ')) && (user.ActiveMarathonPublicKey == m.PublicKey)
                select m).FirstOrDefault();
            if(mar != null)
            {
                Hop h = this.State?.HopOnSuccess?.GetCopy();
                if (h != null)
                {
                    h.IntroductionString = Answer.AlreadyIncludedToMarathon(mar.Name);
                }
                return h ?? this.State?.HopOnSuccess;
            }

            

            user.ActiveMarathonPublicKey = marathon.PublicKey;
            this.Db.SaveChanges();

            //Подключили пользователя к марафону, отправим сообщение консультанту
            DataBase.Models.User consultant = DbMethods.GetConsultantByMarathon(this.Db, marathon);
            string participantName = user.FirstName + " " + user.LastName;
            bot.SendTextMessageAsync(consultant.ChatId,
                Answer.IncludedNewParticipantToMarathon(marathon.Name, participantName));

            //Вывести сообщение о марафоне и перейти к панели настроек
            Hop hop = this.State?.HopOnSuccess?.GetCopy();
            if (hop != null)
            {
                hop.IntroductionString = Answer.SuccessIncludeToMarathon(marathon);
            }
            return hop ?? this.State?.HopOnSuccess;
        }

    }
}
