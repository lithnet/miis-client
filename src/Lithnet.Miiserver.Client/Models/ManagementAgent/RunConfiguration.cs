namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using System.Diagnostics;
    using System.Xml;

    public class RunConfiguration : NodeCache
    {
        internal RunConfiguration(XmlNode node)
            : base(node)
        {
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("id");
            }
        }
        
        public string Name
        {
            get
            {
                return this.GetValue<string>("name");
            }
        }
          
        public DateTime CreationTime
        {
            get
            {
                return this.GetValue<DateTime>("creation-time");
            }
        }

        public DateTime LastModificationTime
        {
            get
            {
                return this.GetValue<DateTime>("last-modification-time");
            }
        }

        public int Version
        {
            get
            {
                return this.GetValue<int>("version");
            }
        }

        public IReadOnlyList<RunStep> RunSteps
        {
            get
            {
                return this.GetReadOnlyObjectList<RunStep>("configuration/step");
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
