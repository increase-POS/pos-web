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
    public class InvoiceModel
    {

        #region Attributes

        public int invoiceId { get; set; }
        public string invNumber { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> createUserId { get; set; }
        public string invType { get; set; }
        public string discountType { get; set; }
        public Nullable<decimal> discountValue { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> deserved { get; set; }
        public Nullable<System.DateTime> deservedDate { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> invoiceMainId { get; set; }
        public string invCase { get; set; }
        public Nullable<System.TimeSpan> invTime { get; set; }
        public string notes { get; set; }
        public string vendorInvNum { get; set; }
        public string name { get; set; }
        public string branchName { get; set; }
        public Nullable<System.DateTime> vendorInvDate { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> itemsCount { get; set; }
        public Nullable<decimal> tax { get; set; }
        public decimal cashReturn { get; set; }
        public Nullable<int> taxtype { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<byte> isApproved { get; set; }
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }
        public Nullable<int> shipUserId { get; set; }
        public string shipUserName { get; set; }
        public string shipCompanyName { get; set; }

        public string status { get; set; }
        public int invStatusId { get; set; }
        public decimal manualDiscountValue { get; set; }
        public string manualDiscountType { get; set; }
        public string createrUserName { get; set; }
        public decimal shippingCost { get; set; }
        public decimal realShippingCost { get; set; }
        public bool isActive { get; set; }
        public string payStatus { get; set; }
        public int sequence { get; set; }
        public string agentName { get; set; }

        public AgentModel Agent { get; set; }
        public List<ItemTransfer> InvoiceItems { get; set; }
        #endregion

        #region methods
        public async Task<List<InvoiceModel>> getAgentInvoices( int agentId, string type)
        {
            List<InvoiceModel> items = new List<InvoiceModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("agentId", agentId.ToString());
            parameters.Add("type", type);
            IEnumerable<Claim> claims = await APIResult.getList("Invoices/getInvoicesByAgentAndType", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<InvoiceModel>> getCustomerDeliverdOrders( int agentId)
        {
            List<InvoiceModel> items = new List<InvoiceModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("agentId", agentId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/getCustomerDeliverdOrders", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<List<InvoiceModel>> getDeliverOrders(string invType, string status, int userId)
        {
            List<InvoiceModel> items = new List<InvoiceModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invType", invType);
            parameters.Add("status", status);
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/getUserDeliverOrders", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<InvoiceModel> GetByInvoiceId(int itemId)
        {
            InvoiceModel item = new InvoiceModel();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Invoices/GetByInvoiceId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }

        public async Task<int> saveOrderStatus(invoiceStatus item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "InvoiceStatus/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        #endregion
    }

    public class invoiceStatus
    {
        public int invStatusId { get; set; }
        public Nullable<int> invoiceId { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string notes { get; set; }
        public Nullable<byte> isActive { get; set; }
    }
}