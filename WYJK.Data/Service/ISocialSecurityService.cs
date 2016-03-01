using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;
using WYJK.Entity.Parameters;

namespace WYJK.Data.IServices
{
    public interface ISocialSecurityService
    {
        /// <summary>
        /// 获取社保列表
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        Task<PagedResult<SocialSecurityShowModel>> GetSocialSecurityList(SocialSecurityParameter Parameter);

        /// <summary>
        /// 获取参保人
        /// </summary>
        /// <returns></returns>
        Task<List<InsuredPeople>> GetInsuredPeopleList();

        Task<bool> DeleteUninsuredPeople(int SocialSecurityPeopleID);
    }
}
