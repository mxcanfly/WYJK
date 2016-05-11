using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace Jobs.Web
{
    /// <summary>
    /// WebApi请求头扩展方法
    /// </summary>
    internal static class HttpHeadersExtentions
    {
        /// <summary>
        /// 从请求头中获取第一个匹配值
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static string GetFirstOfDefault(this HttpHeaders headers,string name)
        {
            IEnumerable<string> values;
            if (headers.TryGetValues(name, out values))
            {
                return values.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 获取第一个匹配项并转化为指定类型
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="headers"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static TValue GetFirstOfDefault<TValue>(this HttpHeaders headers, string name)
        {
            try
            {
                IEnumerable<string> values;
                if (headers.TryGetValues(name, out values))
                {
                    string value = values.FirstOrDefault();

                    return (TValue)Convert.ChangeType(value, typeof(TValue));
                }
            }
            catch
            {
                // ignored
            }
            return default(TValue);
        }
    }
}