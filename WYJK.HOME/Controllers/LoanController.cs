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


        /// <summary>
        /// 借款
        /// </summary>
        /// <returns></returns>
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

            //判断是否计算过身价（MemberLoan : 个人借款表中是否有本人数据）
            if (!sss.LoanCalculator(m.MemberID))//没有进行过身价计算
            {
                return Redirect("/LoanCalculator/CalculatorFirst");
            }

            //跳转到借款页面
            return View();
            
        }
    }
}