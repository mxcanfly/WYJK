using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework
{
    /// <summary>
    /// 招聘信息异常基础类
    /// </summary>
    public class JobException : Exception
    {
        /// <summary>
        /// 初始化对象的实例
        /// </summary>
        public JobException() :base(){ }
        /// <summary>
        /// 初始化对象的实例
        /// </summary>
        /// <param name="message"></param>
        public JobException(string message) : base(message) { }

        /// <summary>
        /// 初始化对象的实例
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public JobException(string message,Exception ex) : base(message, ex) { }

        public JobException(int errorCode, string message)
        {
            ErrorCode = errorCode;
            ErrorMessage = message;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { set; get; }
        /// <summary>
        /// 错误描述信息
        /// </summary>
        public string ErrorMessage { set; get; }
    }
}
