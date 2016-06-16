using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;
using WYJK.HOME.Models;
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class CalculatorController : Controller
    {
        RegionService regionSv = new RegionService();

        // GET: Calculator
        public ActionResult Calculator()
        {
            //获取省份
            ViewBag.Provinces = CommonHelper.EntityListToSelctList(regionSv.GetProvince(),"请选择省份");
            //获取户籍
            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "请选择户籍性质", Value = "0" });
            ViewBag.UserTypes = UserTypeList;

            return View();
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


    }
}