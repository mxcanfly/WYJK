using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WYJK.Entity;
using System.ComponentModel.DataAnnotations;

namespace WYJK.HOME.Models
{
    public class SocialSecurityViewModel:SocialSecurity
    {
        public string IdentityCard { get; set; }

        public decimal AccumulationFundBase { get; set; }

        public Dictionary<string,decimal> SocialAccumulationDict { get; set; }

        public AdjustingBase AdjustBase { get; set; }


    }
}