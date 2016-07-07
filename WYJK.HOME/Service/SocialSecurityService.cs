using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WYJK.Data;
using WYJK.Data.IServices;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;
using WYJK.HOME.Models;

namespace WYJK.HOME.Service
{
    public class SocialSecurityService
    {
        ISocialSecurityService _socialSecurity = new WYJK.Data.ServiceImpl.SocialSecurityService();

        #region 用户借款条件考察


        /// <summary>
        /// 是否进行过身价计算
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool LoanCalculator(int memberId)
        {
            string sql = $@"select 
	                            COUNT(ID) from MemberLoan
                            where MemberID = {memberId}";

            int count = DbHelper.QuerySingle<int>(sql);

            if (count > 0)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 是否缴费满三个月
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool PayedMonthCount(int memberId)
        {
            string sql = $@"select 
	                            case when MAX(ss.PayedMonthCount) is null then 0 else MAX(ss.PayedMonthCount) end
                            from SocialSecurityPeople ssp
	                            left join SocialSecurity ss on ssp.SocialSecurityPeopleID = ss.SocialSecurityPeopleID
                            where MemberID = {memberId}";
            int count = DbHelper.QuerySingle<int>(sql);

            if (count >= 3)
            {
                return true;
            }
            return false;
            

        }

        /// <summary>
        /// 用户是否有参保人
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public bool ExistSocialPeople(int memberId)
        {
            string sql = $@"select 
	                            COUNT(*)
                            from SocialSecurityPeople
	                            where MemberID = {memberId}";
            List<int> list = DbHelper.Query<int>(sql);

            return list.Count > 0 ? true : false;
        }

        #endregion

        /// <summary>
        /// 参保人添加
        /// </summary>
        /// <param name="socialSecurityPeople"></param>
        /// <returns></returns>
        public int AddSocialSecurityPeople(SocialSecurityPeople socialSecurityPeople)
        {
            DbParameter[] parameters = new DbParameter[]{
                new SqlParameter("@Flag", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                //参保人
                new SqlParameter("@MemberID", SqlDbType.Int) { Value = socialSecurityPeople.MemberID },
                new SqlParameter("@SocialSecurityPeopleName", SqlDbType.NVarChar, 50) { Value = socialSecurityPeople.SocialSecurityPeopleName },
                new SqlParameter("@IdentityCard", SqlDbType.NVarChar, 50) { Value = socialSecurityPeople.IdentityCard },
                new SqlParameter("@IdentityCardPhoto", SqlDbType.NVarChar, 512) { Value = socialSecurityPeople.IdentityCardPhoto },
                new SqlParameter("@HouseholdProperty", SqlDbType.NVarChar,512) { Value=socialSecurityPeople.HouseholdProperty },
            };

            string sql = @"insert into SocialSecurityPeople
	                                        (
	                                        MemberID,
	                                        SocialSecurityPeopleName,
	                                        IdentityCard,
	                                        IdentityCardPhoto,
	                                        HouseholdProperty,
	                                        IsPaySocialSecurity,
	                                        IsPayAccumulationFund
	                                        )
                                        values
	                                        (
	                                        @MemberID,
	                                        @SocialSecurityPeopleName,
	                                        @IdentityCard,
	                                        @IdentityCardPhoto,
	                                        @HouseholdProperty,
	                                        0,
	                                        0
	                                        )";

            int id = DbHelper.ExecuteSqlCommandScalar(sql, parameters);

            return id;

        }

        /// <summary>
        /// 参保信息确认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SocialSecurityPeopleViewModel SocialSecurityDetail(int id)
        {
            string sql = $@"select 
	                        ssp.SocialSecurityPeopleID,
	                        ssp.SocialSecurityPeopleName,
	                        ss.InsuranceArea,
	                        ssp.HouseholdProperty,
	                        ss.PayTime,
	                        ss.SocialSecurityBase,
	                        af.AccumulationFundBase
                        from SocialSecurityPeople ssp
	                        left join SocialSecurity ss on ssp.SocialSecurityPeopleID = ss.SocialSecurityPeopleID
	                        left join AccumulationFund af on ssp.SocialSecurityPeopleID = af.SocialSecurityPeopleID
                        where ssp.SocialSecurityPeopleID = {id}";
            return DbHelper.QuerySingle<SocialSecurityPeopleViewModel>(sql);
        }


        /// <summary>
        /// 申请停保
        /// </summary>
        /// <param name="stopReason"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ApplyTopSocialSecurity(string stopReason,string[] ids)
        {

            string sql = $"update SocialSecurity set Status={(int)SocialSecurityStatusEnum.WaitingStop}, StopMethod = {(int)waitingTopEnum.Apply},ApplyStopDate=getdate(),StopReason='{stopReason}' where SocialSecurityPeopleID in ({string.Join(",", ids)})";

            int result = DbHelper.ExecuteSqlCommand(sql, null);

            return result > 0;
        }

        /// <summary>
        /// 申请停公积金
        /// </summary>
        /// <param name="stopReason"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool ApplyTopAccumulationFund(string stopReason, string[] ids)
        {

            string sql = $"update AccumulationFund set Status={(int)SocialSecurityStatusEnum.WaitingStop}, StopMethod = {(int)waitingTopEnum.Apply},ApplyStopDate=getdate() where SocialSecurityPeopleID  in ({string.Join(",", ids)})";

            int result = DbHelper.ExecuteSqlCommand(sql, null);

            return result > 0;
        }

        /// <summary>
        /// 获取社保公积金基数变更记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<SocialSecurityPeopleViewModel> GetBaseAjustRecord(int type,int memberId)
        {
            List<SocialSecurityPeopleViewModel> list = new List<SocialSecurityPeopleViewModel>();

            string sql = $@"select 
	                        ssp.SocialSecurityPeopleName,
	                        ssp.IdentityCard,
	                        ssp.HouseholdProperty,
	                        ba.CurrentBase,
	                        ba.BaseAdjusted 
                        from BaseAudit ba
	                        left join SocialSecurityPeople ssp on ba.SocialSecurityPeopleID = ssp.SocialSecurityPeopleID
                        where MemberID = {memberId} and ba.Type = {type}";

            return DbHelper.Query<SocialSecurityPeopleViewModel>(sql);

        }

        /// <summary>
        /// 获取用户名下的参保人
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<SocialSecurity> GetSocialSecurityPersons(int memberId)
        {
            string sql = $@"select 
	                            ssp.SocialSecurityPeopleID,
	                            ssp.SocialSecurityPeopleName
                            from SocialSecurityPeople ssp
	                            inner join SocialSecurity ss on ssp.SocialSecurityPeopleID = ss.SocialSecurityPeopleID
                            where MemberID = {memberId}";

            return DbHelper.Query<SocialSecurity>(sql);

        }

        /// <summary>
        /// 获取参保人详情
        /// </summary>
        /// <param name="socialId"></param>
        /// <returns></returns>
        public SocialSecurityViewModel GetSocialSecurity(int socialId)
        {
            string sql = $@"select 
	                        ssp.SocialSecurityPeopleID,
	                        ssp.IdentityCard,
	                        ss.SocialSecurityBase,
	                        ssp.HouseholdProperty,
	                        ss.InsuranceArea
                        from SocialSecurityPeople ssp
	                        inner join SocialSecurity ss on ssp.SocialSecurityPeopleID = ss.SocialSecurityPeopleID
                        where ssp.SocialSecurityPeopleID = {socialId}";

            SocialSecurityViewModel vm = DbHelper.QuerySingle<SocialSecurityViewModel>(sql);
            vm.SocialAccumulationDict = SocialAccumulationBase(vm.InsuranceArea, vm.HouseholdProperty);
            return vm;
        }

        /// <summary>
        /// 获取社保公积金基数
        /// </summary>
        /// <param name="area"></param>
        /// <param name="householdProperty"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> SocialAccumulationBase(string area,string householdProperty)
        {
            EnterpriseSocialSecurity model = _socialSecurity.GetDefaultEnterpriseSocialSecurityByArea(area, householdProperty);

            decimal minBase = 0;
            decimal maxBase = 0;
            decimal aFMinBase = 0;
            decimal aFMaxBase = 0;


            if (model != null)
            {
                minBase = Math.Round(model.SocialAvgSalary * (model.MinSocial / 100));
                maxBase = Math.Round(model.SocialAvgSalary * (model.MaxSocial / 100));
                aFMinBase = model.MinAccumulationFund;
                aFMaxBase = model.MaxAccumulationFund;
            }

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict["MinBase"] = minBase;
            dict["MaxBase"] = maxBase;
            dict["AFMinBase"] = aFMinBase;
            dict["AFMaxBase"] = aFMaxBase;

            return dict;
        }

        /// <summary>
        /// 获取用户名下的公积金参保人
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<AccumulationFund> GetAccumulationFundPersons(int memberId)
        {
            string sql = $@"select 
	                            ssp.SocialSecurityPeopleID,
	                            ssp.SocialSecurityPeopleName
                            from SocialSecurityPeople ssp
	                            inner join AccumulationFund af on ssp.SocialSecurityPeopleID = af.SocialSecurityPeopleID
                            where ssp.MemberID = {memberId}";

            return DbHelper.Query<AccumulationFund>(sql);

        }

    }
}