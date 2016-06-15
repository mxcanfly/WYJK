using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Entity;
using System.Web.SessionState;

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

    }
}