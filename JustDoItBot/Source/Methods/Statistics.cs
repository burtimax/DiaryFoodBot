using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDiaryBot.DataBase.Context;
using JustDoItBot.DataBase.Models;
using JustDoItBot.Source.Constants;

namespace JustDoItBot.Source.Methods
{
    class Statistics
    {
        /// <summary>
        /// Сформировать строку статистики за последние countDays дней.
        /// Данные дневника
        /// </summary>
        /// <param name="db"></param>
        /// <param name="chatId"></param>
        /// <param name="countDays"></param>
        /// <returns></returns>
        public string GetUserStatistics(BotDbContext db, long chatId, int countDays, string userName = null)
        {
            string res = null;
            string title = null;
            if (string.IsNullOrWhiteSpace(userName) == false)
            {
                title = Answer.FoodDiaryOfUsername(userName, countDays);
            }

            DateTime StartDate = DateTime.Today.AddDays((-1) * countDays + 1);
            DateTime EndDate = DateTime.Today;

            var foodGroups = 
                (from f in db.Foods
                where f.ChatId == chatId && f.CreateTime.Date>=StartDate.Date
                select f).ToList();

            var waterGroups = (from w in db.Waters
                where w.ChatId == chatId && w.CreateTime.Date >= StartDate.Date
                select w).ToList();

            var weightGroup = (from we in db.Weights
                where we.ChatId == chatId && we.CreateTime.Date >= StartDate.Date
                select we).ToList();

            DateTime cur = StartDate;

            while (cur.Date.Ticks <= EndDate.Ticks)
            {
                string dayRes = null;

                double mass = -1;
                mass = weightGroup.Where(w => w.CreateTime.Date == cur.Date)?.LastOrDefault()?.Mass ?? -1;
                if (mass > -1)
                {
                    dayRes += $"Вес: {mass} кг.\n";
                }

                var curWater = waterGroups?.Where(g => g.CreateTime.Date == cur.Date)?.LastOrDefault()?.Quantity ?? -1;
                if (curWater > -1)
                {
                    dayRes += $"Вода: {curWater} мл.\n";
                }

                var curFood = foodGroups?.Where(g => g.CreateTime.Date == cur.Date);
                if (curFood?.Count() > 0)
                {
                    dayRes += "Еда:\n";
                }
                foreach(var f in curFood)
                {
                    dayRes += $"[{f.CreateTime.ToShortTimeString()}] {f.Description}\n";
                }

                if (string.IsNullOrEmpty(dayRes) == false)
                {
                    res += $"{cur.Date.ToShortDateString()}\n" + dayRes + "\n";
                }

                cur = cur.AddDays(1);
            }

            if(string.IsNullOrWhiteSpace(res) == true)
            {
                res = Answer.NoData;
            }

            if (string.IsNullOrEmpty(title) == false)
            {
                res = title + res;
            }

            return res;
        }


    }
}
