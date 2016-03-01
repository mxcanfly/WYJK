using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IServices;
using WYJK.Data.Services;
using WYJK.Entity;
using WYJK.Entity.Parameters;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Mvc
{
    public class SocialSecurityController : Controller
    {
        private readonly ISocialSecurityService _socialSecurityServic = new SocialSecurityService();

        /// <summary>
        /// 社保公积金预览
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SocialSecurityOverview(SocialSecurityParameter parameter)
        {
            

            //ViewData["HouseholdProperty"] = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));
            ViewData["SocialSecurityPeopleName"] = parameter.SocialSecurityPeopleName;
            ViewData["IdentityCard"] = parameter.IdentityCard;


            PagedResult<SocialSecurityShowModel> list = await _socialSecurityServic.GetSocialSecurityList(parameter);

            return View(list);
        }
    }
}