using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using Report4Car.BENZReportServer;
using Report4Car.Dtos;
using Report4Car.Services;
using Report4Car.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using XHX.Common;

namespace Report4Car.Controllers
{
    [VerificationPermission]
    public class HomeController : Controller
    {
        public ReportService service = new ReportService();
        private Dictionary<string, TreeNode> dicTemp = new Dictionary<string, TreeNode>();
        public static void log(string message)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = appDomainPath + @"\" + "Log" + @"\" + DateTime.Now.ToString("yyyyMMdd") + @"\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
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
        #region View
        /// <summary>
        /// 首页区分是迈巴赫还是奔驰，根据不同的品牌显示不同 的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string index = "index";
            if (Session["user"] != null)
            {
                ViewBag.TreeNode = getTreeviewData();
                User user = Session["user"] as User;
                ViewBag.ProjectCode = user.ProjectCode;
                ProjectDto project = service.SearchProjectByProjectCode(user.ProjectCode);
                string type = user.ProjectCode.Substring(0, 3);
                if (type.ToUpper() == "VAN")
                {
                    ViewBag.ForTitle = "V级及威霆销售质量现场考核";
                    ViewBag.ForNo = project.Quarter;
                    ViewBag.CurrentYear = project.CurrentYear;
                    ViewBag.StartDate = project.StartDate;
                    ViewBag.EndDate = project.EndDate;
                    index = "index3";
                }
                else if (type.ToUpper() == "MBH")
                {
                    ViewBag.ForTitle = "梅赛德斯-迈巴赫销售明采调查";
                    ViewBag.ForNo = project.Quarter;
                    ViewBag.CurrentYear = project.CurrentYear;
                    ViewBag.StartDate = project.StartDate;
                    ViewBag.EndDate = project.EndDate;
                    index = "index2";
                }
                else
                {//MB
                    ViewBag.ForTitle = "梅赛德斯-奔驰销售明采调查";
                    ViewBag.ForNo = project.Quarter;
                    ViewBag.CurrentYear = project.CurrentYear;
                    ViewBag.StartDate = project.StartDate;
                    ViewBag.EndDate = project.EndDate;
                }

            }
            return View(index);
        }
        /// <summary>
        /// 根据区域的代码显示查询区域报告的基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AreaReport(string id)
        {
            ViewBag.ProjectCode = "";
            if (Session["user"] != null)
            {
                ViewBag.TreeNode = getTreeviewData();
                User user = Session["user"] as User;
                if (string.IsNullOrEmpty(id))
                {
                    id = string.IsNullOrEmpty(user.Remark) ? "" : user.Remark.Split('|')[0];
                }
                ViewBag.AreaCode = id;
                ProjectDto project = service.SearchProjectByProjectCode(user.ProjectCode);
                string type = user.ProjectCode.Substring(0, 3);
                ViewBag.ProjectCode = user.ProjectCode;
                ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", project.Quarter);
                //if (type.ToUpper() == "MBH")
                //{
                //    ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", user.ProjectCode.Substring(3));
                //}
                //else
                //{//MB
                //    ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", user.ProjectCode.Substring(2));
                //}
            }
            return View();
        }
        /// <summary>k
        /// /// 根据区域的代码显示查询区域报告的详细数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult DistributorReport(string id, string name)
        {
            ViewBag.ProjectCode = "";
            if (Session["user"] != null)
            {
                ViewBag.TreeNode = getTreeviewData();
                User user = Session["user"] as User;
                if (string.IsNullOrEmpty(id))
                {
                    id = string.IsNullOrEmpty(user.Remark) ? "" : user.Remark.Split('|')[1];
                }
                ViewBag.ShopCode = id;
                DataTable dt = service.SearchShopByShopCode(user.ProjectCode, id, "");
                if (dt.Rows.Count > 0)
                {
                    ViewBag.ShopInfo = string.Format("{0} > {1} > {2} {3}", dt.Rows[0]["BigAreaName"], dt.Rows[0]["SmallAreaName"], dt.Rows[0]["ShopCode"], dt.Rows[0]["ShopName"]);
                }
                ProjectDto project = service.SearchProjectByProjectCode(user.ProjectCode);
                string type = user.ProjectCode.Substring(0, 3);

                ViewBag.ProjectCode = user.ProjectCode;
                ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", project.Quarter);
                ViewBag.Type = type;
                //if (type.ToUpper() == "VAN")
                //{
                //    ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", project.Quarter);
                //}
                //else if (type.ToUpper() == "MBH")
                //{
                //    ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", user.ProjectCode.Substring(3));
                //}
                //else
                //{//MB
                //    ViewBag.ForNo = String.Format("第 {0} 季度经销商报告", user.ProjectCode.Substring(2));
                //}
            }
            return View();
        }
        /// <summary>
        /// 经销商报告下载
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportDownload()
        {
            ViewBag.TreeNode = getTreeviewData();
            ViewBag.RoleType = (Session["user"] as User).Role;
            return View();
        }
        /// <summary>
        /// 经销商报告下载
        /// </summary>
        /// <returns></returns>
        public ActionResult ShopRecord()
        {
            ViewBag.TreeNode = getTreeviewData();
            ViewBag.RoleType = (Session["user"] as User).Role;
            return View();
        }
        public ActionResult ShopReportUpload()
        {
            ViewBag.TreeNode = getTreeviewData();
            ViewBag.RoleType = (Session["user"] as User).Role;
            return View();
        }
        #endregion

        #region Treeview
        /// <summary>
        /// 页面左侧的菜单显示的数据查询，
        /// </summary>
        /// <returns></returns>
        private TreeNode getTreeviewData()
        {
            if (Session["user"] == null) return null;
            User user = Session["user"] as User;

            TreeNode allAreas = null;
            if (dicTemp.ContainsKey(user.ID + user.ProjectCode))
            {
                allAreas = dicTemp[user.ID + user.ProjectCode];
                return allAreas;
            }

            allAreas = new TreeNode();
            allAreas.Value = "";
            allAreas.Name = "全国";
            DataTable dtBigs = service.GetBigAreas(user.ID, user.ProjectCode);
            if (dtBigs != null && dtBigs.Rows.Count > 0)
            {
                allAreas.Nodes = new List<TreeNode>();
                bool isFirst = true;
                foreach (DataRow drBig in dtBigs.Rows)
                {
                    TreeNode treeBig = new TreeNode();
                    treeBig.Value = drBig["AreaCode"].ToString();
                    treeBig.Name = drBig["AreaName"].ToString();
                    DataTable dtSmalls = service.GetSmallAreas(user.ID, user.ProjectCode, treeBig.Value);
                    if (dtSmalls != null && dtSmalls.Rows.Count > 0)
                        treeBig.Nodes = new List<TreeNode>();
                    else
                        continue;
                    foreach (DataRow drSmall in dtSmalls.Rows)
                    {
                        TreeNode treeSmall = new TreeNode();
                        treeSmall.Value = drSmall["AreaCode"].ToString();
                        treeSmall.Name = drSmall["AreaName"].ToString();
                        DataTable dtShops = service.getShops(treeSmall.Value, user.ID, "", user.ProjectCode);
                        if (dtShops != null && dtShops.Rows.Count > 0)
                        {
                            treeSmall.Nodes = new List<TreeNode>();
                            if (isFirst)
                            {
                                (Session["user"] as User).Remark = drSmall["AreaCode"] + "|" + dtShops.Rows[0]["ShopCode"];
                                isFirst = false;
                            }
                        }
                        else
                            continue;

                        foreach (DataRow drShop in dtShops.Rows)
                        {
                            TreeNode treeShop = new TreeNode();
                            treeShop.Value = drShop["ShopCode"].ToString();
                            treeShop.Name = drShop["ShopName"].ToString();
                            treeSmall.Nodes.Add(treeShop);
                        }
                        treeBig.Nodes.Add(treeSmall);
                    }
                    allAreas.Nodes.Add(treeBig);
                }
            }
            dicTemp.Add(user.ID + user.ProjectCode, allAreas);
            return allAreas;
        }

        #endregion

        #region Filter
        /// <summary>
        /// 获取期号的信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProjects()
        {
            DataTable dt = service.GetProjects((Session["user"] as User).Year, (Session["user"] as User).ProjectCode);
            object dic = DT2Obj.ToObj(dt);
            return Json(dic);
        }
        /// <summary>
        /// 获取所有区域的信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBigAreas()
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt = service.GetBigAreas(user.ID, user.ProjectCode);
                object dic = DT2Obj.ToObj(dt);
                return Json(dic);
            }
            return null;
        }
        /// <summary>
        /// 获取小区的信息
        /// </summary>
        /// <param name="bigAreaCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSmallArea(string bigAreaCode)
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt = service.GetSmallAreas(user.ID, user.ProjectCode, bigAreaCode);
                object dic = DT2Obj.ToObj(dt);
                return Json(dic);
            }
            return null;
        }
        /// <summary>
        /// 获取经销商列表信息
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getShops(string groupCode, string smallAreaCode)
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt = service.getShops(smallAreaCode, user.ID, groupCode, user.ProjectCode);
                object dic = DT2Obj.ToObj(dt);
                return Json(dic);
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetGroups()
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt = service.GetGroups(user.ID);
                object dic = DT2Obj.ToObj(dt);
                return Json(dic);
            }
            return null;
        }
        #endregion
        /// <summary>
        /// 查询录音下载列表
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="bigAreaCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchShopRecord(string groupCode, string bigAreaCode, string smallAreaCode, string shopCode, int code)
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt1 = null;
                dt1 = service.SearchShopRecord(bigAreaCode, smallAreaCode, shopCode, "", "", "", "", user.ProjectCode, user.ID);
                object dic1 = DT2Obj.ToObj(dt1);
                return Json(new { data1 = dic1 });
            }
            return null;
        }
        /// <summary>
        /// 查询经销商报告上传时间
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="bigAreaCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchShopReportUpload(string groupCode, string bigAreaCode, string smallAreaCode, string shopCode, int code)
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt1 = null;
                dt1 = service.SearchShopReportUpload(bigAreaCode, smallAreaCode, shopCode, "", "", "", "", user.ProjectCode, user.ID);
                object dic1 = DT2Obj.ToObj(dt1);
                return Json(new { data1 = dic1 });
            }
            return null;
        }
        /// <summary>
        /// 查询经销商报告列表信息
        /// </summary>
        /// <param name="groupCode"></param>
        /// <param name="bigAreaCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchReportDownShop(string groupCode, string bigAreaCode, string smallAreaCode, string shopCode, int code)
        {
            if (Session["user"] != null)
            {
                User user = Session["user"] as User;
                DataTable dt1 = null, dt2 = null;
                switch (code)
                {
                    case 1:
                        dt1 = service.SearchReportDownShop_Area(bigAreaCode, smallAreaCode, shopCode, "", "", "", "", user.ProjectCode, user.ID);
                        break;
                    case 2:
                        dt2 = service.SearchReportDownShop_Group("", "", shopCode, "", "", groupCode, "", user.ProjectCode);
                        break;
                    case 3:
                        dt1 = service.SearchReportDownShop_Area(bigAreaCode, smallAreaCode, shopCode, "", "", "", "", user.ProjectCode, user.ID);
                        dt2 = service.SearchReportDownShop_Group("", "", shopCode, "", "", groupCode, "", user.ProjectCode);
                        break;
                }
                DataTable dt = service.GetProjects((Session["user"] as User).Year, (Session["user"] as User).ProjectCode);
                object dic = DT2Obj.ToObj(dt);
                object dic1 = DT2Obj.ToObj(dt1);
                object dic2 = DT2Obj.ToObj(dt2);
                return Json(new { data1 = dic1, data2 = dic2, prjects = dic });
            }
            return null;
        }
        /// <summary>
        /// 查询经销商环节得分情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="shopCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchShopCharter(string projectCode, string shopCode)
        {
            var res = new JsonResult();

            DataTable score_info_dt = service.SearchShopScoreInfo(projectCode, shopCode);
            object score_info = DT2Obj.ToObj(score_info_dt);

            DataTable sixty_l_dt = service.SearchShopCharterScore(projectCode, shopCode, true);
            object sixty_l = DT2Obj.ToObj(sixty_l_dt);
            DataTable sixty_h_dt = service.SearchShopCharterScore(projectCode, shopCode, false);
            object sixty_h = DT2Obj.ToObj(sixty_h_dt);
            object sixty_info = new { sixty_l = sixty_l, sixty_h = sixty_h };

            List<DataTable> dts = service.SearchCharterReport(projectCode, shopCode);
            if (dts == null || dts.Count == 0) return null;
            List<string> categories = new List<string>();
            List<object> dic = new List<object>();
            for (int i = 0; i < dts.Count; i++)
            {
                DataTable dt = dts[i];
                if (dt.Rows.Count == 0) break;
                if (i == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        categories.Add(dr["CharterName"].ToString());
                    }
                }
                else
                {
                    Dictionary<string, object> scores = new Dictionary<string, object>();
                    scores.Add("Code", dt.Rows[0]["Code"].ToString());
                    scores.Add("Score", new List<float>());
                    foreach (DataRow dr in dt.Rows)
                    {
                        float value = 0;
                        float.TryParse(dr["Score"].ToString(), out value);
                        (scores["Score"] as List<float>).Add(value);
                    }
                    dic.Add(scores);
                }
            }
            object charter_report = new { categories = categories, scores = dic };

            DataTable charter_dt = service.SearchCharter(projectCode);
            object charter = DT2Obj.ToObj(charter_dt);

            return Json(new { score_info = score_info, sixty_info = sixty_info, charter_report = charter_report, charter = charter });
        }
        /// <summary>
        /// 查询区域环节得分情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchAreaCharter(string projectCode, string areaCode)
        {
            var res = new JsonResult();

            DataTable sixty_l_dt = service.SearchAreaCharterScore(projectCode, areaCode, true);
            object sixty_l = DT2Obj.ToObj(sixty_l_dt);
            DataTable sixty_h_dt = service.SearchAreaCharterScore(projectCode, areaCode, false);
            object sixty_h = DT2Obj.ToObj(sixty_h_dt);
            object sixty_info = new { sixty_l = sixty_l, sixty_h = sixty_h };

            List<DataTable> dts = service.SearchAreaCharterReport(projectCode, areaCode);
            if (dts == null || dts.Count == 0) return null;
            List<string> categories = new List<string>();
            List<object> dic = new List<object>();
            for (int i = 0; i < dts.Count; i++)
            {
                DataTable dt = dts[i];
                if (dt.Rows.Count == 0) break;
                if (i == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        categories.Add(dr["CharterName"].ToString());
                    }
                }
                else
                {
                    Dictionary<string, object> scores = new Dictionary<string, object>();
                    scores.Add("Code", dt.Rows[0]["Code"].ToString());
                    scores.Add("Score", new List<float>());
                    foreach (DataRow dr in dt.Rows)
                    {
                        float value = 0;
                        float.TryParse(dr["Score"].ToString(), out value);
                        (scores["Score"] as List<float>).Add(value);
                    }
                    dic.Add(scores);
                }
            }
            object charter_report = new { categories = categories, scores = dic };

            DataTable charter_dt = service.SearchCharter(projectCode);
            object charter = DT2Obj.ToObj(charter_dt);

            return Json(new { sixty_info = sixty_info, charter_report = charter_report, charter = charter });
        }
        /// <summary>
        /// 查询体系得分情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="areaCode"></param>
        /// <param name="charterCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchAreaSubjectScore(string projectCode, string areaCode, string charterCode)
        {
            DataTable dt = service.SearchAreaSubjectScore(projectCode, areaCode, charterCode);
            object dic = DT2Obj.ToObj(dt);
            if (dic == null)
                return null;
            return Json(dic);
        }
        /// <summary>
        /// 查询所有的环节
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchCharters()
        {
            if (Session["user"] == null) return null;
            User user = Session["user"] as User;
            DataTable charter_dt = service.SearchCharter(user.ProjectCode);
            object charter = DT2Obj.ToObj(charter_dt);
            if (charter == null)
                return null;
            return Json(charter);
        }
        public ActionResult DownloadFile(string fileName)
        {
            string contentType = "application/x-msdownload";
            string downloadFileName = Path.GetFileName(fileName);
            return this.File(fileName, contentType, downloadFileName);
        }
        /// <summary>
        /// 查询环节下体系的情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="charterCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchSubjectCharter(string projectCode, string charterCode)
        {
            DataTable dt = service.SearchShopSubjectScore(projectCode, charterCode);
            object dic = DT2Obj.ToObj(dt);
            if (dic == null)
                return null;
            return Json(dic);
        }
        /// <summary>
        /// 查询经销商体系得分情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="charterCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchShopSubjectScore(string projectCode, string shopCode, string charterCode)
        {
            DataTable dt = service.SearchShopSubjectScore(projectCode, shopCode, charterCode);
            object dic = DT2Obj.ToObj(dt);
            if (dic == null)
                return null;
            return Json(dic);
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="shops"></param>
        /// <param name="projects"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadCheckedFiles(string shops, string projects)
        {
            log(shops + " " + projects);
            if (string.IsNullOrEmpty(shops) || string.IsNullOrEmpty(projects))
                return null;
            if (shops.LastIndexOf(',') == shops.Length - 1)
                shops = shops.Remove(shops.Length - 1);
            if (projects.LastIndexOf(',') == projects.Length - 1)
                projects = projects.Remove(projects.Length - 1);

            List<string> shopList = shops.Split(',').ToList<string>();
            string temp = "";
            List<string> projectList = projects.Split(',').ToList<string>();
            try
            {
                string reportPath = HttpContext.Server.MapPath("~/ReportFiles");
                //temp = Path.Combine(reportPath, "TEMP\\temp.zip");
                if (shopList.Count > 1)
                {
                    temp = Path.Combine(reportPath, "TEMP\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip");
                }
                else if (shopList.Count > 0)
                {
                    temp = Path.Combine(reportPath, "TEMP\\" + shopList[0]+ DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip");
                }
                else
                {
                    temp = Path.Combine(reportPath, "TEMP\\" + "error"+ DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip");
                }


                ZipInForFiles(shopList, projectList, reportPath, temp, 9);
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }

            //Stream stream = new FileStream(temp, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //byte[] bytes = new byte[(int)stream.Length];
            //stream.Position = 0;
            //stream.Read(bytes, 0, bytes.Length);
            //stream.Close();
            //string contentType = "application/octet-stream";
            //System.IO.File.Delete(temp);
            //string zipName = string.Empty;
            //foreach (string item in projectList)
            //{
            //    zipName += item + "_";
            //}
            return Json(new { ExportPath = temp });
            //return this.File(bytes, contentType, Url.Encode(Path.GetFileName(zipName + "Report.zip")));

        }

        #region File Zip
        /// <summary>
        /// 文件下载时压缩打包的操作
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="folders"></param>
        /// <param name="folderToZip"></param>
        /// <param name="zipedFile"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static bool ZipInForFiles(List<string> fileNames, List<string> folders, string folderToZip, string zipedFile, int level)
        {
            ReportService reportService = new ReportService();
            UploadFileToAliyun aliyun = new UploadFileToAliyun();
            foreach (string projectCode in folders)
            {
                string projectCodeFile = "";
                projectCodeFile = projectCode;
                if (projectCode.Contains("MBH"))
                {
                    projectCodeFile = "20" + projectCode.Substring(3, 2) + "Q" + projectCode.Substring(6, 1);
                }
                else if (projectCode.Contains("MB"))
                {
                    projectCodeFile = "20" + projectCode.Substring(2, 2) + "Q" + projectCode.Substring(5, 1);
                }
                else if (projectCode.ToUpper().Contains("VAN"))
                {
                    projectCodeFile = "20" + projectCode.Substring(3, 2) + "Q" + projectCode.Substring(6, 1);
                }
                foreach (string shopCode in fileNames)
                {
                    if (!Directory.Exists(folderToZip + @"\" + projectCode))
                    {
                        Directory.CreateDirectory(folderToZip + @"\" + projectCode);
                    }
                    string shopName = "";
                    DataTable dt = reportService.SearchShopByShopCode(projectCode, shopCode, "");
                    if (dt.Rows.Count > 0)
                    {
                        shopName = dt.Rows[0]["ShopName"].ToString();
                    }
                    // 单店报告
                    string file0_downLoad = Path.Combine(folderToZip + @"\" + projectCode, shopCode + ".xlsx");// 最开始的命名规则
                    string file1_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_单店报告" + ".xlsx");
                    string file2_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_星徽产品大使_单店报告" + ".xlsx");
                    string file3_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + "-1 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx");
                    string file4_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + "-2 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx");
                    string file5_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + shopCode + "_" + shopName + "_单店报告" + ".xlsx");

                    // 综合报告
                    string file6_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + "-1 V级及威霆销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx");
                    string file7_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + "-2 V级及威霆销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx");
                    string file8_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_星徽产品大使_综合报告" + ".xlsx");
                    string file9_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx");
                    string file10_downLoad = Path.Combine(folderToZip + @"\" + projectCode, projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx");

                    if (System.IO.File.Exists(file0_downLoad))
                    {
                        System.IO.File.Delete(file0_downLoad);
                    }
                    if (System.IO.File.Exists(file1_downLoad))
                    {
                        System.IO.File.Delete(file1_downLoad);
                    }
                    if (System.IO.File.Exists(file2_downLoad))
                    {
                        System.IO.File.Delete(file2_downLoad);
                    }
                    if (System.IO.File.Exists(file3_downLoad))
                    {
                        System.IO.File.Delete(file3_downLoad);
                    }
                    if (System.IO.File.Exists(file4_downLoad))
                    {
                        System.IO.File.Delete(file4_downLoad);
                    }
                    if (System.IO.File.Exists(file5_downLoad))
                    {
                        System.IO.File.Delete(file5_downLoad);
                    }
                    if (System.IO.File.Exists(file6_downLoad))
                    {
                        System.IO.File.Delete(file6_downLoad);
                    }
                    if (System.IO.File.Exists(file7_downLoad))
                    {
                        System.IO.File.Delete(file7_downLoad);
                    }
                    if (System.IO.File.Exists(file8_downLoad))
                    {
                        System.IO.File.Delete(file8_downLoad);
                    }
                    if (System.IO.File.Exists(file9_downLoad))
                    {
                        System.IO.File.Delete(file9_downLoad);
                    }
                    if (System.IO.File.Exists(file10_downLoad))
                    {
                        System.IO.File.Delete(file10_downLoad);
                    }
                    try
                    {
                        //下载MB或者MBH最开始命名规则的文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + shopCode + ".xlsx",
                                  file0_downLoad);
                    }
                    catch (Exception ex0)
                    {
                        if (!ex0.Message.ToString().Contains("exist"))
                        {
                            log(ex0.Message.ToString());
                        }
                    }
                    try
                    {
                        //下载MB或者MBH第一个文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_单店报告" + ".xlsx",
                                  file1_downLoad);
                    }
                    catch (Exception ex1)
                    {
                        if (!ex1.Message.ToString().Contains("exist"))
                        {
                            log(ex1.Message.ToString());
                        }
                    }
                    try
                    {
                        //下载MB或者MBH第二个文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_星徽产品大使_单店报告" + ".xlsx",
                                  file2_downLoad);
                    }
                    catch (Exception ex2)
                    {
                        if (!ex2.Message.ToString().Contains("exist"))
                        { log(ex2.Message.ToString()); }
                    }
                    try
                    {
                        //下载Van第一个文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + "-1 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx",
                                  file3_downLoad);
                    }
                    catch (Exception ex3)
                    {
                        if (!ex3.Message.ToString().Contains("exist"))
                        { log(ex3.Message.ToString()); }
                    }
                    try
                    {
                        //下载Van第二个文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + "-2 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx",
                                  file4_downLoad);
                    }
                    catch (Exception ex4)
                    {
                        if (!ex4.Message.ToString().Contains("exist"))
                        { log(ex4.Message.ToString()); }
                    }
                    try
                    {
                        //下载迈巴赫文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + shopCode + "_" + shopName + "_单店报告" + ".xlsx",
                                  file5_downLoad);
                    }
                    catch (Exception ex5)
                    {
                        if (!ex5.Message.ToString().Contains("exist"))
                        { log(ex5.Message.ToString()); }
                    }
                    try
                    {

                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + "-1 V级及威霆销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx",
                                  file6_downLoad);
                    }
                    catch (Exception ex6)
                    {
                        if (!ex6.Message.ToString().Contains("exist"))
                        { log(ex6.Message.ToString()); }
                    }
                    try
                    {

                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + "-2 V级及威霆销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx",
                                  file7_downLoad);
                    }
                    catch (Exception ex7)
                    {
                        if (!ex7.Message.ToString().Contains("exist"))
                        { log(ex7.Message.ToString()); }
                    }

                    try
                    {

                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_星徽产品大使_综合报告" + ".xlsx",
                                  file8_downLoad);
                    }
                    catch (Exception ex8)
                    {
                        if (!ex8.Message.ToString().Contains("exist"))
                        { log(ex8.Message.ToString()); }
                    }
                    try
                    {
                        //下载MB或者MBH第一个文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx",
                                  file9_downLoad);
                    }
                    catch (Exception ex9)
                    {
                        if (!ex9.Message.ToString().Contains("exist"))
                        { log(ex9.Message.ToString()); }
                    }
                    try
                    {
                        //下载迈巴赫文件
                        aliyun.GetObject("yrtech", "BENZReport" + @"/" + projectCode + @"/" + projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + shopCode + "_" + shopName + "_综合报告" + ".xlsx",
                                  file10_downLoad);
                    }
                    catch (Exception ex10)
                    {
                        if (!ex10.Message.ToString().Contains("exist"))
                        { log(ex10.Message.ToString()); }
                    }
                }
            }
            bool isSuccess = true;
            if (!Directory.Exists(folderToZip))
            {
                return false;
            }

            try
            {
                // 调用压缩的共同方法
                using (ZipOutputStream zipOutStream = new ZipOutputStream(System.IO.File.Create(zipedFile)))
                {
                    zipOutStream.SetLevel(level);
                    string comment = string.Empty;
                    foreach (string folder in folders)
                    {
                        string projectCodeFile = "";
                        projectCodeFile = folder;
                        if (folder.Contains("MBH"))
                        {
                            projectCodeFile = "20" + folder.Substring(3, 2) + "Q" + folder.Substring(6, 1);
                        }
                        else if (folder.Contains("MB"))
                        {
                            projectCodeFile = "20" + folder.Substring(2, 2) + "Q" + folder.Substring(5, 1);
                        }
                        else if (folder.ToUpper().Contains("VAN"))
                        {
                            projectCodeFile = "20" + folder.Substring(3, 2) + "Q" + folder.Substring(6, 1);
                        }

                        //创建当前文件夹
                        ZipEntry entry = new ZipEntry(folder + "/"); //加上 “/” 才会当成是文件夹创建
                        zipOutStream.PutNextEntry(entry);
                        zipOutStream.Flush();

                        Crc32 crc = new Crc32();

                        foreach (string fileName in fileNames)
                        {
                            string shopName = "";
                            DataTable dt = reportService.SearchShopByShopCode(folder, fileName, "");
                            if (dt.Rows.Count > 0)
                            {
                                shopName = dt.Rows[0]["ShopName"].ToString();
                            }
                            string file0_downLoad = Path.Combine(folderToZip + @"\" + folder, fileName + ".xlsx");// 最开始的命名规则
                            string file1_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_单店报告" + ".xlsx");
                            string file2_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_星徽产品大使_单店报告" + ".xlsx");
                            string file3_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + "-1 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx");
                            string file4_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + "-2 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx");
                            string file5_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + fileName + "_" + shopName + "_单店报告" + ".xlsx");

                            // 综合报告
                            string file6_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + "-1 V级及威霆销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                            string file7_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + "-2 V级及威霆销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                            string file8_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_星徽产品大使_综合报告" + ".xlsx");
                            string file9_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                            string file10_downLoad = Path.Combine(folderToZip + @"\" + folder, projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                            string extension = "xlsx";
                            //if (System.IO.File.Exists(file0 + ".xls"))
                            //    extension = ".xls";
                            //else if (System.IO.File.Exists(file0 + ".xlsx"))
                            //    extension = ".xlsx";
                            //else
                            //{
                            //    comment += "期号：" + folder + "，报表：" + fileName + "不存在。\r\n";
                            //    continue;
                            //}
                            if (System.IO.File.Exists(file0_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file0_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    // entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + fileName + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file1_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file1_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_单店报告" + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file2_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file2_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_星徽产品大使_单店报告" + ".xlsx");

                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file3_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file3_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + "-1 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx");

                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file4_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file4_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + "-2 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_V级及威霆_单店报告" + ".xlsx");

                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file5_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file5_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + fileName + "_" + shopName + "_单店报告" + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file6_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file6_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + "-1 V级及威霆销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file7_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file7_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + "-2 V级及威霆销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file8_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file8_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_星徽产品大使_综合报告" + ".xlsx");

                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file9_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file9_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + " 梅赛德斯-奔驰销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            if (System.IO.File.Exists(file10_downLoad))
                            {
                                using (FileStream fs = System.IO.File.OpenRead(Path.Combine(folderToZip, folder, file10_downLoad)))
                                {
                                    byte[] buffer = new byte[fs.Length];
                                    fs.Read(buffer, 0, buffer.Length);
                                    //entry = new ZipEntry(folder + "/" + fileName + extension);
                                    entry = new ZipEntry(folder + "/" + projectCodeFile + " 梅赛德斯-迈巴赫销售质量现场考核_" + fileName + "_" + shopName + "_综合报告" + ".xlsx");
                                    entry.DateTime = DateTime.Now;
                                    entry.Size = fs.Length;
                                    fs.Close();
                                    crc.Reset();
                                    crc.Update(buffer);
                                    entry.Crc = crc.Value;
                                    zipOutStream.PutNextEntry(entry);
                                    zipOutStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                        }

                    }
                    zipOutStream.SetComment(comment);
                    zipOutStream.Finish();
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        #endregion
    }
}
