using System;
using System.Text;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree
{
    public static class TreeSerializer
    {
        public static void Serialize(BTree tree)
        {
            var doc = new XmlDocument();
            var w = new XmlTextWriter($@"BTs/Test/{Guid.NewGuid()}.xml", Encoding.UTF8);
            w.Formatting = Formatting.Indented;
            w.WriteStartElement(tree.GetType().Name);
            tree.GetRoot().WriteXml(w);
            w.WriteEndElement();
            w.Close();
            doc.Save(w);
        }
    }
}