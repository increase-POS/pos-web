using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace posWebApp.Models
{
    public class DashBoardModel
    {
        public int branchId { get; set; }
        public int purchasesCount { get; set; }
        public int salesCount { get; set; }
        public int vendorsCount { get; set; }
        public int customersCount { get; set; }
        public int onLineUsersCount { get; set; }
        public decimal balance { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }


        public async Task<DashBoardModel> GetDashBoardInfo(int? branchId, DateTime startDate, DateTime? endDate,int userId)
        {
            DashBoardModel items = new DashBoardModel();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("startDate", startDate.ToString());
            parameters.Add("endDate", endDate.ToString());
            parameters.Add("userId", userId.ToString());
            // 
            IEnumerable<Claim> claims = await APIResult.getList("WebDashBoard/GetDashBoardInfo", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items = JsonConvert.DeserializeObject<DashBoardModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return items;
        }

        public async Task getAccuracy()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            string method = "WebDashBoard/getAccuracy";
            int accuracy = await APIResult.post(method, parameters);
            Global.accuracy = accuracy.ToString();
        }
        public async Task getDefaultRegion()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            string method = "WebDashBoard/getCurrency";
            Global.currency = await APIResult.getString(method, parameters);
        }
    }
}