using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BotLibrary.Classes;
using FoodDiaryBot;
using FoodDiaryBot.DataBase.Context;
using FoodDiaryBot.DataBase.Models;
using FoodDiaryBot.Source.Constants;
using JetBrains.Annotations;
using JustDoItBot.DataBase.Models;
using JustDoItBot.Source.Constants;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Message = JustDoItBot.DataBase.Models.Message;
using User = FoodDiaryBot.DataBase.Models.User;

namespace DbInteractionMethods
{
    public class DbMethods
    {
        private static void CheckDbContext(BotDbContext db)
        {
            if(Equals(db, null)) throw new NullReferenceException("DB is null!");
        }

        /// <summary>
        /// Получить пользователя из базы данных, не может вернуть Null, так как пользователь создается сразу же
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ChatId"></param>
        /// <returns></returns>
        [CanBeNull]
        public static User GetUserByChatId([NotNull] BotDbContext db, long ChatId)
        {
            CheckDbContext(db);

            return (from u in db.Users
                where u.ChatId == ChatId
                select u)?.FirstOrDefault();
        }

        /// <summary>
        /// Получить марафон через публичный ключ. или вернуть NULL
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        [CanBeNull]
        public static Marathon GetMarathonByPublicKey([NotNull] BotDbContext db, string publicKey)
        {
            CheckDbContext(db);
            if (string.IsNullOrEmpty(publicKey)) return null;

            return (from m in db.Marathons
                where string.Equals(m.PublicKey, publicKey)
                select m).FirstOrDefault();
        }

        /// <summary>
        /// Установить объем воды на текущий день
        /// </summary>
        /// <param name="db"></param>
        /// <param name="volume"></param>
        public static void SetTodayWater([NotNull] BotDbContext db, long chatId, int volume)
        {
            CheckDbContext(db);

            //Взять последнюю запись воды, дата которой совпадает с текущим днем.
            Water curWater = GetWaterByDate(db, chatId, DateTime.Today);

            if(curWater == null)
            {
                curWater = new Water()
                {
                    ChatId = chatId,
                };
                db.Waters.Add(curWater);
            }

            curWater.Quantity = volume;
            db.SaveChanges();
        }

        /// <summary>
        /// Получить объем воды на определенный день
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ChatId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Water GetWaterByDate(BotDbContext db, long chatId, DateTime date)
        {
            CheckDbContext(db);

            //Взять последнюю запись воды, дата которой совпадает с текущим днем.
            return (from w in db.Waters
                where (w.CreateTime.Date == date.Date && w.ChatId == chatId)
                select w).FirstOrDefault();
        }

        /// <summary>
        /// Получить консультанта марафона
        /// </summary>
        public static User GetConsultantByMarathon(BotDbContext db, Marathon marathon)
        {
            CheckDbContext(db);
            return (from c in db.Users
                where c.ChatId == marathon.ModeratorChatId && c.Role == "moderator"
                select c).FirstOrDefault();
        }

        /// <summary>
        /// Получить консультанта марафона через публичный ключ
        /// </summary>
        public static User GetConsultantByMarathonPublicKey(BotDbContext db, string marathonPublicKey)
        {
            CheckDbContext(db);
            var marathon = GetMarathonByPublicKey(db, marathonPublicKey);
            return GetConsultantByMarathon(db, marathon);
        }

        /// <summary>
        /// Возвращает клавиатуру динамически в зависимости от уровня пользователя
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public static MarkupWrapper<ReplyKeyboardMarkup> GetMainKeyboardforUser(BotDbContext db, long chatId)
        {
            CheckDbContext(db);

            User user = (from u in db.Users
                where u.ChatId == chatId
                select u).First();

            return GetMainKeyboardforUser(db, user);
        }

        /// <summary>
        /// Возвращает клавиатуру динамически в зависимости от уровня пользователя
        /// </summary>
        public static MarkupWrapper<ReplyKeyboardMarkup> GetMainKeyboardforUser(BotDbContext db, User user)
        {
            CheckDbContext(db);

            if (user.ChatId == Constants.SupportChatId)
            {
                return Keyboards.MainUserKeyboardWithAdminButton;
            }

            switch (user.Role)
            {
                case Constants.ROLE_USER:
                    return Keyboards.MainUserKeyboard;
                    break;
                case Constants.ROLE_MODERATOR:
                    return Keyboards.MainUserKeyboardWithModeratorButton;
                    break;
                case Constants.ROLE_ADMIN:
                    return Keyboards.MainUserKeyboardWithAdminButton;
                    break;
            }

            return Keyboards.MainUserKeyboard;
        }

        /// <summary>
        /// Создать сущность марафона и привязать к консультанту
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        /// <param name="marathonName"></param>
        /// <returns></returns>
        public static bool CreateMarathonEntity(BotDbContext db, long chatId, string marathonName)
        {
            CheckDbContext(db);

            var r = new Random(DateTime.Now.Minute + DateTime.Now.Millisecond);
            string publicKey = (r.Next(0, 90000) + 10000).ToString();

            Marathon marathon = new Marathon()
            {
                ModeratorChatId = chatId,
                Name = marathonName,
                Status = Constants.MARATHON_STATUS_ACTIVE,
                PublicKey = publicKey,
            };

            db.Marathons.Add(marathon);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Вернуть Inline клавиатуру (список марафонов)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public static List<Marathon> GetMarathonsByConsultantChatId(BotDbContext db, long chatId)
        {
            CheckDbContext(db);

            return (from m in db.Marathons
                where m.ModeratorChatId == chatId && m.Status == Constants.MARATHON_STATUS_ACTIVE
                select m).ToList();

        }

        /// <summary>
        /// переименовать марафон
        /// </summary>
        /// <param name="db"></param>
        /// <param name="publicKey"></param>
        /// <param name="newName"></param>
        public static void RenameMarathonByMarathonPublicKey(BotDbContext db, string publicKey, string newName)
        {
            CheckDbContext(db);
            var marathon = GetMarathonByPublicKey(db, publicKey);
            marathon.Name = newName;
            db.SaveChanges();
        }

        /// <summary>
        /// Удалить марафон по публичному ключу.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="publicKey"></param>
        public static void DeleteMarathonByMarathonPublicKey(BotDbContext db, string publicKey)
        {
            CheckDbContext(db);
            var marathon = GetMarathonByPublicKey(db, publicKey);
            if (Equals(marathon, null) == false)
            {
                marathon.Status = Constants.MARATHON_STATUS_DELETED;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// получить список всех активных участников марафона
        /// </summary>
        /// <param name="db"></param>
        /// <param name="marathon"></param>
        /// <returns></returns>
        public static List<User> GetAllParticipantsForMarathon(BotDbContext db, Marathon marathon)
        {
            if (Equals(marathon, null) == true) return null;

            CheckDbContext(db);

            return (from u in db.Users
                where u.ActiveMarathonPublicKey == marathon.PublicKey
                select u)?.ToList();
        }

        /// <summary>
        /// Получить количество участников марафона
        /// </summary>
        /// <param name="db"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static int GetCountParticipantsOfMarathon(BotDbContext db, string publicKey)
        {
            CheckDbContext(db);
            return (from u in db.Users
                where u.ActiveMarathonPublicKey == publicKey
                select u).Count();
        }

        /// <summary>
        /// Изменить уровень пользователя на консультанта и обратно
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userChatId"></param>
        /// <param name="approveToConsultant"></param>
        public static void ChangeUserToConsultant(BotDbContext db, User user, bool approveToConsultant)
        {
            CheckDbContext(db);
            
            if (approveToConsultant)
            {
                user.Role = Constants.ROLE_MODERATOR;
            }
            else
            {
                user.Role = Constants.ROLE_USER;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Создать в базе элемент QueryToSupport
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        public static void CreateConsultantQueryToSupportEntity(BotDbContext db, long chatId)
        {
            CheckDbContext(db);

            //Если запрос уже существует, то не добавлять
            if (Equals(GetConsultantQueryToSupportByChatId(db, chatId), null) == false) return;

            QueryToSupport q = new QueryToSupport()
            {
                Type = Constants.QUERY_CONSULTANT,
                FromChatId = chatId,
            };

            db.QueryToSupport.Add(q);
            db.SaveChanges();
        }


        /// <summary>
        /// Получить запрос по ChatId
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        public static QueryToSupport GetConsultantQueryToSupportByChatId(BotDbContext db, long chatId)
        {
            CheckDbContext(db);
            return (from q in db.QueryToSupport
                where q.FromChatId == chatId && q.Type == Constants.QUERY_CONSULTANT
                select q)?.FirstOrDefault();
        }

        /// <summary>
        /// Удалить запрос
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        public static void DeleteConsultantQueryByChatId(BotDbContext db, long chatId)
        {
            CheckDbContext(db);
            var q = GetConsultantQueryToSupportByChatId(db, chatId);
            if (q != null)
            {
                db.QueryToSupport.Remove(q);
                db.SaveChanges();
            }
            
        }

        /// <summary>
        /// Получить все запросы от пользователей
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        public static List<QueryToSupport> GetAllConsultantQueryToSupport(BotDbContext db)
        {
            CheckDbContext(db);
            return db.QueryToSupport.ToList();
        }


        public static int ConsultantsCoundInDb(BotDbContext db)
        {
            CheckDbContext(db);

            return (from u in db.Users
                where u.Role == Constants.ROLE_MODERATOR
                select u)?.Count() ?? 0;
        }

        public static int GetCountParticipants(BotDbContext db)
        {
            CheckDbContext(db);
            return (from u in db.Users
                where string.IsNullOrWhiteSpace(u.ActiveMarathonPublicKey) == false
                select u)?.Count() ?? 0;
        }

        public static int GetUsersCount(BotDbContext db)
        {
            CheckDbContext(db);

            return db.Users.Count();
        }

        public static int GetActiveMarathonsCount(BotDbContext db)
        {
            CheckDbContext(db);

            return (from m in db.Marathons
                    where m.Status == Constants.MARATHON_STATUS_ACTIVE
                    select m)?.Count() ?? 0;
        }

        public static void SaveTextMessageToDb(InboxMessage mes)
        {
            if (mes.Type != MessageType.Text) return;

            using (BotDbContext db = new BotDbContext(HelperDataBase.DB_OPTIONS))
            {
                Message message = new Message()
                {
                    ChatId = mes.ChatId,
                    Text = mes.Data as string,
                };

                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public static int GetMessagesCountFromDate(BotDbContext db, DateTime fromDate)
        {
            CheckDbContext(db);

            return (from m in db.Messages
                where m.CreateTime >= fromDate.Date
                select m)?.Count() ?? 0;
        }


        public static Dictionary<Marathon, List<User>> GetAllActiveMarathonsWithParticipants(BotDbContext db)
        {
            Dictionary<Marathon, List<User>> res = new Dictionary<Marathon, List<User>>();
            var allMarathons = GetAllActiveMarathons(db);

            for (int i = 0; i < allMarathons?.Count; i++)
            {
                var participants = GetAllParticipantsForMarathon(db, allMarathons[i]);

                if (participants == null || participants.Count == 0)
                {
                    allMarathons.RemoveAt(i);
                    i--;
                    continue;
                }

                res.Add(allMarathons[i], participants);
            }

            return res;
        }

        public static List<Marathon> GetAllActiveMarathons(BotDbContext db)
        {
            CheckDbContext(db);

            return (from m in db.Marathons
                where m.Status == Constants.MARATHON_STATUS_ACTIVE
                select m)?.ToList();
        }


        public static int GetGroupUsersMessagesCountFromDate(BotDbContext db, List<long> usersChatId, DateTime fromDate)
        {
            return (from m in db.Messages
                    where usersChatId.Contains(m.ChatId) &&
                          m.CreateTime.Date >= fromDate.Date
                    select m)?.Count() ?? 0;
        }


        public static List<User>GetAllActiveConsultants(BotDbContext db)
        {
            return (from c in db.Users
                where c.Role == Constants.ROLE_MODERATOR &&
                      c.Active != false
                select c)?.ToList() ?? null;
        }

        public static int GetMarathonsCountByConsultant(BotDbContext db, User consultant)
        {
            return (from m in db.Marathons
                where m.ModeratorChatId == consultant.ChatId &&
                      m.Status == Constants.MARATHON_STATUS_ACTIVE
                select m)?.Count() ?? 0;
        }

        public static int GetConsultantParticipantsCount(BotDbContext db, List<string> consultantMarathonsPublicKey)
        {
            return (from u in db.Users
                where consultantMarathonsPublicKey.Contains(u.ActiveMarathonPublicKey)
                select u)?.Count() ?? 0;
        }
    }
}
