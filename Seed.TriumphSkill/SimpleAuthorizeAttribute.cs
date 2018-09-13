using Seed.TriumphSkill.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Seed.TriumphSkill
{
    public class SimpleAuthorizeAttribute : AuthorizeAttribute
    {
        public IPermissionService PermissionService { get; set; }

        private string routeDataController = "controller";

        public string RouteDataController
        {
            get { return routeDataController; }
            set { routeDataController = value; }
        }

        private string routeDataAction = "action";

        public string RouteDataAction
        {
            get { return routeDataAction; }
            set { routeDataAction = value; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            RequestContext requestContext = httpContext.Request.RequestContext;

            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
                return false;

            if (!requestContext.RouteData.Values.ContainsKey(RouteDataController))
                throw new ApplicationException("RouteDataKey " + RouteDataController + " does not exist in the current RouteData");

            if (!requestContext.RouteData.Values.ContainsKey(RouteDataAction))
                throw new ApplicationException("RouteDataKey " + RouteDataAction + " does not exist in the current RouteData");

            if (!(user.Identity is SimpleIdentity))
                return false;

            if ((user.Identity as SimpleIdentity).IsAdministrator)
                return true;

            string activity = "ACTION." + requestContext.RouteData.Values[RouteDataController].ToString() + "." + requestContext.RouteData.Values[RouteDataAction].ToString();
            List<string> permissions = PermissionService.GetAllForUser((user.Identity as SimpleIdentity).Username);
            return permissions.Contains(activity);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Unauthorized"
                            })
                        );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Login"
                            })
                        );
            }
        }
    }
}