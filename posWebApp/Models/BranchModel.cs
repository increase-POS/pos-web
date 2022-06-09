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
    public class BranchModel
    {
        public int branchId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string notes { get; set; }
        public Nullable<int> parentId { get; set; }
        public byte isActive { get; set; }
        public string type { get; set; }
        public Boolean canDelete { get; set; }
        public Nullable<decimal> balance { get; set; }

        public async Task<List<BranchModel>> GetBranchesActive(int userId)
        {
            List<BranchModel> items = new List<BranchModel>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId.ToString());
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Branches/GetByBranchUser", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<BranchModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<BranchModel>> GetAll(string type)
        {
            List<BranchModel> items = new List<BranchModel>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Branches/GetActive", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<BranchModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
    }
}