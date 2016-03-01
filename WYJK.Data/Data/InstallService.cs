using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WYJK.Framework.Helpers;
using WYJK.Framework.Setting;

namespace WYJK.Data
{
    public class InstallService
    {
        #region 初始化分类
        /// <summary>
        /// 初始化商家一级分类
        /// </summary>
        /// <returns></returns>
        public bool InitOptions()
        {
            #region SQL代码

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "install.sql");
            string sql = File.ReadAllText(path);

            #endregion

            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            using (SqlTransaction transaction = connection.BeginTransaction())
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Transaction = transaction;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return false;
        }
        #endregion

        #region 初始化用户
        /// <summary>
        /// 初始化用户
        /// </summary>
        /// <returns></returns>
        public bool InitUser()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 20; i++)
            {
                string name = $"151652668{i.ToString("00")}";

                builder.Append("INSERT dbo.Users ( Account ,UserPassword ,UserMobile,UserImage,IDCard,UserState ,UserGrade,UserPoints , UserSource ,AddTime ,LastTime)" +
                    $"VALUES  ( '{name}' ,'{SecurityHelper.HashPassword("123456","123456")}' ,'{name}' ,'' , '<root/>' , 0 , 0 ,0 ,'' , GETDATE() , GETDATE());");
            }
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.DefaultConnectionString))
            using (SqlTransaction transaction = connection.BeginTransaction())
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = builder.ToString();
                command.Transaction = transaction;
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return false;
        } 
        #endregion

        public async Task<bool> InitMerchant()
        {
            //List<Users> users = await DbHelper.QueryAsync<Users>("SELECT * FROM dbo.Users");
            return false;
        }

    }
}
