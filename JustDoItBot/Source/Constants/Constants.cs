using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FoodDiaryBot.Source.Constants
{
    public class Constants
    {
        public static string DIRECTORY
        {
            get { return Directory.GetCurrentDirectory(); }
        }

        public static string CONNECTION_STRING_FILEPATH
        {
            get { return DIRECTORY + "\\" + "connection_database.txt"; }
        }

        /// <summary>
        /// Chat Id of Support
        /// </summary>
        public static long SupportChatId = 672312299;
       
        
        public const string ROLE_USER = "user";
        public const string ROLE_MODERATOR = "moderator";
        public const string ROLE_ADMIN = "admin";
        
        public const string MARATHON_STATUS_ACTIVE = "active";
        public const string MARATHON_STATUS_DELETED = "deleted";

        public const string QUERY_CONSULTANT = "consultant";

    }
}
