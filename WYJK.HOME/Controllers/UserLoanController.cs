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

namespace WYJK.HOME.Controllers
{
    public class UserLoanController : BaseController
    {
        private readonly IMemberService _memberService = new MemberService();


        public ActionResult Index(LoanQueryParamsModel parameter)
        {

            String sql = $@"
                select 
	                MemberLoanAudit.ID,Members.MemberID,Members.MemberName,members.MemberPhone,
	                MemberLoan.TotalAmount,memberloan.AlreadyUsedAmount,memberloan.AvailableAmount,
	                MemberLoanAudit.ApplyAmount,MemberLoanAudit.Status,MemberLoanAudit.ApplyDate,MemberLoanAudit.AuditDate
                from MemberLoanAudit
	                left join MemberLoan on MemberLoanAudit.MemberID = MemberLoan.MemberID
	                left join Members on  MemberLoanAudit.MemberID = Members.MemberID 
                where 1=1 and Members.MemberID={MemberId()}";

            List<InsuranceListViewModel> SocialSecurityPeopleList = DbHelper.Query<InsuranceListViewModel>(sql);

            var c = SocialSecurityPeopleList.Skip(parameter.SkipCount - 1).Take(parameter.TakeCount);


            PagedResult<InsuranceListViewModel> page = new PagedResult<InsuranceListViewModel>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,

                TotalItemCount = SocialSecurityPeopleList.Count,
                Items = c
            };
            
            return View();
        }





    }
}