using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportFlowSet : XmlObjectBase
    {
        internal ExportFlowSet(XmlNode node)
            : base(node)
        {
        }

        public string CDObjectType => this.GetValue<string>("@cd-object-type");

        public string MVObjectType => this.GetValue<string>("@mv-object-type");

        public IReadOnlyDictionary<string, ExportFlow> ExportFlows
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ExportFlow>("export-flow", t => t.TargetAttribute, StringComparer.OrdinalIgnoreCase);
            }
        }

        public override string ToString()
        {
            return $"{this.MVObjectType} -> {this.CDObjectType}";
        }
    }
}