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
using WYJK.HOME.Service;

namespace WYJK.HOME.Controllers
{
    public class UserInsuranceController : BaseController
    {
        ISocialSecurityService socialSv = new Data.ServiceImpl.SocialSecurityService();

        WYJK.HOME.Service.SocialSecurityService localSocialSv = new Service.SocialSecurityService();


        RegionService regionSv = new RegionService();

        #region 参保人列表

        /// <summary>
        /// 参保人列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Index(InsuranceQueryParamModel parameter)
        {

            Members m = (Members)this.Session["UserInfo"];

            if (m == null)
            {
                return Redirect("/User/Login");
            }


            string where = "";
            if (Convert.ToInt32(parameter.HouseholdProperty) > 0)
            {
                string hp = EnumExt.GetEnumCustomDescription((HouseholdPropertyEnum)(Int32.Parse(parameter.HouseholdProperty)));
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

        private void bulidHouseholdPropertyDropdown(string value)
        {
            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(HouseholdPropertyEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "户籍性质", Value = "" });

            ViewData["HouseholdProperty"] = new SelectList(UserTypeList, "Value", "Text");
        }

        #endregion


        [HttpGet]
        public ActionResult Add1()
        {
            bulidHouseholdPropertyDropdown("");
            return View();
        }

        /// <summary>
        /// 处理身份证上传
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadIDCard()
        {
            var files = Request.Files;
            HttpPostedFileBase file = files[0];
            string fielName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);

            string path = Server.MapPath(Path.Combine("/Uploads", fielName));

            file.SaveAs(path);
            
            return Json(fielName);
        }


        [HttpPost]
        public ActionResult Add1(InsuranceAdd1ViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImgUrls = model.IdentityCardPhoto.Substring(1).Split(',');

                SocialSecurityPeople socialPeople = new SocialSecurityPeople();
                socialPeople.MemberID = CommonHelper.CurrentUser.MemberID;
                socialPeople.IdentityCard = model.IdentityCard;
                socialPeople.SocialSecurityPeopleName = model.SocialSecurityPeopleName;
                socialPeople.IdentityCardPhoto = model.IdentityCardPhoto;
                socialPeople.HouseholdProperty = model.HouseholdProperty;

                int id = localSocialSv.AddSocialSecurityPeople(socialPeople);


                if (id > 0)
                {
                    socialPeople.SocialSecurityPeopleID = id;
                    Session["SocialSecurityPeople"] = socialPeople;
                    return RedirectToAction("Add2");
                }
                else
                {
                    bulidHouseholdPropertyDropdown(model.HouseholdProperty);
                    return View(model);
                }
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
            //获取省份
            ViewBag.Provinces = CommonHelper.EntityListToSelctList(regionSv.GetProvince(), "请选择省份");
            return View();
        }
        [HttpPost]
        public ActionResult Add2(InsuranceAdd2ViewModel model)
        {
            if (ModelState.IsValid)
            {
                //保存数据到数据库
                int id = AddLocalSocialSecurity(model);

                if (id > 0)
                {
                    Session["SocialSecurityPeopleID"] = null;

                    #warning todo 生成订单

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }

                
            }
            else
            {
                return View(model);
            }

        }

        /// <summary>
        /// 添加社保下一步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add2Next(InsuranceAdd2ViewModel model)
        {

            if (ModelState.IsValid)
            {
                
                int id = AddLocalSocialSecurity(model);

                if (id > 0)
                {
                    //保存数据到数据库
                    return RedirectToAction("Add3");
                }
                else
                {
                    return View("Add2",model);
                }
            }
            else
            {
                return View("Add2", model);
            }

        }


        public int AddLocalSocialSecurity(InsuranceAdd2ViewModel model)
        {
            SocialSecurity socialSecurity = new SocialSecurity();
            SocialSecurityPeople people = (SocialSecurityPeople)Session["SocialSecurityPeopleID"];
            socialSecurity.SocialSecurityPeopleID = people.SocialSecurityPeopleID;
            //socialSecurity.SocialSecurityPeopleID = 49;

            //socialSecurity.InsuranceArea = string.Format("{0}|{1}",Request["provinceText"], Request["city"]);
            socialSecurity.InsuranceArea = "山东省|青岛市|崂山区";
            socialSecurity.HouseholdProperty = people.HouseholdProperty;
            //socialSecurity.HouseholdProperty = "本市城镇";

            socialSecurity.SocialSecurityBase = model.SocialSecurityBase;
            socialSecurity.PayProportion = 0;
            socialSecurity.PayTime = model.PayTime;
            socialSecurity.PayMonthCount = model.PayMonthCount;
            socialSecurity.PayBeforeMonthCount = model.PayBeforeMonthCount;
            socialSecurity.BankPayMonth = model.BankPayMonth;
            socialSecurity.EnterprisePayMonth = model.EnterprisePayMonth;
            socialSecurity.Note = model.Note;
            socialSecurity.RelationEnterprise = 0;
            //保存数据到数据库
            int id = socialSv.AddSocialSecurity(socialSecurity);

            return id;
        }


        [HttpGet]
        public ActionResult Add3()
        {
            //获取省份
            ViewBag.Provinces = CommonHelper.EntityListToSelctList(regionSv.GetProvince(), "请选择省份");
            return View();
        }

        [HttpPost]
        public ActionResult Confirm(InsuranceAdd3ViewModel model)
        {
            if (ModelState.IsValid)
            {
                SocialSecurityPeople people = (SocialSecurityPeople)Session["SocialSecurityPeopleID"];
                
                AccumulationFund accumulationFund = new AccumulationFund();

                //accumulationFund.SocialSecurityPeopleID = people.SocialSecurityPeopleID;
                accumulationFund.SocialSecurityPeopleID = 49;
                //accumulationFund.AccumulationFundArea = string.Format("{0}|{1}", Request["provinceText"], Request["city"]);
                accumulationFund.AccumulationFundArea = "山东省|青岛市|崂山区";
                accumulationFund.AccumulationFundBase = model.AccumulationFundBase;
                accumulationFund.PayProportion = 0;
                accumulationFund.PayTime = model.PayTime;
                accumulationFund.PayMonthCount = model.PayMonthCount;
                accumulationFund.PayBeforeMonthCount = model.PayBeforeMonthCount;
                accumulationFund.Note = model.Note;
                accumulationFund.RelationEnterprise = 0;

                int id = socialSv.AddAccumulationFund(accumulationFund);

                if (id > 0)
                {
                    #warning todo 生成订单
                    //跳转到确认页面
                    return null;
                }
                else
                {
                    return View("Add3", model);
                }
            }
            else
            {
                return View("Add3", model);
            }

        }

        /// <summary>
        /// 社保基数变更
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeSB()
        {
            return View();
        }

        /// <summary>
        /// 公积金基数变更
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeFund()
        {
            return View();
        }

        /// <summary>
        /// 社保基数变更历史
        /// </summary>
        /// <returns></returns>
        public ActionResult RecSB()
        {
            return View();
        }

        /// <summary>
        /// 公积金基数变更历史
        /// </summary>
        /// <returns></returns>
        public ActionResult RecFund()
        {
            return View();
        }

    }
}