using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.Captcha;
using WYJK.HOME.Models;

namespace WYJK.HOME.Controllers
{
    public class UserController : Controller
    {
        private readonly IMemberService _memberService = new MemberService();

        // GET: User
        public ActionResult Login(LoginViewModel model)
        {
            if (this.Session["UserInfo"]!=null)
            {
                return Redirect("/User/Info");
            }
            else
            {
                return View("Login", model);
            }
        }

        public ActionResult Register(RegisterViewModel model)
        {

            return View(model);
        }




        public ActionResult Info()
        {
            Members m = (Members)this.Session["UserInfo"];

            string sql = $"SELECT * FROM Members where MemberID=" + m.MemberID; ;

            Members users = DbHelper.QuerySingle<Members>(sql);
            return View("Info");
        }

        [HttpPost]
        public ActionResult DoLogin(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.CheckCode.ToLower().Equals(Session["CheckCode"].ToString().ToLower()))
                {
                    string sql = $"SELECT * FROM Members where MemberName='{model.Email}' and Password='{model.Password}' ";

                    Members users = DbHelper.QuerySingle<Members>(sql);

                    if (users != null)
                    {
                        this.Session["UserInfo"] = users;
                        return Redirect("/User/Info");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "用户名或密码错误";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "验证码输入错误";
                }

            }
            return Login(model);
        }

        #region 显示验证码
        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
                    Session["CheckCode"] = captchaResult.Text;
                    return File(ms.ToArray(), "image/gif");
                }
            }
        }
        #endregion
    }
}