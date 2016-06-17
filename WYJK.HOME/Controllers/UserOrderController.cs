using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class UserOrderController : BaseFilterController
    {
        SocialSecurityService sss = new SocialSecurityService();

        // GET: UserOrder
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 订单确认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            SocialSecurityPeopleViewModel ssp = sss.SocialSecurityDetail(id.Value);

            return View(ssp);
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Craete(SocialSecurityPeopleViewModel model)
        {

            return View();
        }

        public ActionResult Pay()
        {
            return View();
        }

    }
}