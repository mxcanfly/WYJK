using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;

namespace WYJK.Data.IService
{
    /// <summary>
    /// 参数设置服务类
    /// </summary>
    public interface IParameterSettingService
    {
        /// <summary>
        /// 根据状态获取费用实体
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        CostParameterSetting GetCostParameter(int status);

        /// <summary>
        /// 添加费用参数
        /// </summary>
        /// <param name="costParameterSetting"></param>
        /// <returns></returns>
        bool AddCostParameter(CostParameterSetting costParameterSetting);

        /// <summary>
        /// 修改费用参数
        /// </summary>
        /// <param name="costParameterSetting"></param>
        /// <returns></returns>
        bool UpdateCostParameter(CostParameterSetting costParameterSetting);


    }
}
