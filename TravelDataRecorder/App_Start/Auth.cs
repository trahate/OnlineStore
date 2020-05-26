using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Script.Serialization;

namespace TravelDataRecorder.App_Start
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                
            }
            else
            {
                //returns to login url
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "LogIn" }));
            }
        }
    }
}