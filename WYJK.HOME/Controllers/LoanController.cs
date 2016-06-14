using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Entity;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class LoanController : Controller
    {
        SocialSecurityService sss = new SocialSecurityService();


        
        public ActionResult Loan()
        {
            //判断用户是否登录
            Members m = (Members)this.Session["UserInfo"];
            if (m == null)
            {
                return Redirect("/User/Login");
            }

            //判断是否已缴费
            if (!sss.ExistSocialPeople(m.MemberID))
            {
                //添加社保人
                return Redirect("/UserInsurance/Add1");
            }
            //缴费不满三个月
            if(!sss.PayedMonthCount(m.MemberID))
            {
                return Redirect("/UserInsurance/Index");
            }

            //判断是否计算过身价

            //跳转到借款页面
            return View();
            
        }
    }
}