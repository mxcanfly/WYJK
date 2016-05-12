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

            return View("~/Views/Loan/LoanAudit/GetLoanAuditList.cshtml", list);
        }

        /// <summary>
        /// 批量审核
        /// 1、修改审核状态和审核时间
        /// 2、修改用户借款表的已用额度和可用额度
        /// 3、修改用户账户额度(暂去)
        /// 4、记录流水(暂去)
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult BatchAudit(int[] IDs, string Status)
        {
            //判断是否已经审核，只有没有进行审核，才能进行下面操作


            //1、修改审核状态和审核时间
            string IDsStr = string.Join(",", IDs);
            bool flag = _loanAuditService.MemberLoanAudit(IDsStr, Status);
            if (flag == false) throw new Exception("审核失败");

            //只有审核通过才能继续下面操作
            //2、修改用户借款表的已用额度和可用额度


            return Json(new { status = true, message = "审核成功" });
        }
    }
}