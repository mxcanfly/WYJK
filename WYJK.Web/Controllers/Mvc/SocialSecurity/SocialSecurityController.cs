﻿using WYJK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WYJK.Data.IServices;
using WYJK.Data.ServiceImpl;
using WYJK.Entity;
using WYJK.Framework.EnumHelper;

namespace WYJK.Web.Controllers.Mvc
{
    [Authorize]
    public class SocialSecurityController : Controller
    {
        private readonly ISocialSecurityService _socialSecurityService = new SocialSecurityService();

        /// <summary>
        /// 社保待办业务
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SocialSecurityWaitingHandle(SocialSecurityParameter parameter)
        {
            ViewData["SocialSecurityPeopleName"] = parameter.SocialSecurityPeopleName;
            ViewData["IdentityCard"] = parameter.IdentityCard;

            PagedResult<SocialSecurityShowModel> list = await _socialSecurityService.GetSocialSecurityList(parameter);

            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(UserTypeEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "全部", Value = "", Selected = true });

            ViewData["UserType"] = new SelectList(UserTypeList, "Value", "Text", parameter.UserType);

            return View(list);
        }

        /// <summary>
        /// 社保归档业务
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SocialSecurityNormal(SocialSecurityParameter parameter)
        {
            ViewData["SocialSecurityPeopleName"] = parameter.SocialSecurityPeopleName;
            ViewData["IdentityCard"] = parameter.IdentityCard;

            PagedResult<SocialSecurityShowModel> list = await _socialSecurityService.GetSocialSecurityList(parameter);

            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(UserTypeEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "全部", Value = "" });

            ViewData["UserType"] = new SelectList(UserTypeList, "Value", "Text", parameter.UserType);

            return View(list);
        }

        /// <summary>
        /// 社保待续费业务
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SocialSecurityRenew(SocialSecurityParameter parameter)
        {
            ViewData["SocialSecurityPeopleName"] = parameter.SocialSecurityPeopleName;
            ViewData["IdentityCard"] = parameter.IdentityCard;

            PagedResult<SocialSecurityShowModel> list = await _socialSecurityService.GetSocialSecurityList(parameter);

            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(UserTypeEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "全部", Value = "" });

            ViewData["UserType"] = new SelectList(UserTypeList, "Value", "Text", parameter.UserType);

            return View(list);
        }

        /// <summary>
        /// 社保待办停业务
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SocialSecurityWaitingStop(SocialSecurityParameter parameter)
        {
            ViewData["SocialSecurityPeopleName"] = parameter.SocialSecurityPeopleName;
            ViewData["IdentityCard"] = parameter.IdentityCard;

            PagedResult<SocialSecurityShowModel> list = await _socialSecurityService.GetSocialSecurityList(parameter);

            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(UserTypeEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "全部", Value = "" });

            ViewData["UserType"] = new SelectList(UserTypeList, "Value", "Text", parameter.UserType);

            return View(list);
        }

        /// <summary>
        /// 社保已办停业务
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SocialSecurityAlreadyStop(SocialSecurityParameter parameter)
        {
            ViewData["SocialSecurityPeopleName"] = parameter.SocialSecurityPeopleName;
            ViewData["IdentityCard"] = parameter.IdentityCard;

            PagedResult<SocialSecurityShowModel> list = await _socialSecurityService.GetSocialSecurityList(parameter);

            List<SelectListItem> UserTypeList = EnumExt.GetSelectList(typeof(UserTypeEnum));
            UserTypeList.Insert(0, new SelectListItem { Text = "全部", Value = "" });

            ViewData["UserType"] = new SelectList(UserTypeList, "Value", "Text", parameter.UserType);

            return View(list);
        }



        /// <summary>
        /// 批量办结
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BatchComplete(int[] SocialSecurityPeopleIDs)
        {
            //修改参保人社保状态
            bool flag = _socialSecurityService.ModifySocialStatus(SocialSecurityPeopleIDs, (int)SocialSecurityStatusEnum.Normal);

            #region 记录日志
            if (flag == true)
            {
                string names = _socialSecurityService.GetSocialPeopleNames(SocialSecurityPeopleIDs);
                LogService.WriteLogInfo(new Log { UserName = HttpContext.User.Identity.Name, Contents = string.Format("社保业务办结，客户:{0}", names) });
            }
            #endregion

            return Json(new { status = flag, Message = flag ? "办结成功" : "办结失败" });
        }

        /// <summary>
        /// 批量办停
        /// </summary>
        /// <param name="SocialSecurityPeopleIDs"></param>
        /// <returns></returns>
        public JsonResult BatchStop(int[] SocialSecurityPeopleIDs)
        {
            //修改参保人社保状态
            bool flag = _socialSecurityService.ModifySocialStatus(SocialSecurityPeopleIDs, (int)SocialSecurityStatusEnum.AlreadyStop);

            #region 记录日志
            if (flag == true)
            {
                string names = _socialSecurityService.GetSocialPeopleNames(SocialSecurityPeopleIDs);
                LogService.WriteLogInfo(new Log { UserName = HttpContext.User.Identity.Name, Contents = string.Format("社保业务办停，客户:{0}", names) });
            }
            #endregion

            return Json(new { status = flag, Message = flag ? "办停成功" : "办停失败" });
        }

        /// <summary>
        /// 查询社保详情
        /// </summary>
        /// <param name="SocialSecurityPeopleID"></param>
        /// <returns></returns>
        public ActionResult SocialSecurityDetail(int SocialSecurityPeopleID)
        {
            SocialSecurity model = _socialSecurityService.GetSocialSecurityDetail(SocialSecurityPeopleID);

            return View(model);
        }

        /// <summary>
        /// 保存社保号
        /// </summary>
        /// <param name="SocialSecurityNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSocialSecurityNo(int SocialSecurityPeopleID, string SocialSecurityNo)
        {
            bool flag = _socialSecurityService.SaveSocialSecurityNo(SocialSecurityPeopleID, SocialSecurityNo);
            TempData["Message"] = flag ? "更新成功" : "更新失败";

            #region 记录日志
            if (flag == true)
            {
                string names = _socialSecurityService.GetSocialPeopleNames(new int[] { SocialSecurityPeopleID});
                LogService.WriteLogInfo(new Log { UserName = HttpContext.User.Identity.Name, Contents = string.Format("更新社保客户:{0}，社保号为：{1}", names,SocialSecurityNo) });
            }
            #endregion

            return RedirectToAction("SocialSecurityDetail", new { SocialSecurityPeopleID = SocialSecurityPeopleID });
        }


    }
}