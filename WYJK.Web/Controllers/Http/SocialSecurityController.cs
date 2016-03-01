using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IServices;
using WYJK.Data.Services;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Http
{
    public class SocialSecurityController : ApiController
    {
        private readonly ISocialSecurityService _socialSecurityService = new SocialSecurityService();
        /// <summary>
        /// 获取户口性质
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public JsonResult<List<HouseholdProperty>> GetHouseholdPropertyList()
        {
            List<HouseholdProperty> HouseholdPropertyList = HouseholdPropertyClass.GetList(typeof(HouseholdPropertyEnum));

            return new JsonResult<List<HouseholdProperty>>
            {
                status = true,
                Message = "获取成功",
                Data = HouseholdPropertyList
            };
        }

        /// <summary>
        /// 删除未参保人
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<JsonResult<dynamic>> DeleteUninsuredPeople(int SocialSecurityPeopleID)
        {
            bool flag = await _socialSecurityService.DeleteUninsuredPeople(SocialSecurityPeopleID);

            return new JsonResult<dynamic>
            {
                status = flag,
                Message = flag ? "删除成功" : "删除失败"
            };

        }


        /// <summary>
        /// 获取社保基数范围
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public JsonResult<dynamic> GetSocialSecurityBase()
        {
            return new JsonResult<dynamic>
            {
                status = true,
                Message = "获取成功",
                Data = new { min = 1000, max = 2000 }
            };
        }

        /// <summary>
        /// 获取参保人列表
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public async Task<JsonResult<List<InsuredPeople>>> GetInsuredPeopleList()
        {
            List<InsuredPeople> list = await _socialSecurityService.GetInsuredPeopleList();
            return new JsonResult<List<InsuredPeople>>
            {
                status = true,
                Message = "获取成功",
                Data = list
            };
        }

        
    }
}