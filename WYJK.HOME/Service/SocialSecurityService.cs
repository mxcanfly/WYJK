using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Data;

namespace WYJK.HOME.Service
{
    public class SocialSecurityService
    {
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

    }
}