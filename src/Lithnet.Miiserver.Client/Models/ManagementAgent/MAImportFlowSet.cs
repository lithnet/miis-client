using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            return string.Format("{0} -> {1}", this.MVObjectType, this.CDObjectType);
        }
    }
}