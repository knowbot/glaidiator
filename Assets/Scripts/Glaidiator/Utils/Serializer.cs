using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Glaidiator.Utils
{
    public static class Serializer
    {
        public static void Serialize(IXmlSerializable obj, string prefix = "", string path = "Test/")
        {
            var doc = new XmlDocument();
            string[] fnParts = { prefix, obj.GetType().Name};
            string filename = string.Join("_", fnParts.Where(s => !string.IsNullOrEmpty(s)));
            var w = new XmlTextWriter($@"{path}/{filename}.xml", Encoding.UTF8);
            w.Formatting = Formatting.Indented;
            obj.WriteXml(w);
            w.Close();
            doc.Save(w);
        }
    }
}