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
    public class ItemTransfer
    {

        #region Attributes
        public string itemName { get; set; }
        public string unitName { get; set; }
        public Nullable<long> quantity { get; set; }

        #endregion

        #region methods
        public async Task<List<ItemTransfer>> GetInvoicesItems(int invoiceId)
        {
            List<ItemTransfer> items = new List<ItemTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", invoiceId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("ItemsTransfer/Get", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ItemTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }
}