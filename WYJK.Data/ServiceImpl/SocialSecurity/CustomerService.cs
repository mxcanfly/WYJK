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
    /// 客服实现
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// 获取客户服务列表
        /// </summary>
        /// <returns></returns>
        public PagedResult<CustomerServiceViewModel> GetCustomerServiceList(CustomerServiceParameter parameter)
        {
            string sqlCs = "select members.MemberID,members.UserType,Members.MemberName,Members.MemberPhone,members.Account, SocialSecurityPeople.SocialSecurityPeopleName,SocialSecurityPeople.SocialSecurityPeopleID ,SocialSecurityPeople.Status CustomerServiceStatus, SocialSecurityPeople.IdentityCard,SocialSecurity.Status SSstatus, socialsecurity.SocialSecurityException,AccumulationFund.Status AFStatus, AccumulationFund.AccumulationFundException"
                        + " from Members"
                        + " left join SocialSecurityPeople on Members.MemberID = SocialSecurityPeople.MemberID"
                        + " left join SocialSecurity on SocialSecurityPeople.SocialSecurityPeopleID = socialsecurity.SocialSecurityPeopleID"
                        + " left join AccumulationFund on socialsecuritypeople.SocialSecurityPeopleID = AccumulationFund.SocialSecurityPeopleID";
                        //+ " left join OrderDetails on socialsecuritypeople.SocialSecurityPeopleID = OrderDetails.SocialSecurityPeopleID"
                        //+ " left join [order] on[order].OrderCode = OrderDetails.OrderCode";
                        //+ " where members.MemberID = 18";

            string sqlStr = $"select * from (select ROW_NUMBER() OVER(ORDER BY Cs.MemberID )AS Row,Cs.* from ({sqlCs}) Cs) ss WHERE ss.Row BETWEEN @StartIndex AND @EndIndex";

            List<CustomerServiceViewModel> modelList = DbHelper.Query<CustomerServiceViewModel>(sqlStr, new
            {
                StartIndex = parameter.SkipCount,
                EndIndex = parameter.TakeCount
            });

            int totalCount = DbHelper.QuerySingle<int>($"select count(0) as TotalCount from ({sqlCs}) Cs");

            return new PagedResult<CustomerServiceViewModel>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = totalCount,
                Items = modelList
            };
        }

        /// <summary>
        /// 修改客户服务状态变成已通过
        /// </summary>
        /// <param name="SocialSecurityPeopleIDs"></param>
        /// <returns></returns>
        public bool ModifyCustomerServiceStatus(int[] SocialSecurityPeopleIDs,int status)
        {
            string SocialSecurityPeopleIDStr = string.Join("','", SocialSecurityPeopleIDs);
            string sqlstr = $"update SocialSecurityPeople set Status ={status} where SocialSecurityPeopleID in ('{SocialSecurityPeopleIDStr}')";
            int result = DbHelper.ExecuteSqlCommand(sqlstr, null);
            return result > 0;
        }
    }
}
