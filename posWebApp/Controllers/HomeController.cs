using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        BranchModel branchModel = new BranchModel();

        public async Task<ActionResult> Index(DashBoardModel dashBoardModel)
        {
            #region set session values
            if (Request.Cookies["Cookie1"] != null)
            {
                Session["UserName"] = HttpUtility.UrlDecode(Request.Cookies["Cookie1"].Values["UserName"]);
                Session["UserID"] = Request.Cookies["Cookie1"].Values["UserID"];
                Session["Image"] = Request.Cookies["Cookie1"].Values["Image"];
            }
            #endregion
           
            #region get user image
            UserModel user = new UserModel();
            var image = Session["Image"];
            
            if ((Session["Image"].ToString() != "" && Session["info.image"] == null) || (Session["info.image"] != null && Session["info.image"].ToString() == "") )
            {
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await user.downloadImage(Session["Image"].ToString());
                    Session["info.image"] = imageArr;//storing session.
                                                     //ViewBag.Image = imageArr;
                }
                else
                {
                    Session["info.image"] = "";
                }
            }
            #endregion
            #region get branches
            var branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));
            ViewBag.branches = branches;
            #endregion

            DateTime dt;
            if (dashBoardModel.startDate == null)
                dt = DateTime.Now;
            else dt = (DateTime)dashBoardModel.startDate;

            dashBoardModel = await dashBoardModel.GetDashBoardInfo(dashBoardModel.branchId, dt, dashBoardModel.endDate);
            
            
            return View(dashBoardModel);
        }

        [HttpPost]
        public async Task<ActionResult> RefreshDashBoard(int branchId)
        {
            DashBoardModel dashBoardModel = new DashBoardModel();
            DateTime dt = DateTime.Now;
            dashBoardModel = await dashBoardModel.GetDashBoardInfo(branchId, dt, dashBoardModel.endDate);

            JsonResult result = this.Json(new
            {
                purchasesCount = dashBoardModel.purchasesCount,
                salesCount = dashBoardModel.salesCount,
                customersCount = dashBoardModel.customersCount,
                vendorsCount = dashBoardModel.vendorsCount
            }, JsonRequestBehavior.AllowGet);

            return result;
        }

        //public FileResult GetFileFromBytes(byte[] bytesIn)
        //{
        //    return File(bytesIn, "image/jpeg");
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetUserImageFile()
        //{
        //    UserModel user = new UserModel();
        //    var image = Session["Image"];
        //    var imageArr = await user.downloadImage(Session["Image"].ToString());
        //    if (imageArr == null)
        //    {
        //        return null;
        //    }

        //    FileResult imageUserFile = GetFileFromBytes(imageArr);
        //    return imageUserFile;
        //}

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}