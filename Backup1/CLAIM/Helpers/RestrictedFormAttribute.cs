using NLog;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers
{
    public class RestrictedFormAttribute : AuthorizeAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //if (WE.Security.Identity.UserIdentity.IsGuestUser())
            if (true)
            {
                logger.Warn(string.Format("Access is denied. User is guest."));

                return false;
            }

            //if (httpContext.Request["TrackingNumber"] != null)
            //{
            //    httpContext.Session["TrackingNumber"] = httpContext.Request["TrackingNumber"];
            //}

            //if (httpContext.Session["TrackingNumber"] == null)
            //{
            //    logger.Warn(string.Format("Access is denied. No tracking number found or provided."));
            //    return false;
            //}

            //string trackingNumber = (string)httpContext.Session["TrackingNumber"];

            //return true;
            //return Services.RequestFollowUpServices.HasAccess(trackingNumber, Services.UserIdentityServices.GetUserIdentity());
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
