using System;
using System.Web.Mvc;
using OpenData.Data.Core;


namespace OpenData.Framework.Common
{
    public class EntityModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (typeof(DynamicEntity).IsAssignableFrom(modelType))
            {
                var idValue = bindingContext.ValueProvider.GetValue("id");
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}
