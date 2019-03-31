using NLog;
using System;
using System.Web;
using CLAIM.Helpers.Configuration;

namespace CLAIM.Helpers
{
    public class SessionHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void AbandonSession(HttpSessionStateBase session)
        {
            try
            {
                logger.Info(string.Format("Clearing session {0}.", session.SessionID));

                ConfigurationHelper config = new ConfigurationHelper();
                if (!config.IsMockUserIdentity)
                {
                    session.Clear();
                    session.Abandon();
                }
            }
            catch (Exception)
            {
                session.Clear();
                session.Abandon();
            }
        }

        public static void TerminateSession(HttpSessionStateBase session, string returnUrl)
        {
            try
            {
                logger.Info(string.Format("Termintating session {0}.", session.SessionID));

                ConfigurationHelper config = new ConfigurationHelper();
                if (!config.IsMockUserIdentity)
                {
                    session.Clear();
                }

                session["Terminated"] = true;
                session["ReturnUrl"] = returnUrl;
            }
            catch (Exception)
            {
                session.Clear();

                session["Terminated"] = true;
                session["ReturnUrl"] = returnUrl;
            }
        }
    }
}
