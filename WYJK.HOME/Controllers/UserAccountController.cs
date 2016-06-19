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
    public class UserAccountController : BaseFilterController
    {
        UserAccountService accountSv = new UserAccountService();

        // GET: UserAccount
        public ActionResult MyAccount()
        {
            return View();
        }

        public ActionResult Charge()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Charge(RechargeParameters parameter)
        {
            parameter.MemberID = CommonHelper.CurrentUser.MemberID;
            parameter.PayMethod = "银联";

            if (accountSv.SubmitRechargeAmount(parameter))
            {
                return RedirectToAction("MyAccount");
            }


            return View();
        }

        public ActionResult WithDraw()
        {


            return View();
        }
    }
}