using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using posWebApp.Models;
using System.Web.Security;
using posWebApp.Resources;
using System.Web.Routing;
//using System.Web.Http;

namespace posWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login(UserModel userModel)
        {
            try {
                #region set APIUri
                Global.APIUri = Properties.Settings.Default.APIUri;
                #endregion
                DashBoardModel dashBoardModel = new DashBoardModel();
         
                #region get user from cookie
                if (Request.Cookies["POSCookie"] != null )
                {
                FormsAuthentication.SetAuthCookie(userModel.username, false);

                string name = Request.Cookies["POSCookie"].Values["Image"];
                Session["UserName"] = Request.Cookies["POSCookie"].Values["UserName"];
                Session["UserID"] = Request.Cookies["POSCookie"].Values["UserID"];
                Session["Image"] = Request.Cookies["POSCookie"].Values["Image"];
                Session["lang"] = Request.Cookies["POSCookie"].Values["lang"];
                Session["isAdmin"] = Request.Cookies["POSCookie"].Values["isAdmin"];

                #region get user permissions
                PermissionModel permissionModel = new PermissionModel();
                permissionModel = await permissionModel.GetPermissions(int.Parse(Session["UserID"].ToString()));
                Session["showDashBoard"] = permissionModel.showDashBoard;
                Session["showAccountRep"] = permissionModel.showAccountRep;
                Session["showStock"] = permissionModel.showStock;
                Session["showDelivery"] = permissionModel.showDelivery;
                #endregion

                #region get accuracy
                await dashBoardModel.getAccuracy();
                await dashBoardModel.getDefaultRegion();

                #endregion

                #region redirect according to permission
                
                    return await RedirectUser();

                #endregion
            }
            #endregion
            #region first load
            if (userModel.username == "" || userModel.username == null)
            {
                ViewBag.message = "";
                return View(userModel);
            }
            #endregion
            #region after post
            string password = HelpClass.MD5Hash("Inc-m" + userModel.password);
            var user= await userModel.Getloginuser(userModel.username, password);
            if (user.userId == 0)
            {
                ViewBag.message = AppResource.InvalidLogin;
                return View(userModel);
            }
            else
            {
                #region get accuracy
                await dashBoardModel.getAccuracy();
                await dashBoardModel.getDefaultRegion();

                #endregion

                #region get user lang
                var lang  = await userModel.getUserLanguage(user.userId);
                #endregion

                #region get user permissions
                PermissionModel permissionModel = new PermissionModel();
                permissionModel = await permissionModel.GetPermissions(user.userId);

                if (userModel.userId == 2)
                    userModel.isAdmin = true;
                else
                    userModel.isAdmin = false;
                #endregion
                bool rememberMe = false;
                if (userModel.RememberMe)
                    rememberMe = true;

                userModel = user;

                #region remember me
                if (rememberMe)
                {
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                               (
                               1,
                               userModel.username,
                               DateTime.Now,
                               DateTime.Now.AddMinutes(60), // expiry
                               rememberMe,
                               "",
                               "/"
                               );

                    //encrypt the ticket and add it to a cookie
                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie("POSCookie", enTicket);
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                    cookie.HttpOnly = false;
                    cookie.Values.Add("UserName", HttpUtility.UrlEncode(userModel.fullName));
                    cookie.Values.Add("UserId", userModel.userId.ToString());
                    cookie.Values.Add("Image", userModel.image);
                    cookie.Values.Add("lang", lang);
                    cookie.Values.Add("isAdmin", userModel.isAdmin.ToString());

                    Response.Charset = "UTF-8";
                    Response.Cookies.Add(cookie);
                   

                    
                }
                else
                {
                    //give user authintication
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                              (
                              1,
                              userModel.username,
                              DateTime.Now,
                              DateTime.Now.AddMinutes(15), // expiry
                              false,
                              "",
                              "/"
                              );

                    //encrypt the ticket and add it to a cookie
                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie("POSCookie", enTicket);
                    cookie.Expires = DateTime.Now.AddMinutes(15);
                    cookie.HttpOnly = false;
                    cookie.Values.Add("UserName", HttpUtility.UrlEncode(userModel.fullName));
                    cookie.Values.Add("UserId", userModel.userId.ToString());
                    cookie.Values.Add("Image", userModel.image);
                    cookie.Values.Add("lang", lang);
                    cookie.Values.Add("isAdmin", userModel.isAdmin.ToString());


                    Response.Charset = "UTF-8";
                    Response.Cookies.Add(cookie);
                }

                FormsAuthentication.SetAuthCookie(userModel.username, false);
                #endregion


                Session["UserName"] = userModel.fullName;
                Session["UserID"] = userModel.userId;
                Session["Image"] = userModel.image;
                Session["info.image"] = "";
                Session["lang"] = lang;
                Session["isAdmin"] = userModel.isAdmin;

                Session["showDashBoard"] = permissionModel.showDashBoard;
                Session["showAccountRep"] = permissionModel.showAccountRep;
                Session["showStock"] = permissionModel.showStock;
                Session["showDelivery"] = permissionModel.showDelivery;

                #region redirect

               return await RedirectUser();
              
                #endregion
            }
            #endregion
        }
            catch
            {
                return RedirectToAction("Error", "Home");
    }
}


        public async Task<ActionResult> RedirectUser()
        {
            #region get user image
            try
            {
                if ((Session["Image"].ToString() != "" && Session["info.image"] == null) || (Session["info.image"] != null && Session["info.image"].ToString() == ""))
                {
                    UserModel user = new UserModel();
                    var imageArr = await user.downloadImage(Session["Image"].ToString());
                    Session["info.image"] = imageArr;//storing session.                   
                }
                else
                    Session["info.image"] = null;
            }
            catch
            {
                Session["info.image"] = null;
            }
            #endregion
            try
            { 
                if (Session["showDashBoard"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else if(bool.Parse(Session["showDashBoard"].ToString()) == true)
                {
                    return RedirectToAction("Index", "Home",new { redirect = 1});
                }
                else if (bool.Parse(Session["showAccountRep"].ToString()) == true)
                {
                    return RedirectToAction("Customers", "Agent");
                }
                else if (bool.Parse(Session["showDelivery"].ToString()) == true)
                {
                    return RedirectToAction("DeliveryList", "Delivery");
                }
                else if (bool.Parse(Session["showStock"].ToString()) == true)
                {
                    return RedirectToAction("Stock", "Stock");
                }
                else
                {
                    return RedirectToAction("About", "Home");

                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult Logout()
        {
            try { 
                //clear cookie
                if (Request.Cookies["POSCookie"] != null)
                {
                    var c = new HttpCookie("POSCookie");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }

                // remove authintication
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}