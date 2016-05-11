using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Framework.EnumHelper;

namespace WYJK.Framework.EnumHelper
{
    /// <summary>
    /// 户籍性质
    /// </summary>
    public enum HouseholdPropertyEnum
    {
        [EnumDisplayName("本市农村")]
        InRural = 1,

        [EnumDisplayName("本市城镇")]
        InTown = 2,

        [EnumDisplayName("外市农村")]
        OutRural = 3,

        [EnumDisplayName("外市城镇")]
        OutTown = 4
    }

    /// <summary>
    /// 参保状态
    /// </summary>
    public enum SocialSecurityStatusEnum
    {
        [EnumDisplayName("未参保")]
        UnInsured = 1,
        [EnumDisplayName("待办")]
        WaitingHandle = 2,
        [EnumDisplayName("正常")]
        Normal = 3,
        [EnumDisplayName("续费")]
        Renew = 4,
        [EnumDisplayName("待停")]
        WaitingStop = 5,
        [EnumDisplayName("已停")]
        AlreadyStop = 6
    }

    ///// <summary>
    ///// 参公积金状态
    ///// </summary>
    //public enum AccumulationFundEnum
    //{
    //    [EnumDisplayName("未参公积金")]
    //    UnAccumulationFund = 1,
    //    [EnumDisplayName("正常")]
    //    NormalAccumulationFund = 2

    //}

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderEnum
    {
        [EnumDisplayName("待付款")]
        WaitingPay = 0,
        [EnumDisplayName("审核中")]
        Auditing = 1,
        [EnumDisplayName("已完成")]
        completed = 2
    }

    /// <summary>
    /// 收支类型
    /// </summary>
    public enum ShouZhiTypeEnum
    {
        [EnumDisplayName("收入")]
        ShouRu = 0,
        [EnumDisplayName("支出")]
        ZhiChu = 1
    }

    /// <summary>
    /// 来源
    /// </summary>
    public enum LaiYuanTypeEnum
    {
        [EnumDisplayName("微信")]
        WeiXin = 0,
        [EnumDisplayName("支付宝")]
        ZhiFuBao = 1,
        [EnumDisplayName("银联")]
        YinLian = 2
    }

    /// <summary>
    /// 待停
    /// </summary>
    public enum waitingTopEnum
    {
        [EnumDisplayName("申请")]
        Apply = 0,
        [EnumDisplayName("未续费")]
        UnRenew = 1
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypeEnum
    {
        [EnumDisplayName("个人")]
        GeRen = 0,
        [EnumDisplayName("企业")]
        QiYe = 1,
        [EnumDisplayName("个体经营")]
        GeTiJingYing = 2
    }

    /// <summary>
    /// 客服审核
    /// </summary>
    public enum CustomerServiceAuditEnum
    {
        [EnumDisplayName("未审核")]
        NoAudited = 0,
        [EnumDisplayName("已通过")]
        Pass = 1,
    }

    /// <summary>
    /// 缴费类型
    /// </summary>
    public enum PayTypeEnum
    {
        [EnumDisplayName("社保")]
        SocialSecurity = 0,
        [EnumDisplayName("公积金")]
        AccumulationFund = 1,
    }

    /// <summary>
    /// 缴费类型
    /// </summary>
    public enum LoanStatusEnum
    {
        [EnumDisplayName("未借款")]
        SocialSecurity = 1,
        [EnumDisplayName("还款中")]
        AccumulationFund = 1,
    }




    public class HouseholdPropertyClass
    {
        public static List<HouseholdProperty> GetList(Type enumType)

        {

            List<HouseholdProperty> list = new List<HouseholdProperty>();

            foreach (object e in Enum.GetValues(enumType))

            {

                list.Add(new HouseholdProperty() { Text = EnumExt.GetEnumCustomDescription(e), Value = ((int)e) });

            }

            return list;

        }
    }

    /// <summary>
    /// 户籍属性类
    /// </summary>
    public class HouseholdProperty
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}