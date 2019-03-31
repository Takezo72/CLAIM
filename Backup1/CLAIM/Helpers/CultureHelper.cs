using NLog;
using System;
using System.Globalization;
using System.Threading;
using System.Web;

namespace CLAIM.Helpers
{
    public class CultureHelper
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal void ChangeCulture(string culture, HttpRequest request, HttpResponse response)
        {
            string newCulture = "fr-CA";
            if (culture.ToUpper() == "EN" || culture.ToUpper() == "EN-CA")
            {
                newCulture = "en-CA";
            }

            logger.Info(string.Format("SWITCH language to : {0}", newCulture));

            HttpCookie languageCookie = new HttpCookie("language")
            {
                Path = "/",
                Expires = DateTime.Now.AddDays(1)
            };

            languageCookie.Value = newCulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(newCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(newCulture);

            request.Cookies.Add(languageCookie);
            response.SetCookie(languageCookie);

        }

        internal bool InitializeCulture(HttpRequestBase request, HttpResponseBase response)
        {
            bool cultureChanged = false;
            string newCulture = request.QueryString["lang"];

            if (!string.IsNullOrEmpty(newCulture))
            {
                if (newCulture.ToUpper() == "FR" || newCulture.ToUpper() == "EN")
                {
                    if (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpper() != newCulture.ToUpper())
                    {
                        cultureChanged = true;
                    }
                }
            }

            return cultureChanged;
        }
    }
}
