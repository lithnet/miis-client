using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class CounterDetail : XmlObjectBase
    {
        internal CounterDetail(XmlNode node, Guid stepID)
            : base(node)
        {
            this.StepId = stepID;
        }
        
        public int Count => this.GetValue<int>(".");

        public bool HasDetail => this.Count > 0 && this.GetValue<bool>("@detail");

        public string Type => this.XmlNode.Name;

        public Guid StepId { get; }
     }
}