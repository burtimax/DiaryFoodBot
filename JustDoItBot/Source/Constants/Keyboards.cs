using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using BotLibrary.Classes.Bot;
using DbInteractionMethods;
using FoodDiaryBot.DataBase.Models;
using JetBrains.Annotations;
using JustDoItBot.DataBase.Models;
using JustDoItBot.Source.Constants;
using Telegram.Bot.Types.ReplyMarkups;

namespace FoodDiaryBot.Source.Constants
{
    public class Keyboards
    {
        public static MarkupWrapper<ReplyKeyboardMarkup> CancelOperationKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnCancel);

        public static MarkupWrapper<ReplyKeyboardMarkup> ButtonSkipKeyboard = new MarkupWrapper<ReplyKeyboardMarkup>(true)
            .NewRow()
            .Add(Answer.BtnSkip);

        public static MarkupWrapper<ReplyKeyboardMarkup> MainUserKeyboard = new MarkupWrapper<ReplyKeyboardMarkup>(true)
            .NewRow()
            .Add(Answer.BtnWaterRemove300)
            .Add(Answer.BtnSetWater)
            .Add(Answer.BtnWaterAdd300)
            .NewRow()
            .Add(Answer.BtnSetFood)
            .Add(Answer.BtnSetWeight)
            .NewRow()
            .Add(Answer.BtnStatistics)
            .Add(Answer.BtnSettings);

        public static MarkupWrapper<ReplyKeyboardMarkup> MainUserKeyboardWithModeratorButton = new MarkupWrapper<ReplyKeyboardMarkup>(true)
            .NewRow()
            .Add(Answer.BtnWaterRemove300)
            .Add(Answer.BtnSetWater)
            .Add(Answer.BtnWaterAdd300)
            .NewRow()
            .Add(Answer.BtnSetFood)
            .Add(Answer.BtnSetWeight)
            .NewRow()
            .Add(Answer.BtnStatistics)
            .Add(Answer.BtnSettings)
            .NewRow()
            .Add(Answer.BtnConsultantPanel);

        public static MarkupWrapper<ReplyKeyboardMarkup> MainUserKeyboardWithAdminButton = new MarkupWrapper<ReplyKeyboardMarkup>(true)
            .NewRow()
            .Add(Answer.BtnWaterRemove300)
            .Add(Answer.BtnSetWater)
            .Add(Answer.BtnWaterAdd300)
            .NewRow()
            .Add(Answer.BtnSetFood)
            .Add(Answer.BtnSetWeight)
            .NewRow()
            .Add(Answer.BtnStatistics)
            .Add(Answer.BtnSettings)
            .NewRow()
            .Add(Answer.BtnConsultantPanel)
            .Add(Answer.BtnAdminPanel);

        public static MarkupWrapper<ReplyKeyboardMarkup> SettingsUserKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnChangeFirstName)
                .Add(Answer.BtnChangeLastName)
                .NewRow()
                .Add(Answer.BtnOrderToMarathon)
                .NewRow()
                .Add(Answer.BtnWriteToSupport)
                .Add(Answer.BtnWriteToConsultant)
                .NewRow()
                .Add(Answer.BtnGoToMain);

        public static MarkupWrapper<ReplyKeyboardMarkup> StatisticsUserKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.Btn1Day)
                .Add(Answer.Btn3Day)
                .Add(Answer.Btn7Day)
                .NewRow()
                .Add(Answer.BtnGoToMain);

        public static MarkupWrapper<ReplyKeyboardMarkup> SupportUserKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnModeratorQuery)
                .NewRow()
                .Add(Answer.BtnWriteAboutProblem)
                .NewRow()
                .Add(Answer.BtnFeedBack)
                .NewRow()
                .Add(Answer.BtnGoBack);

        public static MarkupWrapper<ReplyKeyboardMarkup> KeyboardYesNo =
        new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnNo)
                .Add(Answer.BtnYes);

        public static MarkupWrapper<ReplyKeyboardMarkup> ModeratorMainKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnMarathons)
                .Add(Answer.BtnParticipantsData)
                .NewRow()
                .Add(Answer.BtnModeratorWritePanel)
                .NewRow()
                .Add(Answer.BtnUserPanel);

        public static MarkupWrapper<ReplyKeyboardMarkup> ModeratorWritePanelKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnModeratorWriteAllUsers)
                .NewRow()
                .Add(Answer.BtnModeratorWriteMarathonUsers)
                .NewRow()
                .Add(Answer.BtnModeratorWriteConcreteUser)
                .NewRow()
                .Add(Answer.BtnGoBack);

        public static MarkupWrapper<ReplyKeyboardMarkup> ModeratorMarathonKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnMyMarathons)
                .NewRow()
                .Add(Answer.BtnAddMarathon)
                .NewRow()
                .Add(Answer.BtnEditDeleteMarathon)
                .NewRow()
                .Add(Answer.BtnGoBack);

        public static MarkupWrapper<ReplyKeyboardMarkup> AdminMainKeyboard =
            new MarkupWrapper<ReplyKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnBotInfo)
                .Add(Answer.BtnBotMarathons)
                .Add(Answer.BtnBotModerators)
                .NewRow()
                .Add(Answer.BtnAdminCommands)
                .Add(Answer.BtnListQueries)
                .NewRow()
                .Add(Answer.BtnGoToMain);

        /// <summary>
        /// Return InlineKeyboard (Marathons List)
        /// </summary>
        /// <param name="marathons"></param>
        /// <returns></returns>
        [CanBeNull]
        public static MarkupWrapper<InlineKeyboardMarkup>  GetShowMarathonsInline(List<Marathon> marathons)
        {
            MarkupWrapper<InlineKeyboardMarkup> inline = new MarkupWrapper<InlineKeyboardMarkup>(true);

            if (marathons?.Count == 0) return null;

            foreach (var m in marathons)
            {
                inline = inline.NewRow().Add(m.Name + $"\nID = {m.PublicKey}", Answer.CallbackShowMarathon + m.PublicKey);
            }

            inline = inline.NewRow()
                .Add(Answer.BtnWrap, Answer.CallbackWrapThisMessage); ;

            return inline;
        }

        /// <summary>
        /// Маленькая клавиатурка (марафон и две кнопки (редактировать и удалить))
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static MarkupWrapper<InlineKeyboardMarkup> GetEditDeleteInlineForMarathon(Marathon m)
        {
            return new MarkupWrapper<InlineKeyboardMarkup>(true)
                .NewRow()
                .Add($"{m.Name}\nID = {m.PublicKey}")
                .NewRow()
                .Add(Answer.EmojiPen, Answer.CallbackEditMarathon + m.PublicKey)
                .Add(Answer.EmojiWasteBasket, Answer.CallbackDeleteMarathon + m.PublicKey);
        }

        /// <summary>
        /// Список участников (пользователей)
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static MarkupWrapper<InlineKeyboardMarkup> GetShowParticipantsInline(List<DataBase.Models.User> users)
        {
            MarkupWrapper<InlineKeyboardMarkup> inline = new MarkupWrapper<InlineKeyboardMarkup>(true);

            if (users?.Count == 0) return null;

            foreach (var u in users)
            {
                inline = inline
                    .NewRow()
                    .Add($"{u.FirstName} {u.LastName}", Answer.CallbackShowUser + u.ChatId);
            }

            return inline.NewRow()
                .Add(Answer.BtnWrap, Answer.CallbackWrapThisMessage);
        }

        /// <summary>
        /// Клавиатурка (вывести список участников) + кнопки (1 дн, 3 дн, 7 дн)
        /// C кнопками вывода данных.
        /// </summary>
        /// <param name="marathons"></param>
        /// <returns></returns>
        public static MarkupWrapper<InlineKeyboardMarkup> GetShowParticipantsDataInline(List<DataBase.Models.User> users, bool addBackButton = false)
        {
            MarkupWrapper<InlineKeyboardMarkup> inline = new MarkupWrapper<InlineKeyboardMarkup>(true);

            if (users?.Count == 0) return null;

            foreach (var u in users)
            {
                inline = inline
                    .NewRow()
                    .Add($"{u.FirstName} {u.LastName}", Answer.CallbackShowUser + u.ChatId)
                    .NewRow()
                    .Add(Answer.Btn1DayShort, Answer.CallbackShowUserData + $"1|{u.ChatId}|{u.FirstName} {u.LastName}")
                    .Add(Answer.Btn3DayShort, Answer.CallbackShowUserData + $"3|{u.ChatId}|{u.FirstName} {u.LastName}")
                    .Add(Answer.Btn7DayShort, Answer.CallbackShowUserData + $"7|{u.ChatId}|{u.FirstName} {u.LastName}");
            }

            inline = inline.NewRow();

            if (addBackButton)
            {
                inline = inline.Add(Answer.BtnGoBack, Answer.CallbackGoBack);
            }

            inline = inline
                .Add(Answer.BtnWrap, Answer.CallbackWrapThisMessage);

            return inline;
        }

        /// <summary>
        /// inline button (Wrap this Message)
        /// </summary>
        public static MarkupWrapper<InlineKeyboardMarkup> InlineWrapMessage =
            new MarkupWrapper<InlineKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnWrap, Answer.CallbackWrapThisMessage);

        /// <summary>
        /// Кнопка (Ответить) под сообщением
        /// </summary>
        public static MarkupWrapper<InlineKeyboardMarkup> InlineAnswerToMessage(long chatId, string username = " ")
        {
            return new MarkupWrapper<InlineKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnAnswerMessage, Answer.CallbackAnswerToMessage + chatId + "|" + username);
        }

        /// <summary>
        /// Кнопка (Ответить) под сообщением
        /// </summary>
        public static MarkupWrapper<InlineKeyboardMarkup> InlineConsultantQueryToSupport(long chatId, string username = " ")
        {
            return new MarkupWrapper<InlineKeyboardMarkup>(true)
                .NewRow()
                .Add(Answer.BtnApproveConsultant, Answer.CallbackConsultantQueryAnswerYes + chatId)
                .Add(Answer.BtnDenyConsultant, Answer.CallbackConsultantQueryAnswerNo + chatId)
                .NewRow()
                .Add(Answer.BtnAnswerMessage, Answer.CallbackAnswerToMessage + chatId + "|" + username);
        }











        /// <summary>
        /// Определяем текущую панель пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static MarkupWrapper<ReplyKeyboardMarkup> GetCurrentUserKeyboard(DataBase.Models.User user, State state)
        {
            ///Панели бывают как динамические, так и статические.
            ///Динамические изменяются в процессе, это зависит от статуса пользователя.
            ///Динамические панели в приоритете над статическими.

            if (string.Equals(state.Name, "MainUser") == true && 
                (user.Role == Constants.ROLE_ADMIN || user.ChatId == Constants.SupportChatId))
            {
                return Keyboards.MainUserKeyboardWithAdminButton;
            }

            if (string.Equals(state.Name, "MainUser") == true && user.Role == Constants.ROLE_MODERATOR)
            {
                return Keyboards.MainUserKeyboardWithModeratorButton;
            }

            if (Equals(state.DynamicReplyKeyboard, null) == false)
            {
                return state.DynamicReplyKeyboard;
            }

            return state.ReplyKeyboard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static MarkupWrapper<InlineKeyboardMarkup> ConfigConsultantQueriesInlineKeyboard(Dictionary<QueryToSupport,DataBase.Models.User> qu)
        {
            if (Equals(qu, null) || qu?.Count == 0) return null;

            MarkupWrapper<InlineKeyboardMarkup> inline = new MarkupWrapper<InlineKeyboardMarkup>(true);

            foreach(var q in qu)
            {
                inline = inline
                    .NewRow()
                    .Add(q.Value.FirstName + " " + q.Value.LastName)
                    .NewRow()
                    .Add(Answer.BtnApproveConsultant, Answer.CallbackConsultantQueryAnswerYes+q.Value.ChatId)
                    .Add(Answer.BtnDenyConsultant, Answer.CallbackConsultantQueryAnswerNo + q.Value.ChatId);
            }

            return inline.NewRow().Add(Answer.BtnWrap, Answer.CallbackWrapThisMessage);
        }
    }
}
