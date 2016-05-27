using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.HOME.Models;

namespace WYJK.HOME.Controllers
{
    public class UserController : Controller
    {
        private readonly IMemberService _memberService = new MemberService();

        // GET: User
        public ActionResult Login()
        {
            return View("Login");
        }


        public ActionResult Info()
        {
            return View("Info");
        }


        public ActionResult DoLogin(LoginViewModels model)
        {
            if (ModelState.IsValid)
            {
                string sql = $"SELECT * FROM Members where MemberName='{model.Email}' and Password='{model.Password}' " ;

                Members users = DbHelper.QuerySingle<Members>(sql);

                if (users!=null)
                {
                    return Info();
                }
                
            }
            ViewBag.ErrorMessage = "用户名或密码输入错误";
            return Login();
        }
    }
}