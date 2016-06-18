using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IServices;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class CalculatorController : Controller
    {
        RegionService regionSv = new RegionService();

        ISocialSecurityService _socialSecurity = new WYJK.Data.ServiceImpl.SocialSecurityService();

        // GET: Calculator
        public ActionResult Calculator()
        {
            //获取省份
            ViewBag.Provinces = CommonHelper.EntityListToSelctList(regionSv.GetProvince(),"请选择省份");
            //获取户籍
            ViewBag.UserTypes = CommonHelper.SelectListType(typeof(HouseholdPropertyEnum), "请选择户籍性质");
            return View();
        }

        public ActionResult CalculatorResult(string InsuranceArea, string HouseholdProperty, decimal SocialSecurityBase, decimal AccountRecordBase)
        {
            SocialSecurityCalculation cal = _socialSecurity.GetSocialSecurityCalculationResult("山东省|青岛市|崂山区", HouseholdProperty, SocialSecurityBase, AccountRecordBase);

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict["SocialSecuritAmount"] = cal.SocialSecuritAmount;
            dict["AccumulationFundAmount"] = cal.AccumulationFundAmount;
            dict["TotalAmount"] = cal.TotalAmount;

            return Json(dict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CitysByProvince(int id)
        {
            return Json(regionSv.GetCitys(id), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获取社保公积金基数
        /// </summary>
        /// <param name="area"></param>
        /// <param name="householdProperty"></param>
        /// <returns></returns>
        public ActionResult GetSocialSecurityBase(string area, string householdProperty)
        {
            EnterpriseSocialSecurity model = _socialSecurity.GetDefaultEnterpriseSocialSecurityByArea(area, householdProperty);

            decimal minBase = Math.Round(model.SocialAvgSalary * (model.MinSocial / 100));
            decimal maxBase = Math.Round(model.SocialAvgSalary * (model.MaxSocial / 100));

            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            dict["MinBase"] = minBase;
            dict["MaxBase"] = maxBase;
            dict["AFMinBase"] = model.MinAccumulationFund;
            dict["AFMaxBase"] = model.MaxAccumulationFund;


            return Json(dict,JsonRequestBehavior.AllowGet);
        }


    }
}