using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYJK.Entity;

namespace WYJK.Data.IService
{
    /// <summary>
    /// 公积金接口
    /// </summary>
    public interface IAccumulationFundService
    {
        /// <summary>
        /// 获取公积金列表(Admin)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<PagedResult<AccumulationFundShowModel>> GetAccumulationFundList(AccumulationFundParameter parameter);

        /// <summary>
        /// 获取某用户公积金列表
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        List<AccumulationFundShowModel> GetAccumulationFundList(int MemberID);

        /// <summary>
        /// 获取某签约企业下的公积金列表
        /// </summary>
        /// <param name="RelationEnterprise"></param>
        /// <returns></returns>
        List<AccumulationFundShowModel> GetAccumulationFundListByEnterpriseID(int RelationEnterprise);

        /// <summary>
        /// 修改公积金状态(Admin)
        /// </summary>
        /// <param name="SocialSecurityPeopleIDs"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool ModifyAccumulationFundStatus(int[] SocialSecurityPeopleIDs, int status);

        /// <summary>
        /// 修改公积金状态
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool ModifyAccumulationFundStatus(int SocialSecurityPeopleID, int status);
        /// <summary>
        /// 获取公积金详情(Admin)
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <returns></returns>
        AccumulationFund GetAccumulationFundDetail(int SocialSecurityPeopleID);

        /// <summary>
        /// 保存公积金号(Admin)
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <param name="AccumulationFundNo"></param>
        /// <returns></returns>
        bool SaveAccumulationFundNo(int SocialSecurityPeopleID, string AccumulationFundNo);
    }
}
