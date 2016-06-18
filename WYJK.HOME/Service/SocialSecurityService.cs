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
using WYJK.HOME.Models;

namespace WYJK.HOME.Service
{
    public class SocialSecurityService
    {
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








    }
}