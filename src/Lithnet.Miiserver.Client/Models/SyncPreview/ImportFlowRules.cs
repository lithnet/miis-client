﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportFlowRules : NodeCache
    {
        public ImportFlowRules (XmlNode node)
        :base(node)
        {
        }

        public IReadOnlyDictionary<string, ImportFlowResult> ImportFlows
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ImportFlowResult>("import-flow", (t) => t.TargetAttribute, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string Type
        {
            get
            {
                return this.GetValue<string>("@import-flow-type");
            }
        }

        public bool HasError
        {
            get
            {
                return this.GetValue<bool>("@has-error");
            }
        }
    }
}
