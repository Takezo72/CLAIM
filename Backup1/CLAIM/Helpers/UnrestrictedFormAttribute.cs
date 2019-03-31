using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers
{
    public class UnrestrictedFormAttribute : AuthorizeAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ConfigurationHelper config = new ConfigurationHelper();
            if (config.IsMockUserIdentity)
            {
                logger.Info(string.Format("MOCK USER ACTIVATED! Access is granted."));

                return true;
            }

            if (httpContext.Request["TrackingNumber"] != null)
            {
                httpContext.Session["TrackingNumber"] = httpContext.Request["TrackingNumber"];
            }

            if (httpContext.Session["TrackingNumber"] == null)
            {
                logger.Warn(string.Format("Access is denied. No tracking number found or provided."));
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            logger.Warn(string.Format("Access is denied. Redirecting to RequestFollowUp."));

            ConfigurationHelper helper = new ConfigurationHelper();

            string url = helper.RequestFollowUpReturnUrlDenied;

            logger.Warn(string.Format("Redirecting to {0}.", url));

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "External", ReturnURL = url }));
        }
    }
}


