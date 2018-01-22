using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace OpenData.Web.Mvc
{

    public sealed class ControllerTypeCache
    {
        private static Dictionary<string, Type> _cache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public static void RegisterController(string controllerTypeName, Type type)
        {
            _cache[controllerTypeName] = type;
        }

        public static Type GetControllerType(string controllerName, IEnumerable<string> namespaces)
        {
            foreach (var item in namespaces)
            {
                var controllerTypeName = item + "." + controllerName + "Controller";
                if (_cache.ContainsKey(controllerTypeName))
                {
                    return _cache[controllerTypeName];
                }
            }
            return null;
        }

    }
    public class BzwayControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }
            var controller = (IController)ApplicationEngine.Current.Resolve(controllerType);
            if (controller == null)
            {
                return (IController)base.GetControllerInstance(requestContext, controllerType);
            }
            return controller;
        }
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            Type controllerType = null;
            object obj2;
            if ((requestContext != null) && requestContext.RouteData.DataTokens.TryGetValue("Namespaces", out obj2))
            {
                IEnumerable<string> namespaces = obj2 as IEnumerable<string>;
                if ((namespaces != null) && namespaces.Any<string>())
                {
                    controllerType = ControllerTypeCache.GetControllerType(controllerName, namespaces);
                }
            }
            if (controllerType != null)
            {
                return controllerType;
            }

            return base.GetControllerType(requestContext, controllerName);
        }
    }
}