using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Dal
{
    public static class WebConfigurationManager
    {
        /// <summary>
        /// 获取配置节点项
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get { return System.Configuration.ConfigurationManager.AppSettings; }
        }
        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public static string DefaultConnectionString
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["JobConnectionString"].ConnectionString; }
        }

        /// <summary>
        /// 日志配置路径
        /// </summary>
        public static string LogConfigurationFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"App_Data", "log4net.config"); }
        }
        
    }
}
