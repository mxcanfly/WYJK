using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUYK.Common.Helpers
{
    /// <summary>
    /// 硬盘容量单位转换
    /// </summary>
    public static class DriveConverter
    {
        /// <summary>
        /// 指定的容量转换为更大容量单位
        /// </summary>
        /// <param name="b">需要转换的容量</param>
        /// <param name="iteration">转换次数</param>
        /// <returns></returns>
        public static decimal ConvertBytes(string b, int iteration)
        {
            long iter = 1;
            for (int i = 0; i < iteration; i++)
            {
                iter *= 1024;
            }
            return Math.Round((Convert.ToDecimal(b)) / Convert.ToDecimal(iter), 2, MidpointRounding.AwayFromZero);
        }
}
}
