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
            #region get branches
            BranchModel branchModel = new BranchModel();
            var branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));
            ViewBag.branches = branches;
            #endregion

            #region items units quantity
            ItemUnitModel itemUnit = new ItemUnitModel();
            var itemsUnits = await itemUnit.GetStockInfo(itemUnitViewModel.branchId);

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

        // GET: Stock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Stock/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Stock/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
