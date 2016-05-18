using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IService;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Http
{
    /// <summary>
    /// 订单接口
    /// </summary>
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService = new OrderService();
        private readonly ISocialSecurityService _socialSecurityService = new SocialSecurityService();
        /// <summary>
        /// 生成订单 Data里头是订单编号
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult<dynamic> GenerateOrder(GenerateOrderParameter parameter)
        {
            ////参保人ID数组
            //string[] SocialSecurityPeopleIDS = HttpContext.Current.Request.Form.GetValues("SocialSecurityPeopleIDS");

            ////用户ID
            //int MemberID = Convert.ToInt32(HttpContext.Current.Request.Form["MemberID"]);

            string SocialSecurityPeopleIDStr = string.Join(",", parameter.SocialSecurityPeopleIDS);

            string orderCode = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000).ToString().PadLeft(3, '0');

            Dictionary<bool, string> dic = _orderService.GenerateOrder(SocialSecurityPeopleIDStr, parameter.MemberID, orderCode);

            return new JsonResult<dynamic>
            {
                status = dic.First().Key,
                Message = dic.First().Key ? "订单生成成功！" : "订单生成失败！",
                Data = dic.First().Value
            };
        }

        /// <summary>
        /// 是否可以自动付款  
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public JsonResult<dynamic> IsCanAutoPayment(GenerateOrderParameter parameter)
        {
            //判断待办与正常的参保人所有所要缴纳金额+本次金额之和与账户金额作比较

            //待办与正常的参保人所有所要缴纳金额
            decimal totalAmount = _socialSecurityService.GetMonthTotalAmountByMemberID(parameter.MemberID);
            //本次订单金额
            string SocialSecurityPeopleIDsStr = string.Join(",", parameter.SocialSecurityPeopleIDS);
            string sqlstr = $@"select SUM(ISNULL(dbo.SocialSecurity.SocialSecurityBase*SocialSecurity.PayProportion/100,0)*ISNULL(dbo.SocialSecurity.PayMonthCount,0)+
ISNULL(dbo.AccumulationFund.AccumulationFundBase*AccumulationFund.PayProportion/100,0)*ISNULL(dbo.AccumulationFund.PayMonthCount,0))
from dbo.SocialSecurityPeople 
left join dbo.SocialSecurity on SocialSecurityPeople.SocialSecurityPeopleID = SocialSecurity.SocialSecurityPeopleID 
left join dbo.AccumulationFund on SocialSecurityPeople.SocialSecurityPeopleID=AccumulationFund.SocialSecurityPeopleID 
where SocialSecurityPeople.SocialSecurityPeopleID in({SocialSecurityPeopleIDsStr})";
            decimal SSAF_Amount = DbHelper.QuerySingle<decimal>(sqlstr);

            int SSNum = DbHelper.QuerySingle<int>($@"select count(1) from SocialSecurityPeople 
                left join SocialSecurity on SocialSecurityPeople.SocialSecurityPeopleID = SocialSecurity.SocialSecurityPeopleID
where SocialSecurityPeople.SocialSecurityPeopleID in({SocialSecurityPeopleIDsStr})");
            decimal SSBacklogCost = DbHelper.QuerySingle<decimal>("select BacklogCost from CostParameterSetting where Status =0 ") * SSNum;


            int AFNum = DbHelper.QuerySingle<int>($@"select count(1) from SocialSecurityPeople 
                left join AccumulationFund on SocialSecurityPeople.SocialSecurityPeopleID = AccumulationFund.SocialSecurityPeopleID
where SocialSecurityPeople.SocialSecurityPeopleID in({SocialSecurityPeopleIDsStr})");
            decimal AFBacklogCost = DbHelper.QuerySingle<decimal>("select BacklogCost from CostParameterSetting where Status =1 ") * SSNum;

            decimal ThisAmount = SSAF_Amount + SSBacklogCost + AFBacklogCost;

            decimal AccountAmount = DbHelper.QuerySingle<decimal>($"select ISNULL(Account,0) from Members where MemberID = {parameter.MemberID}");


            if (totalAmount + ThisAmount < AccountAmount)
                return new JsonResult<dynamic>
                {
                    status = true,
                    Message = "可以自动扣款"
                };
            else
                return new JsonResult<dynamic>
                {
                    status = false,
                    Message = "不可以自动扣款"
                };
        }


        /// <summary>
        /// 自动扣款
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult<dynamic> AutoPayment(GenerateOrderParameter parameter)
        {
            Dictionary<bool, string> dic = null;
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    #region 生成订单
                    string SocialSecurityPeopleIDStr = string.Join(",", parameter.SocialSecurityPeopleIDS);

                    string orderCode = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000).ToString().PadLeft(3, '0');

                    dic = _orderService.GenerateOrder(SocialSecurityPeopleIDStr, parameter.MemberID, orderCode);
                    #endregion

                    if (!dic.First().Key) throw new Exception("自动扣款失败");

                    string sqlOrderDetail = $"select * from OrderDetails where OrderCode ={dic.First().Value}";
                    List<OrderDetails> orderDetailList = DbHelper.Query<OrderDetails>(sqlOrderDetail);

                    string sqlAccountRecord = "";
                    string sqlSocialSecurityPeople = "";
                    //支出记录
                    foreach (var orderDetail in orderDetailList)
                    {
                        sqlSocialSecurityPeople += $"update SocialSecurityPeople set IsPay=1 where SocialSecurityPeopleID ={orderDetail.SocialSecurityPeopleID};";
                        sqlAccountRecord += orderDetail.SocialSecurityFirstBacklogCost != 0 ? $"insert into AccountRecord(MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,CreateTime) values({parameter.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','支出','余额','社保代办',{orderDetail.SocialSecurityFirstBacklogCost},getdate());" : string.Empty;
                        sqlAccountRecord += orderDetail.AccumulationFundFirstBacklogCost != 0 ? $"insert into AccountRecord(MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,CreateTime) values({parameter.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','支出','余额','公积金代办',{orderDetail.AccumulationFundFirstBacklogCost},getdate());" : string.Empty;
                    }

                    //更新未参保人的支付状态
                    DbHelper.ExecuteSqlCommand(sqlSocialSecurityPeople, null);

                    //更新记录
                    DbHelper.ExecuteSqlCommand(sqlAccountRecord, null);

                    //更新订单
                    string sqlUpdateOrder = $"update [Order] set Status = {(int)OrderEnum.completed},PaymentMethod='余额扣款',PayTime=getdate() where OrderCode={dic.First().Value}";
                    DbHelper.ExecuteSqlCommand(sqlUpdateOrder, null);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return new JsonResult<dynamic>
                    {
                        status = false,
                        Message = "自动扣款失败"
                    };
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return new JsonResult<dynamic>
            {
                status = true,
                Message = "自动扣款成功",
                Data = dic.First<KeyValuePair<bool, string>>().Value
            };
        }

        /// <summary>
        /// 获取订单列表 0：待付款、1：审核中、2：已完成
        /// </summary>
        /// <returns></returns>
        public JsonResult<dynamic> GetOrderList(int MemberID, int Status)
        {
            List<OrderListForMobile> list = _orderService.GetOrderList(MemberID, Status);
            return new JsonResult<dynamic>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult<dynamic> CancelOrder(OrderCodeArrayParameter parameter)
        {
            string OrderCodeStrs = string.Join("','", parameter.OrderCode);
            bool flag = _orderService.CancelOrder(OrderCodeStrs);
            return new JsonResult<dynamic>
            {
                status = flag,
                Message = flag ? "取消成功" : "取消失败",
            };
        }

        /// <summary>
        /// 订单支付详情
        /// </summary>
        /// <returns></returns>
        public JsonResult<OrderDetailForMobile> GetOrderDetail(int MemberID, string OrderCode)
        {
            OrderDetailForMobile model = _orderService.GetOrderDetail(MemberID, OrderCode);

            return new JsonResult<OrderDetailForMobile>
            {
                status = true,
                Message = "获取成功",
                Data = model
            };
        }

        /// <summary>
        /// 订单支付 需要传订单编号、支付方式
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public JsonResult<dynamic> OrderPayment(Order model)
        {
            //社保、公积金、社保服务费、公积金服务费、社保第一次代办费、公积金第一次代办费
            //张三、账户余额、支出、支付宝、社保、450、时间

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    //社保公积金进入个人账户
                    //查询订单
                    string sqlOrder = $"select * from [Order] where OrderCode={model.OrderCode}";
                    Order order = DbHelper.QuerySingle<Order>(sqlOrder);

                    string sqlOrderDetail = $"select * from OrderDetails where OrderCode ={model.OrderCode}";
                    List<OrderDetails> orderDetailList = DbHelper.Query<OrderDetails>(sqlOrderDetail);
                    //计算出要进入个人账户的总额
                    decimal Account = 0;
                    orderDetailList.ForEach(n =>
                    {
                        Account += n.SocialSecurityAmount * n.SocialSecuritypayMonth + n.AccumulationFundAmount * n.AccumulationFundpayMonth;
                    });
                    //更新个人账户
                    string sqlMember = $"update Members set Account=ISNULL(Account,0)+{Account} where MemberID={order.MemberID}";
                    int updateResult = DbHelper.ExecuteSqlCommand(sqlMember, null);
                    if (!(updateResult > 0)) throw new Exception("更新个人账户失败");

                    string sqlAccountRecord = "";
                    string sqlSocialSecurityPeople = "";
                    //收支记录
                    foreach (var orderDetail in orderDetailList)
                    {
                        sqlSocialSecurityPeople += $"update SocialSecurityPeople set IsPay=1 where SocialSecurityPeopleID ={orderDetail.SocialSecurityPeopleID};";

                        decimal accountNum = orderDetail.SocialSecurityAmount * orderDetail.SocialSecuritypayMonth + orderDetail.SocialSecurityServiceCost + orderDetail.SocialSecurityFirstBacklogCost + orderDetail.SocialSecurityBuCha
                            + orderDetail.AccumulationFundAmount * orderDetail.AccumulationFundpayMonth + orderDetail.AccumulationFundServiceCost + orderDetail.AccumulationFundFirstBacklogCost;
                        sqlAccountRecord += $"insert into AccountRecord(MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,CreateTime) values({order.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','收入','{model.PaymentMethod}','缴费',{accountNum},getdate());";

                        sqlAccountRecord += orderDetail.SocialSecurityFirstBacklogCost != 0 ? $"insert into AccountRecord(MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,CreateTime) values({order.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','支出','余额','社保代办',{orderDetail.SocialSecurityFirstBacklogCost},getdate());" : string.Empty;
                        sqlAccountRecord += orderDetail.AccumulationFundFirstBacklogCost != 0 ? $"insert into AccountRecord(MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,CreateTime) values({order.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','支出','余额','公积金代办',{orderDetail.AccumulationFundFirstBacklogCost},getdate());" : string.Empty;
                    }

                    //更新未参保人的支付状态
                    DbHelper.ExecuteSqlCommand(sqlSocialSecurityPeople, null);

                    //更新记录
                    DbHelper.ExecuteSqlCommand(sqlAccountRecord, null);

                    //更新订单
                    string sqlUpdateOrder = $"update [Order] set Status = {(int)OrderEnum.Auditing},PaymentMethod='{model.PaymentMethod}',PayTime=getdate() where OrderCode={model.OrderCode}";
                    DbHelper.ExecuteSqlCommand(sqlUpdateOrder, null);

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return new JsonResult<dynamic>
                    {
                        status = false,
                        Message = "支付失败"
                    };
                }
                finally
                {
                    transaction.Dispose();
                }
            }


            return new JsonResult<dynamic>
            {
                status = true,
                Message = "支付成功"
            };
        }

    }
}