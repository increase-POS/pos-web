using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace posWebApp.Models
{
    public class UserModel
    {
        #region Attributes
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string fullName { get; set; }
        public string job { get; set; }
        public string workHours { get; set; }
        public string details { get; set; }
        public float balance { get; set; }
        public int balanceType { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createUserId { get; set; }
        public int? updateUserId { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public short? isActive { get; set; }
        public string notes { get; set; }
        public byte? isOnline { get; set; }
        public bool? isAdmin { get; set; }
        public string role { get; set; }
        public string image { get; set; }
        public Nullable<int> groupId { get; set; }
        public bool RememberMe { get; set; }
        #endregion

        #region methods
        public async Task<UserModel> Getloginuser(string userName, string password)
        {
            UserModel user = new UserModel();

            //########### to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userName", userName);
            parameters.Add("password", password);
            IEnumerable<Claim> claims = await APIResult.getList("Users/Getloginuser", parameters);
            //#################

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    user = JsonConvert.DeserializeObject<UserModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return user;
        }

        public async Task<string> getUserLanguage(int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId.ToString());

            string method = "WebDashBoard/getUserLanguage";
            return await APIResult.getString(method, parameters);
        }
        public async Task<string> downloadImage(string imageName)
        {
            Stream jsonString = null;
            byte[] byteImg = null;
            Image img = null;
            string imgDataURL = "";
            // ... Use HttpClient.
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                client.BaseAddress = new Uri(Global.APIUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(Global.APIUri + "Users/GetImage?imageName=" + imageName);
                //request.Headers.Add("APIKey", Global.APIKey);
                request.Method = HttpMethod.Get;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStreamAsync();
                    img = Bitmap.FromStream(jsonString);
                    byteImg = await response.Content.ReadAsByteArrayAsync();

                    // configure trmporery path
                    //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string dir = Directory.GetCurrentDirectory();
                    var tmpPath = Path.Combine(dir, Global.TMPUsersFolder);
                    if (!Directory.Exists(tmpPath))
                        Directory.CreateDirectory(tmpPath);
                    tmpPath = Path.Combine(tmpPath, imageName);
                    if (System.IO.File.Exists(tmpPath))
                    {
                        System.IO.File.Delete(tmpPath);
                    }
                    using (FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteImg, 0, byteImg.Length);
                    }

                    string imreBase64Data = Convert.ToBase64String(byteImg);
                   imgDataURL = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);
                }
                return imgDataURL;
            }
        }
        #endregion
    }
}