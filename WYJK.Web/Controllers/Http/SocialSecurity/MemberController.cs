using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Web.Filters;
using System.Text.RegularExpressions;
using WYJK.Data;

namespace WYJK.Web.Controllers.Http
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class MemberController : ApiController
    {
        private readonly IMemberService _memberService = new MemberService();

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<MemberRegisterModel>> RegisterMember(MemberRegisterModel entity)
        {
            Dictionary<bool, string> dic = await _memberService.RegisterMember(entity);

            if (dic.First().Key)
            {
                //根据用户名和手机号获取MemberID

                Members member = await DbHelper.QuerySingleAsync<Members>("select * from Members where MemberName=@MemberName and MemberPhone=@MemberPhone", new
                {
                    MemberName = entity.MemberName,
                    MemberPhone = entity.MemberPhone
                });
                entity.MemberID = member.MemberID;
            }

            return new JsonResult<MemberRegisterModel>
            {
                status = dic.First().Key,
                Message = dic.First().Value,
                Data = entity
            };
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<MemberLoginModel>> LoginMember(MemberLoginModel entity)
        {
            Dictionary<bool, string> dic = await _memberService.LoginMember(entity);

            if (dic.First().Key)
            {
                //根据用户名和手机号获取MemberID

                Members member = await DbHelper.QuerySingleAsync<Members>("select * from Members where MemberName=@MemberName or MemberPhone=@MemberPhone", new
                {
                    MemberName = entity.Account,
                    MemberPhone = entity.Account
                });
                entity.MemberID = member.MemberID;
                entity.MemberName = member.MemberName;
                entity.MemberPhone = member.MemberPhone;
            }


            return new JsonResult<MemberLoginModel>
            {
                status = dic.First().Key,
                Message = dic.First().Value,
                Data = entity
            };
        }

        /// <summary>
        /// 用户密码找回
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<MemberForgetPasswordModel>> ForgetPassword(MemberForgetPasswordModel entity)
        {
            Dictionary<bool, string> dic = await _memberService.ForgetPassword(entity);

            return new JsonResult<MemberForgetPasswordModel>
            {
                status = dic.First().Key,
                Message = dic.First().Value,
                Data = entity
            };
        }

        /// <summary>
        /// 获取用户信息 IsAuthentication:是否认证 0/未认证 1/已认证，IsComplete：是否信息补全 0/未补全 1/已补全,UserType 用户类型  0：普通用户、1：企业用户、2：个体用户
        /// </summary>
        /// <param name="MemberID">MemberID</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<JsonResult<dynamic>> GetMemberInfo(int MemberID)
        {
            Members member = await _memberService.GetMemberInfo(MemberID);
            return new JsonResult<dynamic>
            {
                status = true,
                Message = "获取成功",
                Data = new
                {
                    TrueName = member.TrueName,
                    MemberName = member.MemberName,
                    MemberPhone = member.MemberPhone,
                    IsAuthentication = member.IsAuthentication,
                    IsComplete = member.IsComplete,
                    UserType = member.UserType
                }
            };
        }

        /// <summary>
        /// 修改密码 根据MemberID修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<dynamic>> ModifyPassword(MemberMidifyPassword model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return new JsonResult<dynamic>
            //    {
            //        status = false,
            //        Message = ModelState.FirstModelStateError()
            //    };
            //}

            bool isTrue = await _memberService.IsTrueOldPassword(model);
            if (!isTrue)
                return new JsonResult<dynamic>
                {
                    status = false,
                    Message = "原密码错误"
                };
            bool flag = await _memberService.ModifyPassword(model);

            return new JsonResult<dynamic>
            {
                status = flag,
                Message = flag ? "修改成功" : "修改失败"
            };

        }

        /// <summary>
        /// 获取行业类型
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<string>> GetIndustryType()
        {
            List<string> list = new List<string>();
            list.Add("软件行业");
            list.Add("硬件行业");

            return new JsonResult<List<string>>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }

        /// <summary>
        /// 获取公司规模
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<string>> GetEnterprisePeopleNum()
        {
            List<string> list = new List<string>();
            list.Add("20人以下");
            list.Add("21-50");
            list.Add("50-100");

            return new JsonResult<List<string>>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }

        /// <summary>
        /// 企业资质认证
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<dynamic>> CommitEnterpriseCertification(EnterpriseCertification parameter)
        {
            bool flag = await _memberService.CommitEnterpriseCertification(parameter);

            ////验证身份证
            //if (!Regex.IsMatch(parameter.EnterpriseLegalIdentityCardNo, @"(^\d{18}$)|(^\d{15}$)"))
            //    return new JsonResult<dynamic>
            //    {
            //        status = false,
            //        Message = "身份证号填写错误"
            //    };

            return new JsonResult<dynamic>
            {
                status = flag,
                Message = flag ? "企业认证成功" : "企业认证失败"
            };
        }

        /// <summary>
        /// 个体认证
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult<dynamic>> CommitPersonCertification(IndividualCertification parameter)
        {
            bool flag = await _memberService.CommitPersonCertification(parameter);

            return new JsonResult<dynamic>
            {
                status = flag,
                Message = flag ? "个体认证成功" : "个体认证失败"
            };
        }
        /// <summary>
        /// 补充信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<dynamic>> CommitExtensionInformation(ExtensionInformationParameter model)
        {
            //int MemberID = Convert.ToInt32(HttpContext.Current.Request.Form["MemberID"]);
            bool flag = await _memberService.ModifyMemberExtensionInformation(model);

            return new JsonResult<dynamic>
            {
                status = flag,
                Message = flag ? "补充信息成功" : "补充信息失败"
            };
        }

        /// <summary>
        /// 获取补充信息
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<JsonResult<ExtensionInformation>> GetExtensionInformation(int MemberID)
        {
            //获取补充信息
            ExtensionInformation model = await _memberService.GetExtensionInformation(MemberID);

            return new JsonResult<ExtensionInformation>
            {
                status = true,
                Message = "获取成功",
                Data = model
            };
        }

        /// <summary>
        /// 获取证件类型
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<string>> GetCertificateType()
        {
            List<string> list = new List<string>() {
               "身份证","居住证","签证","护照","户口本","军人证","团员证","党员证","港澳通行证"
            };

            return new JsonResult<List<string>>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }

        /// <summary>
        /// 获取政治面貌
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<string>> GetPoliticalStatus()
        {
            List<string> list = new List<string>() {
                "中共党员","共青团员","群众"
            };
            return new JsonResult<List<string>>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }

        /// <summary>
        /// 获取学历
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<string>> GetEducation()
        {
            List<string> list = new List<string>() {
                "中专","高中","高职（大专）","本科","硕士","博士","博士后"
            };
            return new JsonResult<List<string>>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }


        /// <summary>
        /// 获取账单简单信息
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public JsonResult<AccountInfo> GetAccountInfo(int MemberID)
        {
            AccountInfo accountInfo = _memberService.GetAccountInfo(MemberID);
            return new JsonResult<AccountInfo>
            {
                status = true,
                Message = "获取账单信息成功",
                Data = accountInfo
            };
        }

        /// <summary>
        /// 获取账单列表 类型：收入/0,支出/1   简单页面只需传MemberID、PageIndex、PageSize   显示属性如：OperationType，CostDisplay，CreateTime
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="ShouZhiType"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<JsonResult<PagedResult<AccountRecord>>> GetAccountRecordList(int MemberID, string ShouZhiType = "", DateTime? StartTime = null, DateTime? EndTime = null, int PageIndex = 1, int PageSize = 1)
        {
            PagedResult<AccountRecord> list = await _memberService.GetAccountRecordList(MemberID, ShouZhiType, StartTime, EndTime, new PagedParameter { PageIndex = PageIndex, PageSize = PageSize });
            list.Items.ToList().ForEach(n =>
            {
                if (n.ShouZhiType.Trim() == "收入")
                    n.CostDisplay = "+" + Convert.ToString(n.Cost);
                else if (n.ShouZhiType.Trim() == "支出")
                    n.CostDisplay = "-" + Convert.ToString(n.Cost);
            });

            return new JsonResult<PagedResult<AccountRecord>>
            {
                status = true,
                Message = "获取账单列表成功",
                Data = list
            };
        }

        /// <summary>
        /// 账单记录   显示属性：ShouZhiType，LaiYuan，OperationType，CostDisplay，CreateTime
        /// </summary>
        /// <param name="ID">账单ID</param>
        /// <returns></returns>
        public async Task<JsonResult<AccountRecord>> GetAccountRecord(int ID)
        {
            AccountRecord model = await _memberService.GetAccountRecord(ID);
            if (model.ShouZhiType.Trim() == "收入")
                model.CostDisplay = "+" + Convert.ToString(model.Cost);
            else if (model.ShouZhiType.Trim() == "支出")
                model.CostDisplay = "-" + Convert.ToString(model.Cost);

            return new JsonResult<AccountRecord>
            {
                status = true,
                Message = "获取账单记录成功",
                Data = model
            };
        }

        /// <summary>
        /// 获取账户状态 --待确定
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        private JsonResult<dynamic> GetAccountStatus(int MemberID)
        {

            return new JsonResult<dynamic>
            {
                status = true,
                Message = "获取状态成功"
            };
        }


        /// <summary>
        /// 创建续费服务
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        private JsonResult<dynamic> CreateRenewalService(int MemberID)
        {

            return new JsonResult<dynamic>
            {
                status = true,
                Message = "获取状态成功"
            };
        }

        /// <summary>
        /// 续费充值
        /// </summary>
        /// <returns></returns>
        private JsonResult<dynamic> SaveRenewalService()
        {
            return new JsonResult<dynamic>
            {
                status = true,
                Message = "获取状态成功"
            };
        }

        ///// <summary>
        ///// 获取多个错误
        ///// </summary>
        ///// <returns></returns>
        //private string AllModelStateErrors()
        //{
        //    List<string> sb = new List<string>();
        //    //获取所有错误的Key
        //    List<string> Keys = ModelState.Keys.ToList();
        //    //获取每一个key对应的ModelStateDictionary
        //    foreach (var key in Keys)
        //    {
        //        var errors = ModelState[key].Errors.ToList();
        //        //将错误描述添加到sb中
        //        foreach (var error in errors)
        //        {
        //            sb.Add(error.ErrorMessage);
        //        }
        //    }
        //    return string.Join("\n", sb);
        //}

    }
}