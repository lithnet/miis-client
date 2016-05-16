namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class JoinProfile : NodeCache
    {
        internal JoinProfile(XmlNode node)
            : base(node)
        {
        }

        public string ObjectType
        {
            get
            {
                return this.GetValue<string>("@cd-object-type");
            }
        }

        public IReadOnlyList<JoinCriterion> JoinCriteria
        {
            get
            {
                return this.GetReadOnlyObjectList<JoinCriterion>("join-criterion");
            }
        }
    }
}

