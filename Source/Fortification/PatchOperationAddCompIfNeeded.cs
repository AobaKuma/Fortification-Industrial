using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace Fortification
{
    public class PatchOperationAddCompIfNeeded : PatchOperationPathed
    {

        protected override bool ApplyWorker(XmlDocument xml)
        {
            XmlNode node = null;
            bool result = false;
            foreach (object item in xml.SelectNodes(xpath))
            {
                XmlNode xmlNode = item as XmlNode;
                XmlNode xmlNode2 = xmlNode["comps"];
                if (xmlNode2 == null)
                {
                    xmlNode2 = xmlNode.OwnerDocument.CreateElement("comps");
                    xmlNode.AppendChild(xmlNode2);
                }
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(childNode, deep: true));
                }
                result = true;
            }
            return result;
        }
    }
}
