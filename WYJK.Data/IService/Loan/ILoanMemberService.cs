using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;

namespace WYJK.Data.IService
{
    public interface ILoanMemberService
    {
        /// <summary>
        /// 获取用户借款列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        PagedResult<MemberLoanList> GetMemberLoanList(MemberLoanParameter parameter);

        /// <summary>
        /// 获取用户借款详情
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        AppayLoan GetMemberLoanDetail(int MemberID);

        /// <summary>
        /// 提交借款申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SubmitLoanApply(MemberLoanAuditParameter model);

        /// <summary>
        /// 根据用户ID获取正在审核额度之和
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        decimal GetTotalAuditAmountByMemberID(int MemberID);
    }
}
