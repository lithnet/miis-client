using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class CounterDetail : XmlObjectBase
    {
        private Guid stepID;

        internal CounterDetail(XmlNode node, Guid stepID)
            : base(node)
        {
            this.stepID = stepID;
        }
        
        public int Count => this.GetValue<int>(".");

        public bool HasDetail => this.GetValue<bool>("@detail");

        public IEnumerable<CSObjectRef> Items => RunStep.GetStepDetailCSObjectRefs(this.stepID, this.XmlNode.Name);
    }
}