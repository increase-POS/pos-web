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
    public class StockController : Controller
    {
        // GET: Stock
        public async Task<ActionResult> Stock(ItemUnitViewModel itemUnitViewModel, int page = 1)
        {
            try { 
                #region get branches
                BranchModel branchModel = new BranchModel();
                List<BranchModel> branches = new List<BranchModel>();
                if (int.Parse(Session["UserID"].ToString()) == 2)
                    branches = await branchModel.GetAll("all");
                else
                    branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));

                ViewBag.branches = branches;
                #endregion

                #region items units quantity
                ItemUnitModel itemUnit = new ItemUnitModel();
                var itemsUnits = await itemUnit.GetStockInfo(itemUnitViewModel.branchId, int.Parse(Session["UserID"].ToString()));

                var itemsView = new ItemUnitViewModel
                {
                    ItemsUnits = itemsUnits,
                    CurrentPage = page,
                    branchId = itemUnitViewModel.branchId,
                };
                //ViewBag.Invoices = invoices;
                #endregion
                return View(itemsView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

       
    }
}
