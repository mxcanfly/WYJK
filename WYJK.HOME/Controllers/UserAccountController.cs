using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class UserAccountController : BaseFilterController
    {
        UserAccountService accountSv = new UserAccountService();
        UserMemberService memberSv = new UserMemberService();

        private readonly IMemberService _memberService = new MemberService();

        // GET: UserAccount
        public async System.Threading.Tasks.Task<ActionResult> MyAccount(UserAccountPageModel pageModel)
        {
            Members members = memberSv.UserInfos(CommonHelper.CurrentUser.MemberID);
            ViewBag.Account = members.Account;

            PagedResult<AccountRecord> list = await _memberService.GetAccountRecordList(CommonHelper.CurrentUser.MemberID, "", pageModel.BeginTime, pageModel.EndTime, pageModel);
            list.Items.ToList().ForEach(n =>
            {
                if (n.ShouZhiType.Trim() == "收入")
                    n.CostDisplay = "+" + Convert.ToString(n.Cost);
                else if (n.ShouZhiType.Trim() == "支出")
                    n.CostDisplay = "-" + Convert.ToString(n.Cost);
            });

            return View(list);
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