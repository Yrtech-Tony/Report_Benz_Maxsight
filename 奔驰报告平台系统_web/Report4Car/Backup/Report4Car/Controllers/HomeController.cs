using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Report4Car.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DistributorReport()
        {
            return View();
        }
        public ActionResult ReportDownload()
        {
            BENZReportServer.ServiceSoapClient client = new BENZReportServer.ServiceSoapClient();
            DataSet ds = client.SearchProject();
            return View();
        }
    }
}
