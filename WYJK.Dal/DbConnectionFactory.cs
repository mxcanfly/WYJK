using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobs.Framework.Setting;

namespace Jobs.Data
{
    /// <summary>
    /// 数据库工厂
    /// </summary>
    public static class DbConnectionFactory
    {
        /// <summary>
        /// 创建一个数据库连接对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection Creator(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = WebConfigurationManager.DefaultConnectionString;
            }
            return new SqlConnection(connectionString);
        }
    }
}
