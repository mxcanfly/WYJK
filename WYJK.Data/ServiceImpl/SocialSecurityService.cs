using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Data.IServices;
using WYJK.Entity;
using WYJK.Entity.Parameters;

namespace WYJK.Data.Services
{
    public class SocialSecurityService : ISocialSecurityService
    {
        /// <summary>
        /// 删除未参保人
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteUninsuredPeople(int SocialSecurityPeopleID)
        {
            string sql = "delete from SocialSecurityPeople"
                                + " left join SocialSecurity"
                                + " on SocialSecurityPeople.SocialSecurityPeopleID = SocialSecurity.SocialSecurityPeopleID"
                                + " left join AccumulationFund"
                                + " on SocialSecurityPeople.SocialSecurityPeopleID = AccumulationFund.SocialSecurityPeopleID"
                                + " where SocialSecurityPeople.SocialSecurityPeopleID = @SocialSecurityPeopleID ";
            DbParameter[] parameters = {
                new SqlParameter("@SocialSecurityPeopleID",SqlDbType.Int) { Value=SocialSecurityPeopleID}
            };
            int result = await DbHelper.ExecuteSqlCommandAsync(sql, parameters);

            return result > 0;

        }

        /// <summary>
        /// 获取社保列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<PagedResult<SocialSecurityShowModel>> GetSocialSecurityList(SocialSecurityParameter parameter)
        {
            string sql = $"select * from (select ROW_NUMBER() OVER(ORDER BY S.SocialSecurityID )AS Row,s.* from View_SocialSecurity s where SocialSecurityPeopleName like '%{parameter.SocialSecurityPeopleName}%' and IdentityCard like '%{parameter.IdentityCard}%' ) ss WHERE ss.Row BETWEEN @StartIndex AND @EndIndex";

            List<SocialSecurityShowModel> modelList = await DbHelper.QueryAsync<SocialSecurityShowModel>(sql, new
            {
                StartIndex = parameter.SkipCount,
                EndIndex = parameter.TakeCount
            });

            int totalCount = await DbHelper.QuerySingleAsync<int>($"SELECT COUNT(0) AS TotalCount FROM View_SocialSecurity  where SocialSecurityPeopleName like '%{parameter.SocialSecurityPeopleName}%' and IdentityCard like '%{parameter.IdentityCard}%'");

            return new PagedResult<SocialSecurityShowModel>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = totalCount,
                Items = modelList
            };
        }

        /// <summary>
        /// 获取参保人列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<InsuredPeople>> GetInsuredPeopleList()
        {
            string sql = "select SocialSecurityPeople.SocialSecurityPeopleID,SocialSecurityPeople.SocialSecurityPeopleName,"
                        + " SocialSecurity.PayTime SSPayTime, SocialSecurity.PayMonthCount SSPayMonthCount,"
                        + " AccumulationFund.PayTime AFPayTime, AccumulationFund.PayMonthCount AFPayMonthCount"
                        + " from SocialSecurityPeople"
                        + " left join SocialSecurity"
                        + " on SocialSecurityPeople.SocialSecurityPeopleID = SocialSecurity.SocialSecurityPeopleID"
                        + " left join dbo.AccumulationFund"
                        + " on SocialSecurityPeople.SocialSecurityPeopleID = AccumulationFund.SocialSecurityPeopleID";
            List<InsuredPeople> isuredPeopleList = await DbHelper.QueryAsync<InsuredPeople>(sql);
            return isuredPeopleList;
        }
    }
}
