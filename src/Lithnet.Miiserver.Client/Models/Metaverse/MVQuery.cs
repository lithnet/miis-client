using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVQuery
    {
        public MVQuery()
        {
            this.CollationOrder = "SQL_Latin1_General_CP1_CI_AS";
            this.QueryItems = new List<MVAttributeQuery>();
        }

        public string CollationOrder { get; set; }

        public DsmlObjectClass ObjectType { get; set; }

        public IList<MVAttributeQuery> QueryItems { get; private set; }

        internal string GetXml()
        {
            XmlDocument d = new XmlDocument();

            XmlElement filterElement = d.CreateElement("mv-filter");
            filterElement.SetAttribute("collation-order", this.CollationOrder);

            foreach(MVAttributeQuery query in this.QueryItems)
            {
                filterElement.AppendChild(query.GetElement(d));
            }

            if (this.ObjectType != null)
            {
                XmlElement objectTypeElement = d.CreateElement("mv-object-type");
                objectTypeElement.InnerText = this.ObjectType.Name;
                filterElement.AppendChild(objectTypeElement);
            }

            d.AppendChild(filterElement);

            return d.OuterXml;
        }
    }
}
/*
<mv-filter collation-order="SQL_Latin1_General_CP1_CI_AS">
  <mv-attr name="requestedAccountName" type="string" search-type="starts-with">
    <value>ims</value>
  </mv-attr>
  <mv-object-type>user</mv-object-type>
</mv-filter>
*/
