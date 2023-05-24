using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Glaidiator.Utils
{
    public static class Serializer
    {
        public static void Serialize(IXmlSerializable obj, string prefix = "", string path = "Test/")
        {
            var doc = new XmlDocument();
            string dir = Path.Join(Application.dataPath, path);
            if (!dir.EndsWith('/')) dir += '/';
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string[] fnParts = {prefix, obj.GetType().Name};
            string filename = string.Join("_", fnParts.Where(s => !string.IsNullOrEmpty(s)));
            var w = new XmlTextWriter($@"{dir}{filename}.xml", Encoding.UTF8);
            w.Formatting = Formatting.Indented;
            obj.WriteXml(w);
            w.Close();
            doc.Save(w);
        }
    }
}