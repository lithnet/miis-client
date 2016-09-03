namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// A filter operator that can be used when searching the metaverse
    /// </summary>
    public enum MVSearchFilterOperator
    {
        /// <summary>
        /// Equals
        /// </summary>
        Equals = 0,

        /// <summary>
        /// Contains
        /// </summary>
        Contains = 1,

        /// <summary>
        /// Does not contain
        /// </summary>
        NotContains = 2,

        /// <summary>
        /// Is present
        /// </summary>
        IsPresent = 3,

        /// <summary>
        /// Is not present
        /// </summary>
        IsNotPresent = 4,

        /// <summary>
        /// Starts with
        /// </summary>
        StartsWith = 5,

        /// <summary>
        /// Ends with
        /// </summary>
        EndsWith = 6
    }
}