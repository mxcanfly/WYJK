using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.Captcha;
using WYJK.Framework.EnumHelper;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class UserLoanController : BaseController
    {
        private readonly IMemberService _memberService = new MemberService();

        UserLoanService loanSv = new UserLoanService();


        public ActionResult Index(LoanQueryParamsModel parameter)
        {

            string where = "";

            int  status = 0;
            if (Int32.TryParse(parameter.Status, out status))
            {
                where += $@" and Status={status}";
            }

            List<MemberLoanAudit> list = loanSv.GetUserLoans(CommonHelper.CurrentUser.MemberID,where);

            //string sql = $@"
            //    select 
            //     MemberLoanAudit.ID,Members.MemberID,Members.MemberName,members.MemberPhone,
            //     MemberLoan.TotalAmount,memberloan.AlreadyUsedAmount,memberloan.AvailableAmount,
            //     MemberLoanAudit.ApplyAmount,MemberLoanAudit.Status,MemberLoanAudit.ApplyDate,MemberLoanAudit.AuditDate
            //    from MemberLoanAudit
            //     left join MemberLoan on MemberLoanAudit.MemberID = MemberLoan.MemberID
            //     left join Members on  MemberLoanAudit.MemberID = Members.MemberID 
            //    where 1=1 {where} and Members.MemberID={MemberId()}";

            //List<MemberLoanAuditList> SocialSecurityPeopleList = DbHelper.Query<MemberLoanAuditList>(sql);

            var c = list.Skip(parameter.SkipCount - 1).Take(parameter.TakeCount);
             

            PagedResult<MemberLoanAudit> page = new PagedResult<MemberLoanAudit>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = list.Count,
                Items = c
            };

            List<SelectListItem> selectList = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "全部" } };
            selectList.AddRange(EnumExt.GetSelectList(typeof(LoanAuditEnum)));
            ViewData["StatusType"] = selectList;

            return View(page);
        }

        public ActionResult Detail(int? id)
        {
            if (id != null)
            {
                return View(loanSv.GetLoadAuditDetail(id.Value));
            }

            return Redirect("/Index/Index");
        }

    }
}