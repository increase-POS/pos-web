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

        public async Task<ActionResult> Index(DashBoardModel dashBoardModel, int redirect)
        {
            #region set session values
            if (Request.Cookies["Cookie1"] != null && redirect != 1)
            {
                Session["UserName"] = HttpUtility.UrlDecode(Request.Cookies["Cookie1"].Values["UserName"]);
                Session["UserID"] = Request.Cookies["Cookie1"].Values["UserID"];
                Session["Image"] = Request.Cookies["Cookie1"].Values["Image"];
                Session["lang"] = Request.Cookies["Cookie1"].Values["lang"];
                Session["isAdmin"] = Request.Cookies["Cookie1"].Values["isAdmin"];

                #region get user permissions
                PermissionModel permissionModel = new PermissionModel();
                permissionModel = await permissionModel.GetPermissions(int.Parse(Session["UserID"].ToString()));
                Session["showDashBoard"] = permissionModel.showDashBoard;
                Session["showAccountRep"] = permissionModel.showAccountRep;
                Session["showStock"] = permissionModel.showStock;
                Session["showDelivery"] = permissionModel.showDelivery;
                #endregion

                AccountController ac = new AccountController();
                ac.ControllerContext = new ControllerContext(this.Request.RequestContext, ac);

                return ac.RedirectUser();
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
            List<BranchModel> branches = new List<BranchModel>();
            if (int.Parse(Session["UserID"].ToString()) == 2)
                branches = await branchModel.GetAll("all");
            else
                branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));
            ViewBag.branches = branches;
            #endregion

            DateTime dt;
            if (dashBoardModel.startDate == null)
                dt = DateTime.Now;
            else dt = (DateTime)dashBoardModel.startDate;

            dashBoardModel = await dashBoardModel.GetDashBoardInfo(dashBoardModel.branchId, dt, dashBoardModel.endDate, int.Parse(Session["UserID"].ToString()));
            
            
            return View(dashBoardModel);
        }

        [HttpPost]
        public async Task<ActionResult> RefreshDashBoard(int branchId)
        {
            DashBoardModel dashBoardModel = new DashBoardModel();
            DateTime dt = DateTime.Now;
            dashBoardModel = await dashBoardModel.GetDashBoardInfo(branchId, dt, dashBoardModel.endDate, int.Parse(Session["UserID"].ToString()));

            JsonResult result = this.Json(new
            {
                purchasesCount = dashBoardModel.purchasesCount,
                salesCount = dashBoardModel.salesCount,
                balance = dashBoardModel.balance,
                onLineUsersCount = dashBoardModel.onLineUsersCount
            }, JsonRequestBehavior.AllowGet);

            return result;
        }



        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Increase Group.";

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