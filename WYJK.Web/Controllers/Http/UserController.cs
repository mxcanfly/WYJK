using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WYJK.Data.IServices;
using WYJK.Data.Services;
using WYJK.Entity;

namespace WYJK.Web.Controllers.Http
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class UserController : ApiController
    {
        private readonly IUserService _userService = new UserService();

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<UserRegisterModel>> RegisterUser(UserRegisterModel entity)
        {
            Dictionary<bool, string> dic = await _userService.RegisterUser(entity);

            return new JsonResult<UserRegisterModel>
            {
                status = dic.First().Key,
                Message = dic.First().Value,
                Data = entity
            };
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<UserLoginModel>> LoginUser(UserLoginModel entity)
        {
            Dictionary<bool, string> dic = await _userService.LoginUser(entity);

            return new JsonResult<UserLoginModel>
            {
                status = dic.First().Key,
                Message = dic.First().Value,
                Data = entity
            };
        }

        /// <summary>
        /// 用户密码找回
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<JsonResult<UserForgetPasswordModel>> ForgetPassword(UserForgetPasswordModel entity)
        {
            Dictionary<bool, string> dic = await _userService.ForgetPassword(entity);

            return new JsonResult<UserForgetPasswordModel>
            {
                status = dic.First().Key,
                Message = dic.First().Value,
                Data = entity
            };
        }

    }
}