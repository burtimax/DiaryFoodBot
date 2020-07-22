using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDiaryBot.DataBase.Models;

namespace JustDoItBot.DataBase.Models
{
    public class Food
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        //public User User { get; set; }
        //public int UserId { get; set; }
        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }

    }
}
