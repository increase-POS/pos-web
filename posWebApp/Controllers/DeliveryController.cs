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
            string invoiceStatus = "ex,tr,rc";
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

        public async Task<ActionResult> InvoiceDetails(int invoiceId)
        {
            InvoiceModel invoiceModel = new InvoiceModel();
            #region Invoice Details
            ItemTransfer itemTransfer = new ItemTransfer();

            invoiceModel = await invoiceModel.GetByInvoiceId(invoiceId);
            invoiceModel.InvoiceItems = await itemTransfer.GetInvoicesItems(invoiceModel.invoiceId);
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

            
            return View(invoiceModel);
        }

        public async Task<ActionResult> Confirm(int invoiceId)
        {
            InvoiceModel invoiceModel = new InvoiceModel();
            invoiceStatus st = new invoiceStatus();
            st.status = "tr";
            st.invoiceId = invoiceId;
            st.createUserId = int.Parse(Session["UserID"].ToString());
            st.isActive = 1;
            int res = await invoiceModel.saveOrderStatus(st);


            return RedirectToAction("DeliveryList", "Delivery");
        }
    }
}