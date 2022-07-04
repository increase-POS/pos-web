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
    public class AgentModel
    {
        #region attributes
        public int agentId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string company { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public string accType { get; set; }
        public float balance { get; set; }
        public Nullable<byte> balanceType { get; set; }
        public bool isLimited { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string notes { get; set; }
        public int isActive { get; set; }
        public string fax { get; set; }
        public string payType { get; set; }
        public decimal maxDeserve { get; set; }
        #endregion

        #region methods
        public async Task<List<AgentModel>> GetAgentsActive(string type)
        {
            List<AgentModel> items = new List<AgentModel>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string myContent = type;
            parameters.Add("type", myContent);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Agent/GetActive", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<AgentModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<AgentModel> getAgentById(int agentId)
        {
            AgentModel agent = new AgentModel();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("agentId", agentId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Agent/GetAgentByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    agent = JsonConvert.DeserializeObject<AgentModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return agent;
        }


        //public async Task<string> downloadImage(string imageName)
        //{
        //    Stream jsonString = null;
        //    byte[] byteImg = null;
        //    Image img = null;
        //    string imgDataURL = "";
        //    // ... Use HttpClient.
        //    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //    using (var client = new HttpClient())
        //    {
        //        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //        client.BaseAddress = new Uri(Global.APIUri);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
        //        client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
        //        HttpRequestMessage request = new HttpRequestMessage();
        //        request.RequestUri = new Uri(Global.APIUri + "Agent/GetImage?imageName=" + imageName);
        //        request.Method = HttpMethod.Get;
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            jsonString = await response.Content.ReadAsStreamAsync();
        //            img = Bitmap.FromStream(jsonString);
        //            byteImg = await response.Content.ReadAsByteArrayAsync();

        //            // configure trmporery path
        //            string dir = Directory.GetCurrentDirectory();
        //            var tmpPath = Path.Combine(dir, Global.TMPAgentsFolder);
        //            if (!Directory.Exists(tmpPath))
        //                Directory.CreateDirectory(tmpPath);
        //            tmpPath = Path.Combine(tmpPath, imageName);
        //            if (System.IO.File.Exists(tmpPath))
        //            {
        //                System.IO.File.Delete(tmpPath);
        //            }
        //            using (FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.ReadWrite))
        //            {
        //                fs.Write(byteImg, 0, byteImg.Length);
        //            }

        //            string imreBase64Data = Convert.ToBase64String(byteImg);
        //            imgDataURL = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);
        //        }
        //        return imgDataURL;
        //    }
        //}
        public async Task<string> downloadImage(string imageName)
        {
            byte[] byteImg = null;
            string imgDataURL = "";
            if (imageName != "")
            {
                byteImg = await APIResult.getImage("Agent/GetImage", imageName);

                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, Global.TMPAgentsFolder);
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
                imgDataURL = string.Format("data:image/;base64,{0}", imreBase64Data);
            }

            return imgDataURL;

        }
        #endregion
    }
}