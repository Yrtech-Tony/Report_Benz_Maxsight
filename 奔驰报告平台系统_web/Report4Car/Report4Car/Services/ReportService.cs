using Report4Car.BENZReportServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Report4Car.Services
{
    /// <summary>
    /// 调用后台webService 的方法
    /// </summary>
    public class ReportService
    {
        private BENZReportServer.ServiceSoapClient client = new BENZReportServer.ServiceSoapClient();

        /// <summary>
        /// h获取权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public string GetUserRole(string userID,string projectCode) {
           return client.SearchUserRole(userID, projectCode);
        }
        /// <summary>
        /// 获取所有的期号
        /// </summary>
        /// <param name="year"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public DataTable GetProjects(string year,string projectCode) {
            DataSet ds = client.SearchProjectReport(year,projectCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 查询期号的基本信息
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public ProjectDto SearchProjectByProjectCode(string projectCode)
        {
            return client.SearchProjectByProjectCode(projectCode);
        }
        public DataTable GetBigAreas(string userID, string projectCode)
        {
            DataSet ds = client.SearchBigArea(userID, projectCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 获取所有小区的信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectCode"></param>
        /// <param name="bigAreaCode"></param>
        /// <returns></returns>
        public DataTable GetSmallAreas(string userID, string projectCode, string bigAreaCode)
        {
            DataSet ds = client.SearchSmallArea(bigAreaCode, userID, projectCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 获取集团的信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataTable GetGroups(string userID)
        {
            DataSet ds = client.SearchGroup(userID);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据区域查询经销商的列表
        /// </summary>
        /// <param name="smallAreaCode"></param>
        /// <param name="userId"></param>
        /// <param name="groupCode"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public DataTable getShops(string smallAreaCode, string userId, string groupCode, string projectCode)
        {
            DataSet ds = client.SearchShopReport(smallAreaCode, userId, groupCode, projectCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据区域查询经销商下载列表
        /// </summary>
        /// <param name="bigAreaCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="groupCode"></param>
        /// <param name="carType"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public DataTable SearchReportDownShop_Area(string bigAreaCode, string smallAreaCode, string shopCode, string province, string city, string groupCode, string carType, string projectCode,string userId)
        {
            DataSet ds = client.SearchReportDownShop_Area(bigAreaCode, smallAreaCode, shopCode, province, city, groupCode, carType, projectCode,userId);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据集团来查询经销商的信息
        /// </summary>
        /// <param name="bigAreaCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="groupCode"></param>
        /// <param name="carType"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public DataTable SearchReportDownShop_Group(string bigAreaCode, string smallAreaCode, string shopCode, string province, string city, string groupCode, string carType, string projectCode)
        {
            DataSet ds = client.SearchReportDownShop_Group(bigAreaCode, smallAreaCode, shopCode, province, city, groupCode, carType, projectCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据区域查询经销商录音列表
        /// </summary>
        /// <param name="bigAreaCode"></param>
        /// <param name="smallAreaCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="groupCode"></param>
        /// <param name="carType"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public DataTable SearchShopRecord(string bigAreaCode, string smallAreaCode, string shopCode, string province, string city, string groupCode, string carType, string projectCode,string userId)
        {
            DataSet ds = client.SearchShopRecord(bigAreaCode, smallAreaCode, shopCode, province, city, groupCode, carType, projectCode,userId);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 根据经销商查询经销商的得分情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="shopCode"></param>
        /// <returns></returns>
        public DataTable SearchShopScoreInfo(string projectCode, string shopCode) {
            DataSet ds = client.SearchShopScoreInfo(projectCode, shopCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        /// <summary>
        /// 查询经销商环节得分情况
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="shopCode"></param>
        /// <param name="sixchk"></param>
        /// <returns></returns>
        public DataTable SearchShopCharterScore(string projectCode, string shopCode, bool sixchk)
        {
            DataSet ds = client.SearchShopCharterScore(projectCode, shopCode, sixchk);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public List<DataTable> SearchCharterReport(string projectCode, string shopCode)
        {
            DataSet[] dss = client.CharterReport(projectCode, shopCode);
            List<DataTable> dts = new List<DataTable>();
            foreach (DataSet ds in dss)
            {
                if (ds.Tables.Count > 0)
                    dts.Add(ds.Tables[0]);
            }
            return dts;
        }

        public DataTable SearchCharter(string projectCode)
        {
            DataSet ds = client.SearchCharter(projectCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public DataTable SearchShopSubjectScore(string projectCode, string shopCode, string charterCode)
        {
            DataSet ds = client.SearchShopSubjectScore(projectCode, shopCode, charterCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public DataTable SearchShopSubjectScore(string projectCode, string charterCode)
        {
            DataSet ds = client.SearchSubject_Report(projectCode, charterCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public DataTable SearchShopByShopCode(string projectCode, string shopCode, string shopName)
        {
            DataSet ds = client.SearchShopByShopCode_Report(projectCode, shopCode, shopName);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public DataTable SearchAreaCharterScore(string projectCode, string areaCode, bool sixchk)
        {
            DataSet ds = client.SearchAreaCharterScore(projectCode, areaCode, sixchk);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public List<DataTable> SearchAreaCharterReport(string projectCode, string areaCode)
        {
            DataSet[] dss = client.AreaCharterReport(projectCode, areaCode);
            List<DataTable> dts = new List<DataTable>();
            foreach (DataSet ds in dss)
            {
                if (ds.Tables.Count > 0)
                    dts.Add(ds.Tables[0]);
            }
            return dts;
        }

        public DataTable SearchAreaSubjectScore(string projectCode, string areaCode, string charterCode)
        {
            DataSet ds = client.SearchAreaSubjectScore(projectCode, areaCode, charterCode);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
    }
}