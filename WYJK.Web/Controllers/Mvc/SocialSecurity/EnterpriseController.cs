using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IService;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Mvc
{
    /// <summary>
    /// 签约企业
    /// </summary>
    [Authorize]
    public class EnterpriseController : Controller
    {
        private readonly ISocialSecurityService _socialSecurityService = new SocialSecurityService();
        private readonly IEnterpriseService _enterpriseService = new EnterpriseService();
        private readonly IAccumulationFundService _accumulationFundService = new AccumulationFundService();
        /// <summary>
        /// 获取参保企业列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult GetEnterpriseList(EnterpriseSocialSecurityParameter parameter)
        {
            PagedResult<EnterpriseSocialSecurity> EnterpriseSocialSecurityList = _enterpriseService.GetEnterpriseList(parameter);
            return View(EnterpriseSocialSecurityList);
        }

        /// <summary>
        /// 新增签约企业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddEnterprise()
        {
            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "请选择", Value = "" });

            ViewData["HouseholdProperty"] = new SelectList(UserTypeList, "Value", "Text");

            EnterpriseSocialSecurity model = new EnterpriseSocialSecurity();

            return View(model);
        }

        /// <summary>
        /// 保存企业
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEnterprise(EnterpriseSocialSecurity model)
        {
            //已存在判断
            bool isExists = _enterpriseService.IsExistsEnterprise(model.EnterpriseName);
            if (isExists)
            {
                ViewBag.Message = "企业名称已存在";
                return AddEnterprise();
            }

            //ProvinceCode CityCode CountyCode
            string ProvinceName = string.Empty;
            string CityName = string.Empty;
            string CountyName = string.Empty;

            #region 将编码变成名称
            string sqlstr = "select * from Region where RegionCode = '{0}'";
            ProvinceName = DbHelper.QuerySingle<Region>(string.Format(sqlstr, model.ProvinceCode)).RegionName;
            CityName = DbHelper.QuerySingle<Region>(string.Format(sqlstr, model.CityCode)).RegionName;
            CountyName = DbHelper.QuerySingle<Region>(string.Format(sqlstr, model.CountyCode)).RegionName;
            #endregion

            model.EnterpriseArea = ProvinceName + "|" + CityName + "|" + CountyName;

            //更新其他签约企业  注：满足省份|城市和户口类型  默认的只有一个
            if (model.IsDefault)
            {
                _enterpriseService.UpdateEnterpriseDefault(ProvinceName + "|" + CityName, Convert.ToInt32(model.HouseholdProperty));
            }

            //添加
            bool flag = _enterpriseService.AddEnterprise(model);

            #region 记录日志
            LogService.WriteLogInfo(new Log { UserName = HttpContext.User.Identity.Name, Contents = string.Format("新增签约企业:{0}", model.EnterpriseName) });
            #endregion

            TempData["Message"] = flag ? "保存成功" : "保存失败";
            return RedirectToAction("GetEnterpriseList");
        }


        /// <summary>
        /// 批量删除企业
        /// </summary>
        /// <param name="EnterpriseIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchDelete(int[] EnterpriseIDs)
        {
            bool flag = _enterpriseService.BatchDeleteEnterprise(EnterpriseIDs);

            #region 记录日志
            string names = _enterpriseService.GetEnterpriseNames(EnterpriseIDs);
            LogService.WriteLogInfo(new Log { UserName = HttpContext.User.Identity.Name, Contents = string.Format("删除签约企业:{0}", names) });
            #endregion

            return Json(new { status = flag, message = flag ? "删除成功" : "删除失败" });
        }

        /// <summary>
        /// 获取企业详情
        /// </summary>
        /// <param name="EnterpriseID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEnterpriseDetail(int EnterpriseID)
        {
            EnterpriseSocialSecurity model = _enterpriseService.GetEnterpriseSocialSecurity(EnterpriseID);
            return View(model);
        }


        /// <summary>
        /// 编辑企业
        /// </summary>
        /// <param name="EnterpriseID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditEnterprise(int EnterpriseID)
        {

            EnterpriseSocialSecurity model = _enterpriseService.GetEnterpriseSocialSecurity(EnterpriseID);

            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "请选择", Value = "" });

            ViewData["HouseholdProperty1"] = UserTypeList; //new SelectList(UserTypeList, "Value", "Text");

            return View(model);
        }

        /// <summary>
        /// 保存编辑企业
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEnterprise(EnterpriseSocialSecurity model)
        {            //已存在判断
            bool isExists = _enterpriseService.IsExistsEnterprise(model.EnterpriseName, model.EnterpriseID);
            if (isExists)
            {
                ViewBag.Message = "企业名称已存在";
                return EditEnterprise(model.EnterpriseID);
            }

            //ProvinceCode CityCode CountyCode
            string ProvinceName = string.Empty;
            string CityName = string.Empty;
            string CountyName = string.Empty;

            #region 将编码变成名称
            string sqlstr = "select * from Region where RegionCode = '{0}'";
            ProvinceName = DbHelper.QuerySingle<Region>(string.Format(sqlstr, model.ProvinceCode)).RegionName;
            CityName = DbHelper.QuerySingle<Region>(string.Format(sqlstr, model.CityCode)).RegionName;
            CountyName = DbHelper.QuerySingle<Region>(string.Format(sqlstr, model.CountyCode)).RegionName;
            #endregion

            model.EnterpriseArea = ProvinceName + "|" + CityName + "|" + CountyName;

            //更新其他签约企业  注：满足省份|城市和户口类型  默认的只有一个
            if (model.IsDefault)
            {
                _enterpriseService.UpdateEnterpriseDefault(ProvinceName + "|" + CityName, Convert.ToInt32(model.HouseholdProperty));
            }

            bool flag = _enterpriseService.UpdateEnterprise(model);

            #region 记录日志
            LogService.WriteLogInfo(new Log { UserName = HttpContext.User.Identity.Name, Contents = string.Format("修改签约企业:{0}", model.EnterpriseName) });
            #endregion

            TempData["Message"] = flag ? "保存成功" : "保存失败";
            return RedirectToAction("GetEnterpriseList");
        }

        /// <summary>
        /// 根据地区和户籍性质获取默认企业
        /// </summary>
        /// <param name="area"></param>
        /// <param name="HouseholdProperty"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDefaultEnterpriseSocialSecurityByArea(string area, string HouseholdProperty)
        {
            EnterpriseSocialSecurity model = _socialSecurityService.GetDefaultEnterpriseSocialSecurityByArea(area, HouseholdProperty);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取社保列表
        /// </summary>
        /// <param name="RelationEnterprise"></param>
        /// <returns></returns>
        public ActionResult GetSocialSecurityList(int RelationEnterprise) {

            ViewData["SocialSecurityList"] = _socialSecurityService.GetSocialSecurityListByEnterpriseID(RelationEnterprise);
            ViewData["AccumulationFundList"] = _accumulationFundService.GetAccumulationFundListByEnterpriseID(RelationEnterprise);
            return View();
        }
    }
}