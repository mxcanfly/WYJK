using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WYJK.HOME.Models
{
    public class UserLoginAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserInfo"] == null)
            {
                filterContext.RequestContext.HttpContext.Response.Redirect("~/User/Login");
            }


        }


    }
}