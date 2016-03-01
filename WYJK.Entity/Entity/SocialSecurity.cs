using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace WYJK.Entity
{
    //参保人的社保信息
    public class SocialSecurity
    {

        /// <summary>
        /// 社保ID
        /// </summary>		
        public int SocialSecurityID { get; set; }
        /// <summary>
        /// 参保人ID
        /// </summary>		
        public int SocialSecurityManID { get; set; }
        /// <summary>
        /// 参保地
        /// </summary>		
        public string InsuranceArea { get; set; }
        /// <summary>
        /// 社保基数
        /// </summary>		
        public string SocialSecurityBase { get; set; }
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
        /// 银行缴费月数

        /// </summary>		
        public int BankPayMonth { get; set; }
        /// <summary>
        /// 企业缴费月数
        /// </summary>		
        public int EnterprisePayMonth { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Note { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        public string Status { get; set; }

    }

    /// <summary>
    /// 社保业务显示列表
    /// </summary>
    public class SocialSecurityShowModel
    {
        /// <summary>
        /// 参保人ID
        /// </summary>		
        public int SocialSecurityManID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>		
        public string SocialSecurityManName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>		
        public string IdentityCard { get; set; }
        /// <summary>
        /// 户口性质
        /// </summary>		
        public string HouseholdProperty { get; set; }

        /// <summary>
        /// 社保ID
        /// </summary>		
        public int SocialSecurityID { get; set; }
        /// <summary>
        /// 参保地
        /// </summary>		
        public string InsuranceArea { get; set; }
        /// <summary>
        /// 社保基数
        /// </summary>		
        public string SocialSecurityBase { get; set; }
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
        /// 银行缴费月数
        /// </summary>		
        public int BankPayMonth { get; set; }
        /// <summary>
        /// 企业缴费月数
        /// </summary>		
        public int EnterprisePayMonth { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        public string Status { get; set; }
    }
}

