using System.Linq;
using System.Reflection;

namespace Bzway.Data
{
    internal static class OpenEntityHelper
    {
        public static object TryGetValue(this object entity, string name)
        {
            try
            {
                PropertyInfo info = entity.GetType().GetProperties().Where(m => m.Name == name && m.CanRead).First();
                return info.GetValue(entity, null);
            }
            catch
            {
                return null;
            }
        }
        public static void TrySetValue(this object entity, string name, object value)
        {
            try
            {
                PropertyInfo info = entity.GetType().GetProperties().Where(m => m.Name == name && m.CanRead).First();
                info.SetValue(entity, value);
            }
            catch
            {
            }
        }
    }
}