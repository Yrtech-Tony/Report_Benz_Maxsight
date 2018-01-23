using Report4Car.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Report4Car.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.ReturnUrl = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (model.UserName=="admin" && model.Password=="1234")
            {
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                if (Request.QueryString["ReturnUrl"] != null)
                    //跳转到登录前页面
                    return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "账号或密码有误，登录失败!";
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            if (Request.QueryString["ReturnUrl"] != null)
                //跳转到登录前页面
                return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
            else
                return RedirectToAction("Login", "Account");
        }
    }
}
