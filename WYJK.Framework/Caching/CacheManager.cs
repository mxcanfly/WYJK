using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework.Caching
{
    /// <summary>
    /// 缓存管理器
    /// </summary>
    public static class CacheManager
    {
        private static readonly ICache DefaultCache = new MemoryCacheHandle();
        private static readonly ConcurrentDictionary<string,ICache> CacheContainer = new ConcurrentDictionary<string, ICache>();
        /// <summary>
        /// 获取默认缓存容器
        /// </summary>
        public static ICache Cache { get; } = DefaultCache;
        /// <summary>
        /// 获取指定名称的缓存容器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ICache GetCache(string name)
        {
            return CacheContainer.GetOrAdd(name, k => new MemoryCacheHandle(name));
        }
    }
}
