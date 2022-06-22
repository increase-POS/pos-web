using posWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace posWebApp.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        // GET: Delivery
        public async Task<ActionResult> DeliveryList( int page = 1)
        {

            #region delivery man Invoices
            InvoiceModel invoiceModel = new InvoiceModel();
            string invoiceType = "s";
            string invoiceStatus = "Ready,Collected,InTheWay,Done";
            var invoices = await invoiceModel.getDeliverOrders(invoiceType, invoiceStatus,int.Parse(Session["UserID"].ToString()));

            invoices = invoices.OrderBy(x => x.sequence).ToList();
            var invoicesView = new InvoiceViewModel
            {
                Invoices = invoices,
                CurrentPage = page
            };

            #endregion
            return View(invoicesView);
        }

        public async Task<ActionResult> InvoiceDetails(int invoiceId, string status,int page)
        {
            InvoiceModel invoiceModel = new InvoiceModel();
            #region Invoice Details
            ItemTransfer itemTransfer = new ItemTransfer();

            invoiceModel = await invoiceModel.GetByInvoiceId(invoiceId);
            invoiceModel.InvoiceItems = await itemTransfer.GetInvoicesItems(invoiceModel.invoiceId);
            invoiceModel.status = status;
            #endregion

            #region customer info
            AgentModel agentModel = new AgentModel();
            agentModel = await agentModel.getAgentById((int)invoiceModel.agentId);
            invoiceModel.Agent = agentModel;


            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion

            ViewBag.Page = page;
            return View(invoiceModel);
        }

        public async Task<ActionResult> Confirm(int invoiceId)
        {
            InvoiceModel invoiceModel = new InvoiceModel();
            invoiceStatus st = new invoiceStatus();
            st.status = "Collected";
            st.invoiceId = invoiceId;
            st.createUserId = int.Parse(Session["UserID"].ToString());
            st.isActive = 1;
            int res = await invoiceModel.saveOrderStatus(st);


            return RedirectToAction("DeliveryList", "Delivery");
        }

        [HttpPost]
        public async Task<ActionResult> changeOrderStatus(int invoiceId, string status)
        {
            string nextStatus = "";

            #region status object

            invoiceStatus st = new invoiceStatus();
            st.invoiceId = invoiceId;
            st.createUserId = int.Parse(Session["UserID"].ToString());
            st.isActive = 1;
            switch (status)
            {
                case "Ready":
                    st.status = "Collected";
                    nextStatus = "InTheWay";
                    break;
                case "Collected":
                    st.status = "InTheWay";
                    nextStatus = "Done";
                    break;
                case "InTheWay":
                    st.status = "Done";
                    break;
            }
            #endregion

            InvoiceModel invoiceModel = new InvoiceModel();
            int res = await invoiceModel.saveOrderStatus(st);

            JsonResult result = this.Json(new
            {
                message = st.status,
                nextStatus = nextStatus,

            }, JsonRequestBehavior.AllowGet);

            return result;
        }
    }
}