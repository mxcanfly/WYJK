using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Data;
using WYJK.Entity;
using WYJK.HOME.Models;

namespace WYJK.HOME.Service
{
    public class UserOrderService
    {
        public List<UserOrderViewModel> GetOrderList(int memberID, int? status)
        {
            string appendStr = "";

            if (status != null)
            {
                appendStr = $@" and [Order].Status = {status.Value}";
            }

            string sql = $@"select 
	                        [Order].OrderCode,
	                        [Order].GenerateDate,
                            [Order].status,
	                        dbo.OrderPeopleName_add([order].OrderCode) Names , 
	                        SUM(OrderDetails.SocialSecurityAmount * OrderDetails.SocialSecuritypayMonth + OrderDetails.SocialSecurityServiceCost + OrderDetails.SocialSecurityFirstBacklogCost + OrderDetails.SocialSecurityBuCha + OrderDetails.AccumulationFundAmount * OrderDetails.AccumulationFundpayMonth + OrderDetails.AccumulationFundServiceCost + OrderDetails.AccumulationFundFirstBacklogCost) Amounts 
                          from[Order] right join OrderDetails on[Order].OrderCode = OrderDetails.OrderCode 
                          where[Order].MemberID = {memberID} {appendStr}
                          group by[order].OrderCode,[Order].GenerateDate,[Order].Status ";

            List<UserOrderViewModel> list = DbHelper.Query<UserOrderViewModel>(sql);

            return list;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public List<OrderDetaisViewModel> GetOrderDetails(string orderCode)
        {

            string sql = $@"select 
	                            *,
	                            ssp.HouseholdProperty,
	                            ss.InsuranceArea,
	                            ss.SocialSecurityBase,
	                            af.AccumulationFundBase
                            from OrderDetails od
	                            left join SocialSecurityPeople ssp on od.SocialSecurityPeopleID = ssp.SocialSecurityPeopleID 
	                            left join SocialSecurity ss on od.SocialSecurityPeopleID = ss.SocialSecurityPeopleID 
	                            left join AccumulationFund af on od.SocialSecurityPeopleID = af.SocialSecurityPeopleID
	                            where OrderCode = '{orderCode}'";
            return DbHelper.Query<OrderDetaisViewModel>(sql);

        }

    }
}