using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace posWebApp.Models
{
    public static class HelpClass
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        //public static string DecTostring(decimal? dec)
        //{
        //    string sdc = "0";
        //    if (dec == null)
        //    {

        //    }
        //    else
        //    {
        //        decimal dc = decimal.Parse(dec.ToString());

        //        switch (Global.accuracy)
        //        {
        //            case "0":
        //                sdc = string.Format("{0:F0}", dc);
        //                break;
        //            case "1":
        //                sdc = string.Format("{0:F1}", dc);
        //                break;
        //            case "2":
        //                sdc = string.Format("{0:F2}", dc);

        //                break;
        //            case "3":
        //                sdc = string.Format("{0:F3}", dc);
        //                break;
        //            default:
        //                sdc = string.Format("{0:F1}", dc);
        //                break;
        //        }
        //    }


        //    return sdc;
        //}
    }
}