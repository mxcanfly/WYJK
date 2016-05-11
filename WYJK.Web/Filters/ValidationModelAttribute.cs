using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace WYJK.Web.Filters
{
    public class ValidationModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 当执行 Action 时触发
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {

        }
    }
}