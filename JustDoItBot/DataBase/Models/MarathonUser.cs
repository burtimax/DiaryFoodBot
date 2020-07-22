using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDiaryBot.DataBase.Models;

namespace JustDoItBot.DataBase.Models
{
    public class MarathonUser
    {
        public int MarathonId { get; set; }
        public Marathon Marathon { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
