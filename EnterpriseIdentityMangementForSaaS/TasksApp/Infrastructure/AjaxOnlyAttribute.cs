using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

// ReSharper disable CheckNamespace
namespace System.Web.Mvc
// ReSharper restore CheckNamespace
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed, "Resource is only accessible via XMLHttpRequest");
        }
    }
}