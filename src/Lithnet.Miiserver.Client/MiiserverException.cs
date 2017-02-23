using System;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// An exception that is thrown by the synchronization service
    /// </summary>
    public class MiiserverException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MiiserverException class
        /// </summary>
        public MiiserverException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MiiserverException class
        /// </summary>
        /// <param name="message">A message describing the error</param>
        public MiiserverException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MiiserverException class
        /// </summary>
        /// <param name="message">A message describing the error</param>
        /// <param name="innerException">The exception that triggered this exception</param>
        public MiiserverException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
