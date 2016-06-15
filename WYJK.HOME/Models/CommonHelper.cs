using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Entity;
using System.Web.SessionState;
using System.Web.Mvc;

namespace WYJK.HOME.Models
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取当前用户
        /// </summary>
        public static Members CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session["UserInfo"] != null)
                {
                    return HttpContext.Current.Session["UserInfo"] as Members;
                }
                return null;


            }
        }

        public static List<SelectListItem> EntityListToSelctList(List<Region> list, string promt = "请选择")
        {
            List<SelectListItem> selList = new List<SelectListItem>();

            selList.Insert(0, new SelectListItem { Text = promt, Value = "0" });

            foreach (Region item in list)
            {
                selList.Add(new SelectListItem { Text = item.RegionName, Value = item.RegionCode });
            }

            return selList;

        }

    }
}