using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace WYJK.Entity
{
    //参保人
    public class SocialSecurityPeople
    {

        /// <summary>
        /// 参保人ID
        /// </summary>		
        public int SocialSecurityPeopleID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>		
        public string SocialSecurityPeopleName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>		
        public string IdentityCard { get; set; }
        /// <summary>
        /// 身份证照片
        /// </summary>		
        public string IdentityCardPhoto { get; set; }
        /// <summary>
        /// 户口性质
        /// </summary>		
        public string HouseholdProperty { get; set; }
        /// <summary>
        /// 是否缴纳社保
        /// </summary>		
        public bool IsPaySocialSecurity { get; set; }
        /// <summary>
        /// IsPayAccumulationFund
        /// </summary>		
        public bool IsPayAccumulationFund { get; set; }

        /// <summary>
        /// 社保信息
        /// </summary>
        public SocialSecurity socialSecurity { get; set; }

        /// <summary>
        /// 公积金信息
        /// </summary>
        public AccumulationFund accumulationFund { get; set; }

    }

    /// <summary>
    /// 参保人列表信息显示内容
    /// </summary>
    public class InsuredPeople
    {
        #region 参保人
        /// <summary>
        /// 参保人ID
        /// </summary>		
        public int SocialSecurityPeopleID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>		
        public string SocialSecurityPeopleName { get; set; }
        #endregion

        #region 社保信息
        /// <summary>
        /// 起缴时间
        /// </summary>		
        public DateTime SSPayTime { get; set; }
        /// <summary>
        /// 缴费月数
        /// </summary>		
        public int SSPayMonthCount { get; set; }
        #endregion

        #region 公积金信息
        /// <summary>
        /// 起缴时间
        /// </summary>		
        public DateTime AFPayTime { get; set; }
        /// <summary>
        /// 缴费月数
        /// </summary>		
        public int AFPayMonthCount { get; set; }
        #endregion

    }


}

