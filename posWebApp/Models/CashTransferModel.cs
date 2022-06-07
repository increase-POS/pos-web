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
    public class CashTransferModel
    {
        public int cashTransId { get; set; }
        public string transType { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> invId { get; set; }
        public string transNum { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<decimal> cash { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> createUserId { get; set; }
        public string notes { get; set; }
        public Nullable<int> posIdCreator { get; set; }
        public Nullable<byte> isConfirm { get; set; }
        public Nullable<int> cashTransIdSource { get; set; }
        public string side { get; set; }
        public string docName { get; set; }
        public string docNum { get; set; }
        public string docImage { get; set; }
        public Nullable<int> bankId { get; set; }
        public string bankName { get; set; }
        public string agentName { get; set; }
        public string usersName { get; set; }// side=u
        public string posName { get; set; }
        public string pos2Name { get; set; }
        public Nullable<int> pos2Id { get; set; }
        public string posCreatorName { get; set; }
        public int cashTrans2Id { get; set; }
        public Nullable<byte> isConfirm2 { get; set; }
        public string processType { get; set; }
        public Nullable<int> cardId { get; set; }
        public string createUserName { get; set; }
        public string createUserJob { get; set; }
        public string createUserLName { get; set; }
        public string usersLName { get; set; } // side=u
        public string cardName { get; set; }// processType=card
        public string reciveName { get; set; }
        public Nullable<int> bondId { get; set; }
        public Nullable<System.DateTime> bondDeserveDate { get; set; }
        public Nullable<byte> bondIsRecieved { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }
        public string shippingCompanyName { get; set; }
        public string userAcc { get; set; }

        #region methods
        public async Task<List<CashTransferModel>> GetCustomerPayments(int agentId)
        {
            List<CashTransferModel> items = new List<CashTransferModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("agentId", agentId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("WebDashBoard/GetCustomerPayments", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<CashTransferModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }
}