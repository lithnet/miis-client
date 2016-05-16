using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class DsmlObjectClass : NodeCache
    {
        internal DsmlObjectClass(XmlNode node, IReadOnlyDictionary<string, DsmlAttribute> allAttributes)
            : base(node)
        {
            Dictionary<string, DsmlAttribute> attributes = new Dictionary<string, DsmlAttribute>();

            foreach (XmlNode n2 in node.SelectNodes("dsml:attribute/@ref", this.nsmanager))
            {
                string name = n2.InnerText.Remove(0, 1);
                if (allAttributes.ContainsKey(name))
                {
                    attributes.Add(name, allAttributes[name]);
                }
            }

            this.Attributes = new ReadOnlyDictionary<string, DsmlAttribute>(attributes);
        }

        public string Name
        {
            get
            {
                return this.GetValue<string>("dsml:name");
            }
        }

        public IReadOnlyDictionary<string, DsmlAttribute> Attributes { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}


/*
<dsml:class id="synchronizationRule" type="structural">
  <dsml:name>synchronizationRule</dsml:name>
  <dsml:attribute ref="#csObjectID" required="true" />
  <dsml:attribute ref="#connectedObjectType" required="true" />
  <dsml:attribute ref="#connectedSystem" required="true" />
  <dsml:attribute ref="#connectedSystemScope" required="false" />
  <dsml:attribute ref="#msidmOutboundScopingFilters" required="false" />
  <dsml:attribute ref="#createConnectedSystemObject" required="false" />
  <dsml:attribute ref="#createILMObject" required="false" />
  <dsml:attribute ref="#disconnectConnectedSystemObject" required="true" />
  <dsml:attribute ref="#dependency" required="false" />
  <dsml:attribute ref="#description" required="false" />
  <dsml:attribute ref="#displayName" required="true" />
  <dsml:attribute ref="#existenceTest" required="false" />
  <dsml:attribute ref="#flowType" required="true" />
  <dsml:attribute ref="#initialFlow" required="false" />
  <dsml:attribute ref="#ilmObjectType" required="true" />
  <dsml:attribute ref="#objectID" required="false" />
  <dsml:attribute ref="#persistentFlow" required="false" />
  <dsml:attribute ref="#precedence" required="false" />
  <dsml:attribute ref="#relationshipCriteria" required="false" />
  <dsml:attribute ref="#synchronizationRuleParameters" required="false" />
  <dsml:attribute ref="#msidmOutboundIsFilterBased" required="false" />
</dsml:class>
*/
