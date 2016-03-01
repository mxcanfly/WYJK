using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Data.IServices;
using WYJK.Entity;

namespace WYJK.Data.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Dictionary<bool, string>> ForgetPassword(UserForgetPasswordModel model)
        {

            Dictionary<bool, string> dic = new Dictionary<bool, string>();
            DbParameter[] parameters = new DbParameter[]{
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = model.UserName },
                new SqlParameter("@UserPhone", SqlDbType.NVarChar, 50) { Value = model.UserPhone },
                new SqlParameter("@Password", SqlDbType.NVarChar, 100) { Value = model.HashPassword },
                new SqlParameter("@Flag", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                new SqlParameter("@Message", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output }
            };
            await DbHelper.ExecuteSqlCommandAsync("User_ForgetPassword", parameters, CommandType.StoredProcedure);

            dic.Add((bool)parameters[3].Value, parameters[4].Value.ToString());

            return dic;
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Dictionary<bool, string>> LoginUser(UserLoginModel model)
        {
            Dictionary<bool, string> dic = new Dictionary<bool, string>();
            DbParameter[] parameters = new DbParameter[]{
                new SqlParameter("@Account", SqlDbType.NVarChar, 50) { Value = model.Account },
                new SqlParameter("@Password", SqlDbType.NVarChar, 100) { Value = model.HashPassword },
                new SqlParameter("@Flag", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                new SqlParameter("@Message", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output }
            };
            await DbHelper.ExecuteSqlCommandAsync("User_Login", parameters, CommandType.StoredProcedure);

            dic.Add((bool)parameters[2].Value, parameters[3].Value.ToString());

            return dic;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Dictionary<bool,string>> RegisterUser(UserRegisterModel model)
        {
            Dictionary<bool, string> dic = new Dictionary<bool, string>();
            DbParameter[] parameters = new DbParameter[]{
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = model.UserName },
                new SqlParameter("@UserPhone", SqlDbType.NVarChar, 50) { Value = model.UserPhone },
                new SqlParameter("@Password", SqlDbType.NVarChar, 100) { Value = model.HashPassword },
                new SqlParameter("@InviteCode", SqlDbType.NVarChar, 50) { Value = model.InviteCode },
                new SqlParameter("@Flag", SqlDbType.Bit) { Direction = ParameterDirection.Output },
                new SqlParameter("@Message", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output }
            };
            await DbHelper.ExecuteSqlCommandAsync ("User_Register", parameters,CommandType.StoredProcedure);
            
            dic.Add((bool)parameters[4].Value, parameters[5].Value.ToString());

            return dic;
        }


    }
}
