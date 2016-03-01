using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace WYJK.Entity
{
    //公积金信息表
    public class AccumulationFund
    {

        /// <summary>
        /// 公积金ID
        /// </summary>		
        public int AccumulationFundID { get; set; }
        /// <summary>
        /// 参保人ID
        /// </summary>		
        public int SocialSecurityManID { get; set; }
        /// <summary>
        /// 参公积金地
        /// </summary>		
        public string AccumulationFundArea { get; set; }
        /// <summary>
        /// 公积金基数
        /// </summary>		
        public string AccumulationFundBase { get; set; }
        /// <summary>
        /// 缴费比例
        /// </summary>		
        public string PayProportion { get; set; }
        /// <summary>
        /// 起缴时间
        /// </summary>		
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 缴费月数
        /// </summary>		
        public int PayMonthCount { get; set; }
        /// <summary>
        /// 补交月数
        /// </summary>		
        public int PayBeforeMonthCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Note { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        public string Status { get; set; }

    }
}

