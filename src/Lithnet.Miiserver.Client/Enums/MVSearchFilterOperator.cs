using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithnet.Miiserver.Client
{
    public enum MVSearchFilterOperator
    {
        Equals = 0,
        Contains = 1,
        NotContains = 2,
        IsPresent = 3,
        IsNotPresent = 4,
        StartsWith = 5,
        EndsWith = 6
    }
}