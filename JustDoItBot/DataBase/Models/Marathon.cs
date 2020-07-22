using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.DataBase.Models;

namespace FoodDiaryBot.DataBase.Models
{
    public class Marathon
    {
        public int Id { get; set; }
        public string PublicKey { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }//(active/deleted)
        public long ModeratorChatId { get; set; }
        public List<MarathonUser> Users { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }

        public Marathon()
        {
            this.Users = new List<MarathonUser>();
        }
    }
}
