using System;
using System.Web.Mvc;

namespace CLAIM.Helpers
{
    public class FloatModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            float value = 0;

            if (!float.TryParse(valueProviderResult.AttemptedValue.Replace(',', '.'), out value))
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            else
            {
                return value;
            }
        }
    }
}
