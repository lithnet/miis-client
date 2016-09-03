using System;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// An exception that is throw when more than the expected number of results are returned from a query
    /// </summary>
    [Serializable]
    public class TooManyResultsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TooManyResultsException
        /// </summary>
        public TooManyResultsException()
            : base()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the TooManyResultsException
        /// </summary>
        /// <param name="message">A message describing the condition</param>
        public TooManyResultsException(string message)
            :base(message)
        {
        }
    }
}
