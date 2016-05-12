using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IService;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Mvc.Loan
{
    /// <summary>
    /// 借款审核
    /// </summary>
    public class LoanAuditController : Controller
    {
        private ILoanAuditService _loanAuditService = new LoanAuditService();

        /// <summary>
        /// 获取借款审核列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLoanAuditList(MemberLoanAuditListParameter parameter)
        {
            PagedResult<MemberLoanAuditList> list = _loanAuditService.GetLoanAuditList(parameter);

            List<SelectListItem> selectList = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "全部" } };
            selectList.AddRange(EnumExt.GetSelectList(typeof(LoanAuditEnum)));
            ViewData["StatusType"] = selectList;

            return View("/Views/Loan/LoanAudit/GetLoanAuditList.cshtml", list);
        }
    }
}