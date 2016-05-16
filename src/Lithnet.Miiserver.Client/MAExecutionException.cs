using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithnet.Miiserver.Client
{
    public class MAExecutionException : MiiserverException
    {
        public string Result { get; private set; }

        public MAExecutionException()
            : base()
        {
        }

        public MAExecutionException(string result)
            : base(string.Format("Run profile execution failed: {0}", result))
        {
            this.Result = result;
        }

        public MAExecutionException(string result, Exception innerException)
            : base(string.Format("Run profile execution failed: {0}", result), innerException)
        {
            this.Result = result;
        }
    }
}
