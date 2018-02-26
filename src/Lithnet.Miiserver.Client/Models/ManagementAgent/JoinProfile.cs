using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class JoinProfile : XmlObjectBase
    {
        internal JoinProfile(XmlNode node)
            : base(node)
        {
        }

        public string ObjectType => this.GetValue<string>("@cd-object-type");

        public IReadOnlyList<JoinCriterion> JoinCriteria => this.GetReadOnlyObjectList<JoinCriterion>("join-criterion");
    }
}

