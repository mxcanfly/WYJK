using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IService;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Mvc
{
    /// <summary>
    /// 客户管理
    /// </summary>
    [Authorize]
    public class CustomerServiceController : Controller
    {
        private readonly ICustomerService _customerService = new CustomerService();
        private readonly IOrderService _orderService = new OrderService();
        private readonly ISocialSecurityService _socialSecurityService = new SocialSecurityService();
        private readonly IAccumulationFundService _accumulationFundService = new AccumulationFundService();
        private readonly IMemberService _memberService = new MemberService();
        /// <summary>
        /// 获取客户管理列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustomerServiceList(CustomerServiceParameter parameter)
        {
            PagedResult<CustomerServiceViewModel> customerServiceList = _customerService.GetCustomerServiceList(parameter);


            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(UserTypeEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "全部", Value = "" });

            ViewData["UserType"] = new SelectList(UserTypeList, "Value", "Text");

            ViewBag.memberList = _memberService.GetMembersList();

            return View(customerServiceList);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMemberList1(string UserType)
        {
            List<Members> memberList = _memberService.GetMembersList();
            var list = memberList.Where(n => UserType == string.Empty ? true : n.UserType == UserType)
                .Select(item => new { MemberID = item.MemberID, MemberName = item.MemberName });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量通过  注： 参保人客户服务状态变成已审核，并判断对应的订单财务是否已经审核通过
        /// 遍历参保人对应的订单，只要有一个是已付款或已完成（如果没有，则需提示），则客服审核状态变成审核通过，如果是订单是已完成，则参保人状态变成待办
        /// </summary>
        /// <param name="SocialSecurityPeopleIDs"></param>
        /// <returns></returns>
        public JsonResult BatchComplete(int[] SocialSecurityPeopleIDs)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    foreach (var socialSecurityPeopleID in SocialSecurityPeopleIDs)
                    {
                        List<Order> orderList = _orderService.GetOrderList(socialSecurityPeopleID);
                        bool flag = false;
                        foreach (var order in orderList)
                        {
                            if (Convert.ToInt32(order.Status) == (int)OrderEnum.Auditing || Convert.ToInt32(order.Status) == (int)OrderEnum.completed)
                            {
                                _customerService.ModifyCustomerServiceStatus(new int[] { socialSecurityPeopleID }, (int)CustomerServiceAuditEnum.Pass);
                                flag = true;
                            }

                            if (Convert.ToInt32(order.Status) == (int)OrderEnum.completed)
                            {
                                //更新社保和公积金为待办状态
                                _socialSecurityService.ModifySocialStatus(new int[] { socialSecurityPeopleID }, (int)SocialSecurityStatusEnum.WaitingHandle);
                                _accumulationFundService.ModifyAccumulationFundStatus(new int[] { socialSecurityPeopleID }, (int)SocialSecurityStatusEnum.WaitingHandle);
                            }
                        }
                        if (!flag) throw new Exception("所选参保人不符合申请条件");
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = ex.Message });
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return Json(new { status = true, message = "申请通过" });
        }

        /// <summary>
        /// 根据参保人ID获取详情
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public ActionResult GetSocialSecurityPeopleDetail(int SocialSecurityPeopleID, int MemberID, int Type)
        {

            SocialSecurityPeople socialSecurityPeople = _socialSecurityService.GetSocialSecurityPeopleForAdmin(SocialSecurityPeopleID);

            if (socialSecurityPeople.IsPaySocialSecurity)
            {
                socialSecurityPeople.socialSecurity = _socialSecurityService.GetSocialSecurityDetail(SocialSecurityPeopleID);
            }
            if (socialSecurityPeople.IsPayAccumulationFund)
            {
                socialSecurityPeople.accumulationFund = _accumulationFundService.GetAccumulationFundDetail(SocialSecurityPeopleID);
            }

            //获取会员信息
            ViewData["member"] = _memberService.GetMemberInfoForAdmin(MemberID);

            //获取账户列表
            ViewData["accountRecordList"] = _memberService.GetAccountRecordList(MemberID).OrderByDescending(n => n.CreateTime).ToList();

            #region 户口性质
            List<SelectListItem> list = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));

            int householdType = 0;
            foreach (var item in list)
            {
                if (item.Text == socialSecurityPeople.HouseholdProperty)
                {
                    householdType = Convert.ToInt32(item.Value);
                    break;
                }
            }

            ViewData["HouseholdProperty"] = new SelectList(list, "value", "text", householdType);
            #endregion

            return View(socialSecurityPeople);
        }

        /// <summary>
        /// 根据参保人ID获取订单列表
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <returns></returns>
        public ActionResult GetOrderList(int SocialSecurityPeopleID)
        {
            List<Order> orderList = _orderService.GetOrderList(SocialSecurityPeopleID);
            return View(orderList);
        }
    }
}
