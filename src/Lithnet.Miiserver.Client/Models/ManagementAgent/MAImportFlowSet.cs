using System.Collections.Generic;

namespace Lithnet.Miiserver.Client
{
    public class MAImportFlowSet 
    {
        internal MAImportFlowSet(string csObjectType, string mvObjectType, IReadOnlyList<MAImportFlow> importFlows)
        {
            this.CDObjectType = csObjectType;
            this.MVObjectType = mvObjectType;
            this.ImportFlows = importFlows;
        }

        public string CDObjectType { get; private set; }

        public string MVObjectType { get; private set; }

        public IReadOnlyList<MAImportFlow> ImportFlows { get; private set; }

        public override string ToString()
        {
            return $"{this.MVObjectType} -> {this.CDObjectType}";
        }
    }
}