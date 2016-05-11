using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Data.IService;
using WYJK.Entity;

namespace WYJK.Data.ServiceImpl
{
    /// <summary>
    /// 公积金
    /// </summary>
    public class AccumulationFundService : IAccumulationFundService
    {

        /// <summary>
        /// 获取公积金列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<PagedResult<AccumulationFundShowModel>> GetAccumulationFundList(AccumulationFundParameter parameter)
        {
            string userTypeSql = string.IsNullOrEmpty(parameter.UserType) ? "1=1" : "UserType=" + parameter.UserType;

            string sql = $"select * from (select ROW_NUMBER() OVER(ORDER BY S.AccumulationFundID )AS Row,s.* from View_AccumulationFund s where " + userTypeSql + $" and Status = {parameter.Status} and SocialSecurityPeopleName like '%{parameter.SocialSecurityPeopleName}%' and IdentityCard like '%{parameter.IdentityCard}%' ) ss WHERE ss.Row BETWEEN @StartIndex AND @EndIndex";

            List<AccumulationFundShowModel> modelList = await DbHelper.QueryAsync<AccumulationFundShowModel>(sql, new
            {
                StartIndex = parameter.SkipCount,
                EndIndex = parameter.TakeCount
            });

            int totalCount = await DbHelper.QuerySingleAsync<int>($"SELECT COUNT(0) AS TotalCount FROM View_AccumulationFund where  " + userTypeSql + $" and Status = {parameter.Status} and  SocialSecurityPeopleName like '%{parameter.SocialSecurityPeopleName}%' and IdentityCard like '%{parameter.IdentityCard}%'");

            return new PagedResult<AccumulationFundShowModel>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = totalCount,
                Items = modelList
            };
        }

        /// <summary>
        /// 修改公积金状态
        /// </summary>
        /// <param name="SocialSecurityPeopleIDs"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ModifyAccumulationFundStatus(int[] SocialSecurityPeopleIDs, int status)
        {
            string sql = $"update AccumulationFund set Status={status} where SocialSecurityPeopleID in({string.Join(",", SocialSecurityPeopleIDs)})";

            int result = DbHelper.ExecuteSqlCommand(sql, null);

            return result > 0;
        }

        /// <summary>
        /// 获取公积金详情
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <returns></returns>

        public AccumulationFund GetAccumulationFundDetail(int SocialSecurityPeopleID)
        {
            string sql = $"select AccumulationFund.*,SocialSecurityPeople.SocialSecurityPeopleName from AccumulationFund left join SocialSecurityPeople on SocialSecurityPeople.SocialSecurityPeopleID = AccumulationFund.SocialSecurityPeopleID   where AccumulationFund.SocialSecurityPeopleID ={SocialSecurityPeopleID}";
            AccumulationFund model = DbHelper.QuerySingle<AccumulationFund>(sql);
            return model;
        }

        /// <summary>
        /// 保存公积金号
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <param name="AccumulationFundNo"></param>
        /// <returns></returns>
        public bool SaveAccumulationFundNo(int SocialSecurityPeopleID, string AccumulationFundNo)
        {
            string sql = $"update AccumulationFund set AccumulationFundNo ='{AccumulationFundNo}' where SocialSecurityPeopleID={SocialSecurityPeopleID}";
            int result = DbHelper.ExecuteSqlCommand(sql, null);
            return result > 0;
        }

        /// <summary>
        /// 修改公积金状态
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ModifyAccumulationFundStatus(int SocialSecurityPeopleID, int status)
        {
            string sqlstr = $"update AccumulationFund set Status={status} where SocialSecurityPeopleID={SocialSecurityPeopleID}";
            int result = DbHelper.ExecuteSqlCommand(sqlstr, null);
            return result >= 0;

        }
    }
}
