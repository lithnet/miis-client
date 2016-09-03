namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// An exception that is thrown when a run profile on a management fails
    /// </summary>
    public class MAExecutionException : MiiserverException
    {
        /// <summary>
        /// Gets the result of the run profile
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MAExecutionException class
        /// </summary>
        public MAExecutionException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MAExecutionException class
        /// </summary>
        /// <param name="result">The result returned by the run profile</param>
        public MAExecutionException(string result)
            : base($"Run profile execution failed: {result}")
        {
            this.Result = result;
        }
    }
}
