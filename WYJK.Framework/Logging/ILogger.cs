using System;
using System.Threading.Tasks;

namespace WYJK.Framework.Logging
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 普通文本信息
        /// </summary>
        Information,
        /// <summary>
        /// 警告信息
        /// </summary>
        Warning,
        /// <summary>
        /// 错误信息
        /// </summary>
        Error,
        /// <summary>
        /// 失败信息
        /// </summary>
        Fatal
    }
    /// <summary>
    /// 日志统一接口
    /// </summary>
    public interface ILogger : IDependency
    {
        /// <summary>
        /// 获取一个值，判断是否启用了概级别的日志记录
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsEnabled(LogLevel level);

        void Debug(string message);
        void Debug(string message, Exception ex);

        void Info(string message);
        void Info(string message, Exception ex);

        void Warn(string message);
        void Warn(string message, Exception ex);

        void Error(string message);
        void Error(string message, Exception ex);
        void Fatal(string message);
        void Fatal(string message, Exception ex);
    }
}
