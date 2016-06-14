using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Entity;
using WYJK.HOME.Models;

namespace WYJK.HOME.Controllers
{
    public class LoanController : Controller
    {
        
        public ActionResult Loan()
        {
            //判断用户是否登录
            Members m = (Members)this.Session["UserInfo"];
            if (m == null)
            {
                return Redirect("/User/Login");
            }

            //判断是否缴费满三个月



            return Redirect("/User/Info");
        }
    }
}