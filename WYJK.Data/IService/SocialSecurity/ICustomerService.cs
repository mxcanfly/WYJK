using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;

namespace WYJK.Data.IService
{
    /// <summary>
    /// 客服接口
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// 获取客户服务列表
        /// </summary>
        /// <returns></returns>
        PagedResult<CustomerServiceViewModel> GetCustomerServiceList(CustomerServiceParameter parameter);

        /// <summary>
        /// 修改客户服务状态
        /// </summary>
        /// <param name="SocialSecurityPeopleIDs"></param>
        /// <returns></returns>
        bool ModifyCustomerServiceStatus(int[] SocialSecurityPeopleIDs,int status);
    }
}
