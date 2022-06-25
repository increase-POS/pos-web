using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace posWebApp.Models
{
    public class Global
    {
        //public static string APIUri = "http://localhost:107/api/";
       public static string APIUri = "";

        #region pagination settings
        public static int rowsInPage = 5;
        #endregion
        #region folders pathes
        public const string TMPFolder = "C:/Temp/Thumb";
        public const string TMPUsersFolder = "C:/Temp/Thumb/users";
        public const string TMPAgentsFolder = "C:/Temp/Thumb/agents";
        #endregion

        #region global parameters
        public static string accuracy;
        public static string currency;
        #endregion

    }
}