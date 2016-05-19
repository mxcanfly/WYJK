using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Entity
{
    /// <summary>
    /// 基数审核
    /// </summary>
    public class BaseAudit
    {
        /// <summary>
		/// ID
        /// </summary>		
        public int ID { get; set; }
        /// <summary>
        /// 参保人ID
        /// </summary>		
        public int SocialSecurityPeopleID { get; set; }
        /// <summary>
        /// 调整后基数
        /// </summary>		
        public decimal BaseAdjusted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>		
        public string Status { get; set; }
        /// <summary>
        /// 类型（社保：0、公积金：1）
        /// </summary>		
        public string Type { get; set; }
    }

    /// <summary>
    /// 调整基数
    /// </summary>

    public class AdjustingBase
    {
        /// <summary>
        /// 参保人ID
        /// </summary>
        public int SocialSecurityPeopleID { get; set; }

        /// <summary>
        /// 是否缴纳社保
        /// </summary>
        public bool IsPaySocialSecurity { get; set; }
        /// <summary>
        /// 是否缴纳公积金
        /// </summary>
        public bool IsPayAccumulationFund { get; set; }
        /// <summary>
        /// 社保基数
        /// </summary>
        public decimal SocialSecurityBase { get; set; }
        /// <summary>
        /// 社保最小基数
        /// </summary>
        public decimal SocialSecurityMinBase { get; set; }
        /// <summary>
        /// 社保最大基数
        /// </summary>
        public decimal SocialSecurityMaxBase { get; set; }
        /// <summary>
        /// 公积金基数
        /// </summary>
        public decimal AccumulationFundBase { get; set; }

        /// <summary>
        /// 公积金最小基数
        /// </summary>
        public decimal AccumulationFundMinBase { get; set; }
        /// <summary>
        /// 公积金最大基数
        /// </summary>
        public decimal AccumulationFundMaxBase { get; set; }

    }

    /// <summary>
    /// 调整基数参数类
    /// </summary>
    public class AdjustingBaseParameter
    {
        /// <summary>
        /// 参保人ID
        /// </summary>
        public int SocialSecurityPeopleID { get; set; }
        /// <summary>
        /// 是否缴纳社保
        /// </summary>
        public bool IsPaySocialSecurity { get; set; }
        /// <summary>
        /// 是否缴纳公积金
        /// </summary>
        public bool IsPayAccumulationFund { get; set; }
        /// <summary>
        /// 调整后的社保基数
        /// </summary>
        public decimal SocialSecurityBaseAdjusted { get; set; }
        /// <summary>
        /// 调整后的公积金基数
        /// </summary>
        public decimal AccumulationFundBaseAdjusted { get; set; }
    }
}
