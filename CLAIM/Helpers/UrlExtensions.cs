using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace CLAIM.Helpers
{
    public static class UrlExtensions
    {
        public static string LocalizedUrl(this UrlHelper urlHelper, string contentPath)
        {
            return urlHelper.Content(contentPath);
        }

        public static string UrlStaticContent(this UrlHelper urlHelper, string contentPath)
        {
            var urlStaticRessources = ConfigurationManager.AppSettings["StaticResourcesURL"];
            //Method called only if mode debug
            SetLocalUrl(urlHelper, ref urlStaticRessources);
            if (string.IsNullOrEmpty(urlStaticRessources))
                throw new Exception("Missing value for key StaticResourcesURL. Verify web.config.");

            return ConstruireUrl(contentPath, urlStaticRessources, ".*\\.(css|js|gif|jpg|png)(/.*)?");
        }

        [Conditional("DEBUG")]
        private static void SetLocalUrl(UrlHelper urlHelper, ref string urlStaticRessources)
        {
            if (urlStaticRessources != null)
            {
                var request = urlHelper.RequestContext.HttpContext.Request;
                var url = request.Url.Scheme + "://" + request.Url.Authority;
                urlStaticRessources = url;
            }
        }


        private static string ConstruireUrl(string contenu, string urlBase, string noVersionRegex, bool ajouterVersion = true)
        {
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            if (!string.IsNullOrEmpty(urlBase))
            {
                contenu = contenu.Substring(0, 1) == "~" ? contenu.Substring(1) : contenu;
                contenu = contenu.Substring(0, 1) == "/" ? contenu.Substring(1) : contenu;

                urlBase = helper.Content(urlBase);
                urlBase = urlBase.Substring(urlBase.Length - 1, 1) != "/" ? urlBase + "/" : urlBase;

                //On rajoute le numéro de version de l'application pour contrer les différents problèmes de "cache" des navigateurs

                if (HttpContext.Current != null && !string.IsNullOrEmpty(string.Format("{0}", HttpContext.Current.Application["NoVersion"])) && (string.IsNullOrEmpty(noVersionRegex) || Regex.IsMatch(contenu.ToLower(), noVersionRegex)) && ajouterVersion)
                {
                    return string.Format("{0}{1}?v={2}", urlBase, contenu, HttpContext.Current.Application["NoVersion"]);
                }

                return urlBase + contenu;
            }
            if (!string.IsNullOrEmpty(urlBase) && contenu == "~")
            {
                return helper.Content(urlBase);
            }

            return helper.Content(contenu);
        }
    }
}

