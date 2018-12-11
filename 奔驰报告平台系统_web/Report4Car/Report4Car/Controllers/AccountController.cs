using Report4Car.Dtos;
using Report4Car.Models;
using Report4Car.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Report4Car.Controllers
{
    public class AccountController : Controller
    {
        private const String _encryptKey = "76159843";
        //默认密钥向量  
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.ReturnUrl = HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]);
            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginError()
        {
            return View();
        }
        /// <summary>
        /// 用于本系统自己登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (model.Password == "!1234&qwe.pwd")//!1234&qwe.pwd
            {
                try
                {
                    ReportService service = new ReportService();
                    string role = service.GetUserRole(model.UserName, "MB1803");
                    Session["user"] = new User() { ID = model.UserName, Role = role, ProjectCode = "MB1803", Year = "2018" };
                    FormsAuthentication.SetAuthCookie(model.UserName + "|" + role+"|" + "MB1803", true, "");
                    if (Request.QueryString["ReturnUrl"] != null)
                        //跳转到登录前页面
                        return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                    else
                        return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.InnerException.ToString();
                }
            }
            else
            {
                ViewBag.Message = "账号或密码有误，登录失败!";
            }

            return View(model);
        }
        /// <summary>
        /// 从外部系统登陆进来
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ExLogin(String id)
        {
            try
            {
               
                string info = DecryptString(id);
                String[] user = info.Split('|');
                String barndAndQuarter = user[0];
                String year = user[1];
                String userId = user[2];
                string count = "";
                if (user.Length > 3)
                {
                    count = user[3];
                }
                // 2015之前的命名规则和之后的规则不同，所以分开进行处理
                if (year != "2015")
                {
                    if (barndAndQuarter.ToUpper().Contains("MBH"))
                    {
                        barndAndQuarter = barndAndQuarter.Insert(3, year.Substring(2, 2));
                    }
                    else if (barndAndQuarter.ToUpper().Contains("MB"))
                    {
                        barndAndQuarter = barndAndQuarter.Insert(2, year.Substring(2, 2));
                    }
                    else if (barndAndQuarter.ToUpper().Contains("VAN"))
                    {
                        barndAndQuarter = barndAndQuarter.Insert(3, year.Substring(2, 2)) + count;
                        log(id + " " + barndAndQuarter +  " " + year + " " + " " + userId);
                    }
                }
                ReportService service = new ReportService();
                string role = service.GetUserRole(userId, barndAndQuarter);
                if (role == "Other")
                {
                    //return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                }
                Session["user"] = new User() { ID = userId, Role = role, ProjectCode = barndAndQuarter, Year = year };
                FormsAuthentication.SetAuthCookie(userId + "|" + role+"|"+ barndAndQuarter, true, "");
                if (Request.QueryString["ReturnUrl"] != null)
                    //跳转到登录前页面
                    return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception exx)
            {
                log(exx.InnerException.ToString());
                return RedirectToAction("LoginError", "Account");

            }
        }
        /// <summary>
        /// 对于报错的情况自动留下日志
        /// </summary>
        /// <param name="message"></param>
        public static void log(string message)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = appDomainPath + @"\" + "Log" + @"\" + DateTime.Now.ToString("yyyyMMdd") + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            //File.Create(fileName);
            if (!Directory.Exists(appDomainPath + @"\" + "Log"))
            {
                Directory.CreateDirectory(appDomainPath + @"\" + "Log");
            }
            if (!Directory.Exists(appDomainPath + @"\" + "Log" + @"\" + DateTime.Now.ToString("yyyyMMdd")))
            {
                Directory.CreateDirectory(appDomainPath + @"\" + "Log" + @"\" + DateTime.Now.ToString("yyyyMMdd"));
            }
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                byte[] by = WriteStringToByte(message, fs);
                fs.Flush();
            }
        }
        /// <summary>
        /// 日志写入文档的方法
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static byte[] WriteStringToByte(string str, FileStream fs)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(str);
            fs.Write(info, 0, info.Length);
            return info;
        }
        /// <summary>
        /// 注销操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["user"] = null;
            if (Request.QueryString["ReturnUrl"] != null)
                //跳转到登录前页面
                return Redirect(HttpUtility.UrlDecode(Request.QueryString["ReturnUrl"]));
            else
                return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 登陆信息加密和解密的过程
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <returns></returns>
        private string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return decryptString;
            }
        }

        private string DecryptString(string encryptString)
        {
            encryptString = encryptString.Replace(' ', '+');
            String decryptString = DecryptDES(encryptString, _encryptKey);
            String[] infoArray = decryptString.Split('|');
            return decryptString;
        }

    }
}
