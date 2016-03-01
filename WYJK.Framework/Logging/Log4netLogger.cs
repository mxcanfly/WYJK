using System;
using System.IO;
using System.Reflection;
using WYJK.Framework.Setting;
using log4net;

namespace WYJK.Framework.Logging
{
    /// <summary>
    /// 基于 Log4net 的接口实现
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _logger;

        static Log4NetLogger()
        {
            if (string.IsNullOrWhiteSpace(WebConfigurationManager.LogConfigurationFile) == false)
            {
                LoadConfigurationFile(WebConfigurationManager.LogConfigurationFile);
            }
        }
        /// <summary>
        /// 初始化对象的实例
        /// </summary>
        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        #region 从配置文件中加载 Log4net
        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static void LoadConfigurationFile()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static void LoadConfigurationFile(string path)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
        }

       
        #endregion

        public bool IsEnabled(LogLevel level)
        {
            return (level == LogLevel.Debug && _logger.IsDebugEnabled) ||
                   (level == LogLevel.Error && _logger.IsErrorEnabled) ||
                   (level == LogLevel.Fatal && _logger.IsFatalEnabled) ||
                   (level == LogLevel.Information && _logger.IsInfoEnabled) ||
                   (level == LogLevel.Warning && _logger.IsWarnEnabled);
        }
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            _logger.Debug(message, ex);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(string message, Exception ex)
        {
            _logger.Fatal(message, ex);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            _logger.Info(message, ex);
        }
        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            _logger.Warn(message,ex);
        }
    }
}
