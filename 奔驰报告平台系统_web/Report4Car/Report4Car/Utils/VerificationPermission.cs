using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Report4Car.Utils
{
    public class VerificationPermission : AuthorizeAttribute
    {
        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContextBase context = filterContext.HttpContext;
            if (context.Session["User"] == null)
            {
                FormsAuthentication.SignOut();
                context.Response.Redirect(FormsAuthentication.LoginUrl + "?ReturnUrl=" + context.Server.UrlEncode(context.Request.Url.PathAndQuery));
            }
        }
    }
}