using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WYJK.Web.Models;
using System.IO;
using System.Drawing.Imaging;
using WYJK.Framework.Captcha;
using WYJK.Framework.Helpers;
using WYJK.Entity;
using WYJK.Data;
using Newtonsoft.Json;
using System.Web.Security;
using System.Collections.Generic;

namespace WYJK.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            string url = Request.QueryString["ReturnUrl"];
            return View();
        }

        #region 显示验证码
        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public FileContentResult Captcha()
        {
            CaptchaOptions options = new CaptchaOptions
            {
                GaussianDeviation = 0.4,
                Height = 35,
                Background = NoiseLevel.Low,
                Line = NoiseLevel.Low
            };
            using (ICapatcha capatch = new FluentCaptcha())
            {
                capatch.Options = options;
                CaptchaResult captchaResult = capatch.DrawBackgroud().DrawLine().DrawText().Atomized().DrawBroder().DrawImage();
                using (captchaResult)
                {
                    MemoryStream ms = new MemoryStream();
                    captchaResult.Bitmap.Save(ms, ImageFormat.Gif);
                    Session["Captcha"] = captchaResult.Text;
                    return File(ms.ToArray(), "image/gif");
                }
            }
        }
        #endregion

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.VerificationCode.Equals(Session["Captcha"] + "", StringComparison.OrdinalIgnoreCase) == false)
                {
                    ViewBag.ErrorMessage = "验证码不正确";
                    return View(model);
                }
                string password = SecurityHelper.HashPassword(model.Password, model.Password);
                string sql = $"SELECT * FROM Employee where EmployeeName='{model.EmployeeName}' and Password='{password}'";

                EmployeeViewModel employee = await DbHelper.QuerySingleAsync<EmployeeViewModel>(sql);
                if (employee != null)
                {
                    string data = JsonConvert.SerializeObject(employee);
                    SetAuthCookie(employee.EmployeeName, data);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorMessage = "用户名或密码错误";
            }

            return View();
        }


        #region 私有方法
        /// <summary>
        /// 设置授权Cookie
        /// </summary>
        /// <param name="account"></param>
        /// <param name="data"></param>
        private void SetAuthCookie(string account, string data)
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, account, DateTime.Now, DateTime.Now.AddHours(12), true, data);
            HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(Ticket));//加密身份信息，保存至Cookie
            Response.Cookies.Add(Cookie);



            //int expiration = 1440;
            //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, account, DateTime.Now, DateTime.Now.AddDays(1), true, data);
            //string cookieValue = FormsAuthentication.Encrypt(ticket);
            //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            //{
            //    HttpOnly = true,
            //    Secure = FormsAuthentication.RequireSSL,
            //    Domain = FormsAuthentication.CookieDomain,
            //    Path = FormsAuthentication.FormsCookiePath
            //};
            //if (expiration > 0)
            //{
            //    cookie.Expires = DateTime.Now.AddMinutes(expiration);
            //}

            //Response.Cookies.Remove(cookie.Name);
            //Response.Cookies.Add(cookie);
        }
        #endregion

        #region 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login");
        }
        #endregion

    }
}