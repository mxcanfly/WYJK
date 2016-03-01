using System;

namespace WYJK.Framework.Logging
{
    /// <summary>
    /// 公共日志记录对象
    /// </summary>
    public static class Logger
    {
        private static readonly ILogger _logger = new Log4NetLogger();
        public static void Debug(string message)
        {
            _logger.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            _logger.Debug(message, ex);
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public static void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public static void Fatal(string message, Exception ex)
        {
            _logger.Fatal(message, ex);
        }

        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Info(string message, Exception ex)
        {
            _logger.Info(message, ex);
        }
        public static void Warn(string message)
        {
            _logger.Warn(message);
        }

        public static void Warn(string message, Exception ex)
        {
            _logger.Warn(message, ex);
        }
    }
}
