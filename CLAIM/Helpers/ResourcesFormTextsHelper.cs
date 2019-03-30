using System;
using System.Globalization;
using System.Resources;
using System.Web;
using CLAIM.Ressources.FormTexts;

namespace CLAIM.Helpers
{
    public class ResourcesFormTextsHelper
    {
        private readonly ResourceManager _resourceManager;

        public CultureInfo ResourceCulture { get; }

        public Type ResourceType { get; }

        public ResourcesFormTextsHelper(bool isInsured, CultureInfo resourceCulture)
        {
            ResourceType = isInsured ? typeof(UI) : typeof(UI_Representative);
            _resourceManager = new ResourceManager(ResourceType);
            ResourceCulture = resourceCulture ?? CultureInfo.CurrentCulture;
        }

        public IHtmlString GetString(string key)
        {
            return GetString(key, ResourceCulture);
        }

        public IHtmlString GetString(string key, CultureInfo resourceCulture)
        {
            return new HtmlString(_resourceManager.GetString(key, resourceCulture));
        }
    }
}

