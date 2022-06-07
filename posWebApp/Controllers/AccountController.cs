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
//using System.Web.Http;

namespace posWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
       [AllowAnonymous]
        public async Task<ActionResult> Login(UserModel userModel)
        {
            DashBoardModel dashBoardModel = new DashBoardModel();

            #region get user from cookie
            if (Request.Cookies["Cookie1"] != null && Request.Cookies["Cookie1"].Values["UserName"] == userModel.fullName)
            {
                string name = Request.Cookies["Cookie1"].Values["Image"];
                Session["UserName"] = Request.Cookies["Cookie1"].Values["UserName"];
                Session["UserID"] = Request.Cookies["Cookie1"].Values["UserID"];
                Session["Image"] = Request.Cookies["Cookie1"].Values["Image"];
                Session["lang"] = Request.Cookies["Cookie1"].Values["lang"];
                Session["isAdmin"] = Request.Cookies["Cookie1"].Values["isAdmin"];

                #region authintication
                //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                //               (
                //               1,
                //               Session["UserName"].ToString(),
                //               DateTime.Now,
                //               DateTime.Now.AddMinutes(15), // expiry
                //               false,
                //               "",
                //               "/"
                //               );

                ////encrypt the ticket and add it to a cookie
                //string enTicket = FormsAuthentication.Encrypt(authTicket);
                //HttpCookie cookie = new HttpCookie("Cookie1", enTicket);
                //cookie.Expires = DateTime.Now.AddMinutes(15);
                //cookie.HttpOnly = false;
                //cookie.Values.Add("UserName", Session["UserName"].ToString());
                //cookie.Values.Add("UserId", Session["UserID"].ToString());
                //cookie.Values.Add("Image", Session["Image"].ToString());

                //Response.Charset = "UTF-8";
                //Response.Cookies.Add(cookie);
                #endregion
                // FormsAuthentication.SetAuthCookie(Session["UserName"].ToString(), false);
                #region get accuracy
                await dashBoardModel.getAccuracy();
                await dashBoardModel.getDefaultRegion();

                #endregion
                if (bool.Parse(Session["isAdmin"].ToString()) == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("DeliveryList", "Delivery");
                }
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
            if (user.username == null)
            {
                ViewBag.message = AppResource.InvalidLogin;
                return View(userModel);
            }
            else
            {
                #region get user lang
                var lang  = await userModel.getUserLanguage(user.userId);
                #endregion

                #region get user role
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
                    HttpCookie cookie = new HttpCookie("Cookie1", enTicket);
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
                              DateTime.Now.AddMinutes(5), // expiry
                              false,
                              "",
                              "/"
                              );

                    //encrypt the ticket and add it to a cookie
                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie("Cookie1", enTicket);
                    cookie.Expires = DateTime.Now.AddMinutes(5);
                    cookie.HttpOnly = false;
                    cookie.Values.Add("UserName", HttpUtility.UrlEncode(userModel.fullName));
                    cookie.Values.Add("UserId", userModel.userId.ToString());
                    cookie.Values.Add("Image", userModel.image);
                    cookie.Values.Add("lang", lang);
                    cookie.Values.Add("isAdmin", userModel.isAdmin.ToString());

                    Response.Charset = "UTF-8";
                    Response.Cookies.Add(cookie);
                    //FormsAuthentication.SetAuthCookie(userModel.username, false);
                }

                FormsAuthentication.SetAuthCookie(userModel.username, false);
                #endregion


                Session["UserName"] = userModel.fullName;
                Session["UserID"] = userModel.userId;
                Session["Image"] = userModel.image;
                Session["info.image"] = "";
                Session["lang"] = lang;
                Session["isAdmin"] = userModel.isAdmin.ToString() ;

                #region get accuracy
                await dashBoardModel.getAccuracy();
                await dashBoardModel.getDefaultRegion();

                #endregion

                if (userModel.isAdmin == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("DeliveryList", "Delivery");
                }
            }
            #endregion

        }

        public ActionResult CeckAccount()
        {
            if (bool.Parse(Session["isAdmin"].ToString()) == true)
                return RedirectToAction("Index","Home");
            else
                return RedirectToAction("DeliveryList", "Delivery");

        }
        public ActionResult Logout()
        {
            //clear cookie
            if (Request.Cookies["Cookie1"] != null)
            {
                var c = new HttpCookie("Cookie1");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            // remove authintication
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<ActionResult> removeAuthentication(int agentId)
        {

            // remove authintication
            FormsAuthentication.SignOut();
            Session.Abandon();
           
            JsonResult result = this.Json(new
            {
                res = "sucssess"
            }, JsonRequestBehavior.AllowGet);

            return result;
        }
    }
}