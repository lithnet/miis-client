using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithnet.Miiserver.Client
{
    public class MiiserverException : Exception
    {
        public MiiserverException()
            : base()
        {
        }

        public MiiserverException(string message)
            : base(message)
        {
        }

        public MiiserverException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
