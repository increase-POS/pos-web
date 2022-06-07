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
    public class ItemUnitModel
    {
        #region Attributes
        public int sequence { get; set; }
        public string itemName { get; set; }
        public string unitName { get; set; }
        public string branchName { get; set; }
        public Nullable<long> quantity { get; set; }

        #endregion


        #region methods
        public async Task<List<ItemUnitModel>> GetStockInfo(int branchId)
        {
            List<ItemUnitModel> items = new List<ItemUnitModel>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("branchId", branchId.ToString());
            // 
            IEnumerable<Claim> claims = await APIResult.getList("WebDashBoard/GetStockInfo", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ItemUnitModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }
}