using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WYJK.Data;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.Captcha;
using WYJK.Framework.EnumHelper;
using WYJK.HOME.Models;

namespace WYJK.HOME.Controllers
{
    public class UserInsuranceController : BaseController
    {

        public ActionResult Index(InsuranceQueryParamModel parameter)
        {


            Members m = (Members)this.Session["UserInfo"];

            if (m == null)
            {
                return Redirect("/User/Login");
            }


            String where = "";
            if (Convert.ToInt32(parameter.HouseholdProperty) > 0)
            {
                String hp = EnumExt.GetEnumCustomDescription((HouseholdPropertyEnum)(Int32.Parse(parameter.HouseholdProperty)));
                where += $@"and sp.HouseholdProperty='{hp}'";
            }
            if (parameter.InsuranceArea != null)
            {

                where += $@"and ss.InsuranceArea='{parameter.InsuranceArea}'";
            }
            if (parameter.SocialSecurityPeopleName != null)
            {
                where += $@"and sp.SocialSecurityPeopleName='{parameter.SocialSecurityPeopleName}'";
            }
            string sqlstr = $@" 
                SELECT 
	                sp.SocialSecurityPeopleID,sp.SocialSecurityPeopleName,sp.IdentityCard,
	                sp.HouseholdProperty,convert(varchar(10),ss.PayTime,111) PayTime,convert(varchar(10),ss.StopDate,111) StopDate,ss.SocialSecurityBase,ss.Status SocialSecurityStatus,
	                cast(round((ss.SocialSecurityBase*ss.PayProportion)/100,2) as numeric(12,2)) SocialSecurityAmount,
	                af.AccumulationFundBase,	
	                cast(round((af.AccumulationFundBase*af.PayProportion)/100,2) as numeric(12,2)) AccumulationFundAmount,	
	                af.Status AccumulationFundStatus	
                from SocialSecurityPeople sp
                left join SocialSecurity  ss on sp.SocialSecurityPeopleID=ss.SocialSecurityPeopleID
                left join AccumulationFund af on sp.SocialSecurityPeopleID=af.SocialSecurityPeopleID
            where sp.MemberID = {m.MemberID} {where} order by sp.SocialSecurityPeopleID desc  ";



            List<InsuranceListViewModel> SocialSecurityPeopleList = DbHelper.Query<InsuranceListViewModel>(sqlstr);

            var c = SocialSecurityPeopleList.Skip(parameter.SkipCount - 1).Take(parameter.PageSize);


            PagedResult<InsuranceListViewModel> page = new PagedResult<InsuranceListViewModel>
            {
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize,
                TotalItemCount = SocialSecurityPeopleList.Count,
                Items = c
            };

            bulidHouseholdPropertyDropdown(parameter.HouseholdProperty);

            return View(page);
        }

        private void bulidHouseholdPropertyDropdown(String value)
        {
            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "请选择", Value = "" });

            ViewData["HouseholdProperty"] = new SelectList(UserTypeList, "Value", "Text", value);
        }
        [HttpGet]
        public ActionResult Add1()
        {
            String HouseholdProperty = "";
            if (Session["InsuranceAdd1ViewModel"] != null)
            {
                HouseholdProperty = ((InsuranceAdd1ViewModel)Session["InsuranceAdd1ViewModel"]).HouseholdProperty;
            }
            bulidHouseholdPropertyDropdown(HouseholdProperty);
            return View(Session["InsuranceAdd1ViewModel"]);
        }



        [HttpPost]
        public ActionResult Add1(InsuranceAdd1ViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["InsuranceAdd1ViewModel"] = model;
                return Redirect("InsuranceAdd2");
            }
            else
            {
                bulidHouseholdPropertyDropdown(model.HouseholdProperty);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Add2()
        {
            return View(Session["InsuranceAdd2ViewModel"]);
        }
        [HttpPost]
        public ActionResult Add2(InsuranceAdd2ViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["InsuranceAdd2ViewModel"] = model;
                return Redirect("InsuranceAdd3");
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult Add3()
        {
            return View(Session["InsuranceAdd3ViewModel"]);
        }

        [HttpPost]
        public ActionResult Confirm(InsuranceAdd3ViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["InsuranceAdd3ViewModel"] = model;
                return null;
            }
            else
            {
                return View("InsuranceAdd3", model);
            }

        }




       
    }
}