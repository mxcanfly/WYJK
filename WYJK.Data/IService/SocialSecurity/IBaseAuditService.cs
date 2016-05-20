using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;

namespace WYJK.Data.IService
{
    /// <summary>
    /// 基数审核服务类
    /// </summary>
    public interface IBaseAuditService
    {
        /// <summary>
        /// 获取基数审核列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        PagedResult<BaseAuditList> GetBaseAuditList(BaseAuditParameter parameter);
        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="Status"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        bool BatchAudit(int[] IDs, string Status, int Type);
    }
}
