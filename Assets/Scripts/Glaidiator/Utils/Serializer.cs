using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Glaidiator.Utils
{
    public class Serializer
    {
        public static string Serialize(object obj)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("<?");
            Type myType = obj.GetType();
            sb.Append(myType);
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(obj, null);
                sb.AppendLine();
                sb.Append(@"    [" + prop.Name + "=" + propValue + "]");
            }
            sb.AppendLine();
            sb.Append("?>");
            return sb.ToString();
        }
    }
}