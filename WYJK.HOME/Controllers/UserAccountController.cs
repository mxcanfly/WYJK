using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WYJK.HOME.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult MyAccount()
        {
            return View();
        }

        public ActionResult Charge()
        {

            return View();
        }

        public ActionResult WithDraw()
        {


            return View();
        }
    }
}