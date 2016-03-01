using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace WYJK.Web.Filters
{
    /// <summary>
    /// 身份验证过滤器
    /// </summary>
    public class AuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //actionContext.Result = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}