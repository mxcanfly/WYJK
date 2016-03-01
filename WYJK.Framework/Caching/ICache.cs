using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 将数据设置到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object Set(string key, object value);
        /// <summary>
        /// 将数据设置到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        object Set(string key, object value, TimeSpan duration);
        /// <summary>
        /// 将数据设置到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        object Set(string key, Func<object> func);
        /// <summary>
        /// 将数据设置到缓存中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        object Set(string key, Func<object> func, TimeSpan duration);
        /// <summary>
        /// 获取指定键名的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);
        /// <summary>
        /// 获取指定键名的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void Remove(string key);
    }
}
