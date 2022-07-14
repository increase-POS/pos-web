using posWebApp.Models;
using posWebApp.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace posWebApp.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        AgentModel agentModel = new AgentModel();

        #region customer view
        [HttpPost]
        public async Task<JsonResult> GetCustomers(string Prefix)
        {
            //Note : you can bind same list from database  
            var customers = await agentModel.GetAgentsActive("c");
            Session["Customers"] = customers;
            //Searching records from list using LINQ query 

            //var customerRes = customers.Where(s => s.name.ToLower().Contains(Prefix.ToLower())).Select(w => w).ToList();
            var customerRes = (from customer in customers
                                where customer.name.ToLower().Contains(Prefix.ToLower())
                                select new
                                {
                                    label = customer.name,
                                    val = customer.agentId
                                }).ToList();
            return Json(customerRes, JsonRequestBehavior.AllowGet);

        }

  
        //public async Task<ActionResult> Customers([Bind(Include = "agentId")]AgentModel agentModel)
        //{
        //    try { 
        //        //#region get customers
        //        //var customers = await agentModel.GetAgentsActive("c");
        //        //ViewBag.customers = customers;
        //        //Session["Customers"] = customers;
        //        //#endregion

        //        #region customer info
        //        if(agentModel.agentId != 0)
        //            agentModel = await agentModel.getAgentById(agentModel.agentId);

        //        var image = agentModel.image;
        //        if (image != null && image.ToString() != "")
        //        {
        //            var imageArr = await agentModel.downloadImage(agentModel.image);
        //            ViewBag.Image = imageArr;
        //        }
        //        #endregion
        //        return View(agentModel);
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }
        //}
        public async Task<ActionResult> Customers(int agentId=0)
        {
            try
            {

                #region customer info
                if (agentId != 0)
                    agentModel = await agentModel.getAgentById(agentId);

                var image = agentModel.image;
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await agentModel.downloadImage(agentModel.image);
                    ViewBag.Image = imageArr;
                }
                #endregion
                return View(agentModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetAgentInfo(int agentId)
        {
            try { 
                agentModel = await agentModel.getAgentById(agentId);
                var imageArr = await agentModel.downloadImage(agentModel.image);

                string balanceType = "";

                if(agentModel.balance != 0)
                switch (agentModel.balanceType)
                {
                    case 0: balanceType = AppResource.Worthy;
                        break;
                    case 1: balanceType = AppResource.Demands;
                        break;
                }

                JsonResult result = this.Json(new
                {
                    name = agentModel.name,
                    company = agentModel.company,
                    mobile = agentModel.mobile,
                    image = imageArr,
                    balance = agentModel.balance,
                    balanceType = balanceType,
                }, JsonRequestBehavior.AllowGet);

                return result;
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region sales invoices
 
        public async Task<ActionResult> SalesInvoices(int agentId,int page=1)
        {
            try { 
                //#region get customers
                //var customers = await agentModel.GetAgentsActive("c");
                //ViewBag.customers = customers;
                //#endregion

                #region customer info
                agentModel = await agentModel.getAgentById(agentId);
                var image = agentModel.image;
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await agentModel.downloadImage(agentModel.image);
                    ViewBag.Image = imageArr;
                }
                #endregion

                #region Customer Invoices
                InvoiceModel invoiceModel = new InvoiceModel();
                var invoices = await invoiceModel.getAgentInvoices(agentId, "s");

                var invoicesView = new InvoiceViewModel
                {
                    Invoices = invoices,
                    Agent = agentModel,
                    CurrentPage = page
                };
                //ViewBag.Invoices = invoices;
                #endregion
                return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        #endregion

        #region customer quotations

        public async Task<ActionResult> Quotations(int agentId, int page = 1)
        {
            try { 
                //#region get customers
                //var customers = await agentModel.GetAgentsActive("c");
                //ViewBag.customers = customers;
                //#endregion

                #region customer info
                agentModel = await agentModel.getAgentById(agentId);
                var image = agentModel.image;
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await agentModel.downloadImage(agentModel.image);
                    ViewBag.Image = imageArr;
                }
                #endregion

                #region Customer Invoices
                InvoiceModel invoiceModel = new InvoiceModel();
                var invoices = await invoiceModel.getAgentInvoices(agentId, "q");

                var invoicesView = new InvoiceViewModel
                {
                    Invoices = invoices,
                    Agent = agentModel,
                    CurrentPage = page
                };
                #endregion
                return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region customer orders

        public async Task<ActionResult> CustomerOrders(int agentId, int page = 1)
        {
            try { 
                //#region get customers
                //var customers = await agentModel.GetAgentsActive("c");
                //ViewBag.customers = customers;
                //#endregion

                #region customer info
                agentModel = await agentModel.getAgentById(agentId);
                var image = agentModel.image;
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await agentModel.downloadImage(agentModel.image);
                    ViewBag.Image = imageArr;
                }
                #endregion

                #region Customer Invoices
                InvoiceModel invoiceModel = new InvoiceModel();
                var invoices = await invoiceModel.getAgentInvoices(agentId, "or");

                var invoicesView = new InvoiceViewModel
                {
                    Invoices = invoices,
                    Agent = agentModel,
                    CurrentPage = page
                };
                #endregion
                return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion 
        
        #region customer payments

        public async Task<ActionResult> CustomerPayments(int agentId, int page = 1)
        {
            try { 
                //#region get customers
                //var customers = await agentModel.GetAgentsActive("c");
                //ViewBag.customers = customers;
                //#endregion

                #region customer info
                agentModel = await agentModel.getAgentById(agentId);
                var image = agentModel.image;
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await agentModel.downloadImage(agentModel.image);
                    ViewBag.Image = imageArr;
                }
                #endregion

                #region Customer Payments
                CashTransferModel cashTransfer = new CashTransferModel();
                var cashes = await cashTransfer.GetCustomerPayments(agentId);

                var cashTransferView = new CashTransferViewModel
                {
                    CashTransfers = cashes,
                    Agent = agentModel,
                    CurrentPage = page
                };
                #endregion
                return View(cashTransferView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion
        
        #region customer delivery

        public async Task<ActionResult> Delivery(int agentId, int page = 1)
        {
            try { 
            #region get customers
            var customers = await agentModel.GetAgentsActive("c");
            ViewBag.customers = customers;
            #endregion

            #region customer info
            agentModel = await agentModel.getAgentById(agentId);
            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion

            #region Customer deliveries
            InvoiceModel invoiceModel = new InvoiceModel();
            var invoices = await invoiceModel.getCustomerDeliverdOrders(agentId);

            var invoicesView = new InvoiceViewModel
            {
                Invoices = invoices,
                Agent = agentModel,
                CurrentPage = page
            };
            #endregion
            return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region vendor view
        //public async Task<ActionResult> Vendors(AgentModel agentModel)
        //{
        //    try { 
        //    #region get customers
        //    var vendors = await agentModel.GetAgentsActive("v");
        //    ViewBag.vendors = vendors;
        //    #endregion

        //    #region customer info
        //    if (agentModel.agentId != 0)
        //        agentModel = await agentModel.getAgentById(agentModel.agentId);

        //    var image = agentModel.image;
        //    if (image != null && image.ToString() != "")
        //    {
        //        var imageArr = await agentModel.downloadImage(agentModel.image);
        //        ViewBag.Image = imageArr;
        //    }
        //    #endregion
        //    return View(agentModel);
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }
        //}
        [HttpPost]
        public async Task<JsonResult> GetVendors(string Prefix)
        {
            //Note : you can bind same list from database  
            var vendors = await agentModel.GetAgentsActive("v");
            Session["Customers"] = vendors;
            //Searching records from list using LINQ query 

            //var customerRes = customers.Where(s => s.name.ToLower().Contains(Prefix.ToLower())).Select(w => w).ToList();
            var vendorRes = (from vendor in vendors
                               where vendor.name.ToLower().Contains(Prefix.ToLower())
                               select new
                               {
                                   label = vendor.name,
                                   val = vendor.agentId
                               }).ToList();
            return Json(vendorRes, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Vendors(int agentId=0)
        {
            try { 
            //#region get customers
            //var vendors = await agentModel.GetAgentsActive("v");
            //ViewBag.vendors = vendors;
            //#endregion

            #region customer info
            if (agentId != 0)
                agentModel = await agentModel.getAgentById(agentId);

            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion
            return View(agentModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion


        #region purchases invoices

        public async Task<ActionResult> PurchasesInvoices(int agentId, int page = 1)
        {
            try { 
            //#region get customers
            //var vendors = await agentModel.GetAgentsActive("v");
            //ViewBag.vendors = vendors;
            //#endregion

            #region customer info
            agentModel = await agentModel.getAgentById(agentId);
            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion

            #region Customer Invoices
            InvoiceModel invoiceModel = new InvoiceModel();
            var invoices = await invoiceModel.getAgentInvoices(agentId, "p");

            var invoicesView = new InvoiceViewModel
            {
                Invoices = invoices,
                Agent = agentModel,
                CurrentPage = page
            };
            #endregion
            return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        #endregion

        #region vendor orders

        public async Task<ActionResult> VendorOrders(int agentId, int page = 1)
        {
            try { 
            //#region get customers
            //var vendors = await agentModel.GetAgentsActive("v");
            //ViewBag.vendors = vendors;
            //#endregion

            #region customer info
            agentModel = await agentModel.getAgentById(agentId);
            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion

            #region Customer Invoices
            InvoiceModel invoiceModel = new InvoiceModel();
            var invoices = await invoiceModel.getAgentInvoices(agentId, "po");

            var invoicesView = new InvoiceViewModel
            {
                Invoices = invoices,
                Agent = agentModel,
                CurrentPage = page
            };
            #endregion
            return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

        #region vendor payments

        public async Task<ActionResult> VendorPayments(int agentId, int page = 1)
        {
            try { 
            //#region get customers
            //var vendors = await agentModel.GetAgentsActive("v");
            //ViewBag.vendors = vendors;
            //#endregion

            #region customer info
            agentModel = await agentModel.getAgentById(agentId);
            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion

            #region Customer Payments
            CashTransferModel cashTransfer = new CashTransferModel();
            var cashes = await cashTransfer.GetCustomerPayments(agentId);

            var cashTransferView = new CashTransferViewModel
            {
                CashTransfers = cashes,
                Agent = agentModel,
                CurrentPage = page
            };
            #endregion
            return View(cashTransferView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #endregion

       
    }
}