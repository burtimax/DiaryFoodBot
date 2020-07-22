using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FoodDiaryBot.Source.User;
using FoodDiaryBot.DataBase.Models;
using JustDoItBot.DataBase.Models;

namespace FoodDiaryBot.DataBase.Models
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Active { get; set; }
        public string Role { get; set; } //(user/moderator/admin)
        public string ActiveMarathonPublicKey { get; set; }
        public ChatState ChatState { get; set; }
        public List<MarathonUser> Marathons { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }

        public User()
        {
            this.Marathons = new List<MarathonUser>();
        }
    }

}
