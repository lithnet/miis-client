using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RunConfiguration : XmlObjectBase
    {
        internal RunConfiguration(XmlNode node)
            : base(node)
        {
        }

        public Guid ID => this.GetValue<Guid>("id");

        public string Name => this.GetValue<string>("name");

        public DateTime CreationTime => this.GetValue<DateTime>("creation-time");

        public DateTime LastModificationTime => this.GetValue<DateTime>("last-modification-time");

        public int Version => this.GetValue<int>("version");

        public IReadOnlyList<RunStep> RunSteps => this.GetReadOnlyObjectList<RunStep>("configuration/step");

        public override string ToString()
        {
            return this.Name;
        }
    }
}
