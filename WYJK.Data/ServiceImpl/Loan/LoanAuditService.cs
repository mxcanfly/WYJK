using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Data.IService;
using WYJK.Entity;

namespace WYJK.Data.ServiceImpl
{
    public class LoanAuditService : ILoanAuditService
    {

        /// <summary>
        /// 获取借款审核列表
        /// </summary>
        /// <returns></returns>
        public PagedResult<MemberLoanAuditList> GetLoanAuditList(MemberLoanAuditListParameter parameter)
        {
            StringBuilder builder = new StringBuilder(" where 1 = 1");
            if (!string.IsNullOrEmpty(parameter.Status))
            {
                builder.Append($" and Status = {parameter.Status}");
            }

            builder.Append($" and Members.MemberName like '%{parameter.MemberName}%'");

            string innersqlstr = $@"select Members.MemberID,Members.MemberName,members.MemberPhone,
MemberLoan.TotalAmount,memberloan.AlreadyUsedAmount,memberloan.AvailableAmount,
MemberLoanAudit.ApplyAmount,MemberLoanAudit.Status,MemberLoanAudit.ApplyDate,MemberLoanAudit.AuditDate
from MemberLoanAudit
left join MemberLoan on MemberLoanAudit.MemberID = MemberLoan.MemberID
left join Members on  MemberLoanAudit.MemberID = Members.MemberID"
+ builder.ToString();

            string sqlstr = "select * from (select ROW_NUMBER() OVER(ORDER BY t.ApplyDate )AS Row,t.* from"
                             + $" ({innersqlstr}) t) tt"
                             + " where tt.Row BETWEEN @StartIndex AND @EndIndex";

            List<MemberLoanAuditList> list = DbHelper.Query<MemberLoanAuditList>(sqlstr, new
            {
                StartIndex = parameter.SkipCount,
                EndIndex = parameter.TakeCount
            });
            int totalCount = DbHelper.QuerySingle<int>($"select count(0) from ({innersqlstr}) t");

            return new PagedResult<MemberLoanAuditList>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = totalCount,
                Items = list
            };
        }
    }
}
