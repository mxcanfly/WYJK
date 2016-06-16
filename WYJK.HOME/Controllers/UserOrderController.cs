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
        UserMemberService memberSv = new UserMemberService();

        // GET: UserOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            return View();
        }

        public ActionResult Pay()
        {
            return View();
        }

    }
}