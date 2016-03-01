using Newtonsoft.Json;
using System;
using WYJK.Framework.Helpers;

namespace WYJK.Entity
{
    //注册用户
    public class User
    {

        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>		
        public string UserPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>		
        public string Password { get; set; }
        /// <summary>
        /// 注册邀请码
        /// </summary>		
        public string InviteCode { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>		
        public string TrueName { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>		
        public string CertificateType { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>		
        public string CertificateNo { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>		
        public string PoliticalStatus { get; set; }
        /// <summary>
        /// 学历
        /// </summary>		
        public string Education { get; set; }
        /// <summary>
        /// 生日
        /// </summary>		
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 性别0：男，1：女
        /// </summary>		
        public string Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>		
        public string Address { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>		
        public string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>		
        public string Email { get; set; }
        /// <summary>
        /// QQ
        /// </summary>		
        public string QQ { get; set; }
        /// <summary>
        /// 支付宝账号
        /// </summary>		
        public string Alipay { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>		
        public string BankCardNo { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>		
        public string BankAccount { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>		
        public string UserAccount { get; set; }
        /// <summary>
        /// 第二联系人
        /// </summary>		
        public string SecondContact { get; set; }
        /// <summary>
        /// 第二联系人手机
        /// </summary>		
        public string SecondContactPhone { get; set; }
        /// <summary>
        /// 投保地
        /// </summary>		
        public string InsuranceArea { get; set; }
        /// <summary>
        /// 户口性质
        /// </summary>		
        public string HouseholdType { get; set; }
        /// <summary>
        /// 传真
        /// </summary>		
        public string Fax { get; set; }
        /// <summary>
        /// 营业执照编码
        /// </summary>		
        public string BusinessCode { get; set; }
        /// <summary>
        /// 经营者身份证号
        /// </summary>		
        public string BusinessIdentityCardNo { get; set; }
        /// <summary>
        /// 营业执照名称
        /// </summary>		
        public string BusinessName { get; set; }
        /// <summary>
        /// 经营者姓名
        /// </summary>		
        public string BusinessUser { get; set; }
        /// <summary>
        /// 经营者身份证正反面
        /// </summary>		
        public string BusinessIdentityPhoto { get; set; }
        /// <summary>
        /// 营业执照照片
        /// </summary>		
        public string BusinessLicensePhoto { get; set; }
        /// <summary>
        /// 企业申请人姓名
        /// </summary>		
        public string EnterpriseApplyName { get; set; }
        /// <summary>
        /// 申请人身份证号
        /// </summary>		
        public string EnterpriseApplyIdentityCardNo { get; set; }
        /// <summary>
        /// 申请人身份证照片正反面
        /// </summary>		
        public string EnterpriseApplyIdentityPhoto { get; set; }
        /// <summary>
        /// 企业申请人手机
        /// </summary>		
        public string EnterpriseApplyUserPhone { get; set; }
        /// <summary>
        /// 申请人邮箱
        /// </summary>		
        public string EnterpriseApplyUserEmail { get; set; }
        /// <summary>
        /// 授权协议数照片
        /// </summary>		
        public string EnterpriseLicensePhone { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>		
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 行业类型
        /// </summary>		
        public string EnterpriseType { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>		
        public string EnterpriseArea { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>		
        public string EnterpriseLegal { get; set; }
        /// <summary>
        /// 法人代表身份证号
        /// </summary>		
        public string EnterpriseLegalIdentityCardNo { get; set; }
        /// <summary>
        /// 企业人数范围
        /// </summary>		
        public string EnterprisePeopleNum { get; set; }
        /// <summary>
        /// 是否三证合一
        /// </summary>		
        public string EnterpriseIsThreeInOne { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>		
        public string EnterpriseBusinessLicense { get; set; }
        /// <summary>
        /// 税务登记证
        /// </summary>		
        public string EnterpriseTaxRegistrationCertificate { get; set; }
        /// <summary>
        /// 组织机构代码证
        /// </summary>		
        public string EnterpriseOrganizationCodeCertificate { get; set; }
        /// <summary>
        /// 企业固定电话
        /// </summary>		
        public string EnterpriseFixedTelePhone { get; set; }
        /// <summary>
        /// 是否已认证
        /// </summary>		
        public string IsAuthentication { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>		
        public string UserType { get; set; }
        /// <summary>
        /// 纳税人名称
        /// </summary>		
        public string TaxpayerName { get; set; }
        /// <summary>
        /// 税号
        /// </summary>		
        public string TaxNumber { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>		
        public string ContactUser { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>		
        public string ContactUserPhone { get; set; }
        /// <summary>
        /// 开户电话
        /// </summary>		
        public string OpenAccountPhone { get; set; }

    }

    /// <summary>
    /// 用户简单信息
    /// </summary>
    public class UserSimpleModel
    {
        /// <summary>
        /// 用户名
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>		
        public string UserPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>		
        public string Password { get; set; }
        /// <summary>
        /// 加密后的密码
        /// </summary>
        public string HashPassword
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    return SecurityHelper.HashPassword(Password, Password);
                }
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// 注册用户
    /// </summary>
    public class UserRegisterModel : UserSimpleModel
    {
        /// <summary>
        /// 注册邀请码
        /// </summary>		
        public string InviteCode { get; set; }
    }

    public class UserForgetPasswordModel : UserSimpleModel
    {

    }

    /// <summary>
    /// 登录用户
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class UserLoginModel
    {
        /// <summary>
        /// 用户名或手机号
        /// </summary>		
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>		
        public string Password { get; set; }
        /// <summary>
        /// 加密后的密码
        /// </summary>
        [JsonIgnore]
        public string HashPassword
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    return SecurityHelper.HashPassword(Password, Password);
                }
                return string.Empty;
            }
        }

    }



}

