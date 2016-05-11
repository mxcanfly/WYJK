using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Entity
{
    /// <summary>
    /// 用户借款
    /// </summary>
    public class MemberLoan : Members
    {
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>		
        public int MemberID { get; set; }
        /// <summary>
        /// 总额度
        /// </summary>		
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已用额度
        /// </summary>		
        public decimal AlreadyUsedAmount { get; set; }
        /// <summary>
        /// 可用额度
        /// </summary>		
        public decimal AvailableAmount { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public string Status { get; set; }
    }

    /// <summary>
    /// 用户借款参数
    /// </summary>
    public class MemberLoanParameter : PagedParameter
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string MemberName{get;set;}
    }
}
