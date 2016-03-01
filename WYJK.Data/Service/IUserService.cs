using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;

namespace WYJK.Data.IServices
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Dictionary<bool, string>> RegisterUser(UserRegisterModel model);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Dictionary<bool, string>> LoginUser(UserLoginModel model);

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Dictionary<bool, string>> ForgetPassword(UserForgetPasswordModel model);

    }
}
