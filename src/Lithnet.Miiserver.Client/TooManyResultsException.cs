using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithnet.Miiserver.Client
{
    [Serializable]
    public class TooManyResultsException : Exception
    {
        public TooManyResultsException()
            : base()
        {
        }
         
        public TooManyResultsException(string message)
            :base(message)
        {
        }
    }
}
