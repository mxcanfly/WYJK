using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Framework.Setting
{
    /// <summary>
    /// 百度配置
    /// </summary>
    public class BaiduSetting
    {
        public BaiduSetting()
        {
            
        }
        public BaiduSetting(string ak, string sk)
        {
            AppKey = sk;
            SecurityKey = sk;
        }
        /// <summary>
        /// 应用AK
        /// </summary>
        public string AppKey { set; get; }
        /// <summary>
        /// 该应用对应的SK
        /// </summary>
        public string SecurityKey { set; get; }
    }
}
