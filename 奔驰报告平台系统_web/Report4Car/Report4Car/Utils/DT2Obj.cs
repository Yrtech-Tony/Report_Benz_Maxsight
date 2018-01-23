using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Report4Car.Utils
{
    /// <summary>
    /// DataTable 转化为对象的方法
    /// </summary>
    public static class DT2Obj
    {
        public static List<Dictionary<string, object>> ToObj(DataTable dt) {
            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                dic.Add(result);
            }
            return dic;
        }
    }
}