using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithnet.Miiserver.Client
{
    public enum FlowRuleType
    {
        None = 0,
        Direct = 1,
        DNComponent = 2,
        RulesExtension = 3,
        Constant = 4,
        SyncRule = 5
    }
}
