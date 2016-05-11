using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IService;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;

namespace WYJK.Web.Controllers.Mvc.Loan
{
    [Authorize]
    public class LoanMemberController : Controller
    {
        private ILoanSubjectService _loanSubjectService = new LoanSubjectService();

        /// <summary>
        /// 获取借款用户列表1
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLoanMemberList(MemberLoanParameter parameter)
        {
            PagedResult<MemberLoan> memberLoanList = _loanSubjectService.GetMemberLoanList(parameter);

            return View("/Views/Loan/LoanMember/GetLoanMemberList.cshtml", memberLoanList);
        }
    }
}