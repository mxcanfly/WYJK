using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class UserApplyWithDrawController : BaseFilterController
    {
        UserAccountService accountSv = new UserAccountService();

        public ActionResult Index()
        {
            DrawCash drawCash = accountSv.GetLastestDrawCash(CommonHelper.CurrentUser.MemberID);





            return View();
        }

        [HttpPost]
        public ActionResult WithDraw(DrawCash drawCash)
        {
            drawCash.MemberId = CommonHelper.CurrentUser.MemberID;

            drawCash.ApplyStatus = 0;//审核中

            if (drawCash.Money > drawCash.LeftAccount)
            {
                assignMessage("提现金额不能大于可提现余额", false);
                return RedirectToAction("WithDraw");
            }

            drawCash.LeftAccount = CommonHelper.CurrentUser.Account - drawCash.FrozenMoney - drawCash.Money;
            int num = accountSv.DrawCash(drawCash);

            if (num > 0)//提现申请成功
            {
                assignMessage("提现申请成功,请等待审核", true);
            }
            else
            {
                assignMessage("提现失败", false);
            }

            return RedirectToAction("WithDraw");
        }
    }
}