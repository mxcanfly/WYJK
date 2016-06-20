using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IService;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class UserOrderController : BaseFilterController
    {
        UserOrderService userOderSv = new UserOrderService();

        Service.SocialSecurityService sss = new Service.SocialSecurityService();

        private readonly IOrderService _orderService = new OrderService();
        private readonly ISocialSecurityService _socialSecurityService = new Data.ServiceImpl.SocialSecurityService();


        // GET: UserOrder
        public ActionResult Index(PagedParameter parameter,int? id)
        {
            List<UserOrderViewModel> list = userOderSv.GetOrderList(CommonHelper.CurrentUser.MemberID,id);

            var c = list.Skip(parameter.SkipCount - 1).Take(parameter.PageSize);


            UserOrderPageResult<UserOrderViewModel> page = new UserOrderPageResult<UserOrderViewModel>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = list.Count,
                Items = c,
                Status = id
            };

            return View(page);
        }


        /// <summary>
        /// 订单确认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            SocialSecurityPeopleViewModel ssp = sss.SocialSecurityDetail(id.Value);
            //保费计算
            ssp.Calculation = _socialSecurityService.GetSocialSecurityCalculationResult(ssp.InsuranceArea, ssp.HouseholdProperty, ssp.SocialSecurityBase, ssp.AccumulationFundBase);
            return View(ssp);
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SocialSecurityPeopleViewModel model)
        {
            ////首先判断是否有未支付订单，若有，则不能生成订单
            //if (_orderService.IsExistsWaitingPayOrderByMemberID(CommonHelper.CurrentUser.MemberID))
            //{
            //    ViewBag.ErrorMessage = "有未支付的订单，请先进行支付";
            //}
            //判断所选参保人中有没有超过14号的
            string sqlstr = $"select * from SocialSecurity where SocialSecurityPeopleID in({model.SocialSecurityPeopleID})";
            List<SocialSecurity> socialSecurityList = DbHelper.Query<SocialSecurity>(sqlstr);
            foreach (var socialSecurity in socialSecurityList)
            {
                if (socialSecurity.PayTime.Month < DateTime.Now.Month || (socialSecurity.PayTime.Month == DateTime.Now.Month && socialSecurity.PayTime.Day > 13))
                {
                    ViewBag.ErrorMessage = "参保人日期已失效，请修改";
                }
            }

            string sqlstr1 = $"select * from AccumulationFund where SocialSecurityPeopleID in({model.SocialSecurityPeopleID})";
            List<AccumulationFund> accumulationFundList = DbHelper.Query<AccumulationFund>(sqlstr1);
            foreach (var accumulationFund in accumulationFundList)
            {
                if (accumulationFund.PayTime.Month < DateTime.Now.Month || (accumulationFund.PayTime.Month == DateTime.Now.Month && accumulationFund.PayTime.Day > 13))
                {
                    ViewBag.ErrorMessage = "参保人日期已失效，请修改";
                }
            }

            string orderCode = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000).ToString().PadLeft(3, '0');

            Dictionary<bool, string> dic = _orderService.GenerateOrder(model.SocialSecurityPeopleID.ToString(), CommonHelper.CurrentUser.MemberID, orderCode);

            return Redirect("/UserOrder/Pay/"+orderCode);
        }


        public ActionResult Pay(string id)
        {
            OrderDetaisViewModel detail = userOderSv.GetOrderDetails(id)[0];
            return View(detail);
        }

        [HttpPost]
        public ActionResult Pay(Order model)
        {
            model.MemberID = CommonHelper.CurrentUser.MemberID;

            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    string sqlOrder = $"select * from [Order] where OrderCode={model.OrderCode}";
                    Order order = DbHelper.QuerySingle<Order>(sqlOrder);

                    string sqlOrderDetail = $"select * from OrderDetails where OrderCode ={model.OrderCode}";
                    List<OrderDetails> orderDetailList = DbHelper.Query<OrderDetails>(sqlOrderDetail);

                    int[] SocialSecurityPeopleIDS = new int[orderDetailList.Count];
                    for (int i = 0; i < orderDetailList.Count; i++)
                    {
                        SocialSecurityPeopleIDS[i] = orderDetailList[i].SocialSecurityPeopleID;
                    }

                    string SocialSecurityPeopleIDStr = string.Join(",", SocialSecurityPeopleIDS);
                    //判断所选参保人中有没有超过14号的
                    string sqlstr = $"select * from SocialSecurity where SocialSecurityPeopleID in({SocialSecurityPeopleIDStr})";
                    List<SocialSecurity> socialSecurityList = DbHelper.Query<SocialSecurity>(sqlstr);
                    foreach (var socialSecurity in socialSecurityList)
                    {
                        if (socialSecurity.PayTime.Month < DateTime.Now.Month || (socialSecurity.PayTime.Month == DateTime.Now.Month && socialSecurity.PayTime.Day > 13))
                        {
                            ViewBag.ErrorMessage = "参保人日期已失效，请修改";
                        }
                    }

                    string sqlstr1 = $"select * from AccumulationFund where SocialSecurityPeopleID in({SocialSecurityPeopleIDStr})";
                    List<AccumulationFund> accumulationFundList = DbHelper.Query<AccumulationFund>(sqlstr1);
                    foreach (var accumulationFund in accumulationFundList)
                    {
                        if (accumulationFund.PayTime.Month < DateTime.Now.Month || (accumulationFund.PayTime.Month == DateTime.Now.Month && accumulationFund.PayTime.Day > 13))
                        {
                            ViewBag.ErrorMessage = "参保人日期已失效，请修改";
                        }
                    }

                    string sqlAccountRecord = "";
                    string sqlSocialSecurityPeople = "";

                    decimal memberAccount = DbHelper.QuerySingle<decimal>($"select Account from Members where MemberID = {model.MemberID}");
                    //收支记录
                    string ShouNote = "缴费：";
                    string ZhiNote = string.Empty;
                    decimal Bucha = 0;//补差
                    decimal ZhiAccount = 0;//支出总额
                    decimal accountNum = 0;//订单总额
                    foreach (var orderDetail in orderDetailList)
                    {
                        sqlSocialSecurityPeople += $"update SocialSecurityPeople set IsPay=1 where SocialSecurityPeopleID ={orderDetail.SocialSecurityPeopleID};";

                        accountNum += orderDetail.SocialSecurityAmount * orderDetail.SocialSecuritypayMonth + orderDetail.SocialSecurityFirstBacklogCost + orderDetail.SocialSecurityBuCha
                            + orderDetail.AccumulationFundAmount * orderDetail.AccumulationFundpayMonth + orderDetail.AccumulationFundFirstBacklogCost;
                        Bucha += orderDetail.SocialSecurityBuCha;
                        ZhiAccount += orderDetail.SocialSecurityFirstBacklogCost + orderDetail.SocialSecurityBuCha + orderDetail.AccumulationFundFirstBacklogCost;
                        ShouNote += (orderDetail.SocialSecurityAmount != 0 ? string.Format("{0}:{1}个月社保,单月保费:{2},社保待办费:{3},补差费:{4};", orderDetail.SocialSecurityPeopleName, orderDetail.SocialSecuritypayMonth, orderDetail.SocialSecurityAmount, orderDetail.SocialSecurityFirstBacklogCost, orderDetail.SocialSecurityBuCha) : string.Empty) + (orderDetail.AccumulationFundAmount != 0 ? string.Format("{0}:{1}个月公积金,单月公积金费:{2},公积金代办费:{3};", orderDetail.SocialSecurityPeopleName, orderDetail.AccumulationFundpayMonth, orderDetail.AccumulationFundAmount, orderDetail.AccumulationFundFirstBacklogCost) : string.Empty);
                        ZhiNote += (orderDetail.SocialSecurityAmount != 0 ? string.Format("{0}:社保待办费:{1},补差费:{2};", orderDetail.SocialSecurityPeopleName, orderDetail.SocialSecurityFirstBacklogCost, orderDetail.SocialSecurityBuCha) : string.Empty) + (orderDetail.AccumulationFundAmount != 0 ? string.Format("{0}:公积金代办费:{1};", orderDetail.SocialSecurityPeopleName, orderDetail.AccumulationFundFirstBacklogCost) : string.Empty);

                        #region 作废
                        //sqlAccountRecord += $"insert into AccountRecord(SerialNum,MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,Balance,CreateTime) values({DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000).ToString().PadLeft(3, '0')},{order.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','收入','{model.PaymentMethod}','缴费',{accountNum},{memberAccount},getdate());";
                        //memberAccount -= orderDetail.SocialSecurityFirstBacklogCost;
                        //sqlAccountRecord += orderDetail.SocialSecurityFirstBacklogCost != 0 ? $"insert into AccountRecord(SerialNum,MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,Balance,CreateTime) values({DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000).ToString().PadLeft(3, '0')},{order.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','支出','余额','社保代办',{orderDetail.SocialSecurityFirstBacklogCost},{memberAccount},getdate());" : string.Empty;
                        //memberAccount -= orderDetail.AccumulationFundFirstBacklogCost;
                        //sqlAccountRecord += orderDetail.AccumulationFundFirstBacklogCost != 0 ? $"insert into AccountRecord(SerialNum,MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,Balance,CreateTime) values({DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000).ToString().PadLeft(3, '0')},{order.MemberID},{orderDetail.SocialSecurityPeopleID},'{orderDetail.SocialSecurityPeopleName}','支出','余额','公积金代办',{orderDetail.AccumulationFundFirstBacklogCost},{memberAccount},getdate());" : string.Empty;
                        #endregion
                    }

                    sqlAccountRecord += $@"insert into AccountRecord(SerialNum,MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,Balance,CreateTime)
values({DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random(Guid.NewGuid().GetHashCode()).Next(1000).ToString().PadLeft(3, '0')},{model.MemberID},'','','收入','{model.PaymentMethod}','{ShouNote}',{accountNum},{memberAccount + accountNum},getdate());
                                       insert into AccountRecord(SerialNum,MemberID,SocialSecurityPeopleID,SocialSecurityPeopleName,ShouZhiType,LaiYuan,OperationType,Cost,Balance,CreateTime) 
values({DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random(Guid.NewGuid().GetHashCode()).Next(1000).ToString().PadLeft(3, '0')},{model.MemberID},'','','支出','余额','{ZhiNote}',{ZhiAccount},{memberAccount + accountNum - ZhiAccount},getdate()); ";

                    //更新未参保人的支付状态
                    DbHelper.ExecuteSqlCommand(sqlSocialSecurityPeople, null);

                    //计算出要进入个人账户的总额
                    decimal Account = 0;
                    orderDetailList.ForEach(n =>
                    {
                        Account += n.SocialSecurityAmount * n.SocialSecuritypayMonth + n.AccumulationFundAmount * n.AccumulationFundpayMonth;
                    });
                    //更新个人账户
                    string sqlMember = $"update Members set Account=ISNULL(Account,0)+{Account},Bucha=ISNULL(Bucha,0)+{Bucha} where MemberID={order.MemberID}";
                    int updateResult = DbHelper.ExecuteSqlCommand(sqlMember, null);
                    if (!(updateResult > 0)) throw new Exception("更新个人账户失败");

                    //更新记录
                    DbHelper.ExecuteSqlCommand(sqlAccountRecord, null);

                    //更新订单
                    string sqlUpdateOrder = $"update [Order] set Status = {(int)OrderEnum.Auditing},PaymentMethod='{model.PaymentMethod}',PayTime=getdate() where OrderCode={model.OrderCode}";
                    DbHelper.ExecuteSqlCommand(sqlUpdateOrder, null);

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "失败";
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return Redirect("/UserOrder/Index");
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string id)
        {
            //订单详情总计
            OrderDetailForMobile detail =_orderService.GetOrderDetail(CommonHelper.CurrentUser.MemberID, id);
            ViewBag.Detail = detail;

            List<OrderDetaisViewModel> list = userOderSv.GetOrderDetails(id);

            return View(list);
        }

    }
}