using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYJK.Entity
{
    /// <summary>
    /// 省市区信息表
    /// </summary>
    public class Region
    {

        /// <summary>
        /// 主键
        /// </summary>
        public int RegionId
        {
            get; set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName
        {
            get; set;
        }
        /// <summary>
        /// 区域编号
        /// </summary>
        public string RegionCode
        {
            get; set;
        }
        /// <summary>
        /// 父编号
        /// </summary>
        public string ParentCode
        {
            get; set;
        }

    }
}
