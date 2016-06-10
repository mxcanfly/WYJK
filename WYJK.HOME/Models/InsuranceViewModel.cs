using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WYJK.Entity;

namespace WYJK.HOME.Models
{
    public class InsuranceQueryParamModel : PagedParameter
    {
        public string SocialSecurityPeopleName { get; set; }
        public string HouseholdProperty { get; set; }
        public string InsuranceArea { get; set; }
    }

    public class InsuranceListViewModel
    {

        public string SocialSecurityPeopleID { get; set; }

        public string SocialSecurityPeopleName { get; set; }
        public string IdentityCard { get; set; }
        public string HouseholdProperty { get; set; }
        public string PayTime { get; set; }
        public string StopDate { get; set; }
        public string SocialSecurityBase { get; set; }
        public string SocialSecurityStatus { get; set; }
        public string SocialSecurityAmount { get; set; }
        public string AccumulationFundBase { get; set; }
        public string AccumulationFundStatus { get; set; }

    }

    public class InsuranceAdd1ViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>		
        [Required]
        [Display(Name = "真实姓名")]
        public string SocialSecurityPeopleName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>		
        [Required]
        [MaxLength(18)]
        [MinLength(18)]
        [Display(Name = "身份证号")]
        public string IdentityCard { get; set; }
        /// <summary>
        /// 身份证照片
        /// </summary>		
        [Required]
        [Display(Name = "身份证照片")]
        public string IdentityCardPhoto { get; set; }
        /// <summary>
        /// 身份证照片数组
        /// </summary>
        [Required]
        [Display(Name = "身份证照片数组")]
        public string[] ImgUrls { get; set; }
        /// <summary>
        /// 户口性质
        /// </summary>		
        [Required]
        [Display(Name = "户口性质")]
        public string HouseholdProperty { get; set; }
    }
}