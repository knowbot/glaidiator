using System;
using System.Text;
using System.Xml;
using Glaidiator.BehaviorTree.Base;

namespace Glaidiator.BehaviorTree
{
    public static class Serializer
    {
        public static void Serialize(BTree tree)
        {
            var doc = new XmlDocument();
            var w = new XmlTextWriter($@"BTs/Test/tree_{Guid.NewGuid()}.xml", Encoding.UTF8);
            w.Formatting = Formatting.Indented;
            w.WriteStartElement(tree.GetType().Name);
            tree.GetRoot().WriteXml(w);
            w.WriteEndElement();
            w.Close();
            doc.Save(w);
        }
        
        public static void Serialize(Node node)
        {
            var doc = new XmlDocument();
            var w = new XmlTextWriter($@"BTs/Test/node_{Guid.NewGuid()}.xml", Encoding.UTF8);
            w.Formatting = Formatting.Indented;
            node.WriteXml(w);
            w.Close();
            doc.Save(w);
        }
    }
}