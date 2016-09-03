namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Indicates a type of flow rule
    /// </summary>
    public enum FlowRuleType
    {
        /// <summary>
        /// No flow rule
        /// </summary>
        None = 0,

        /// <summary>
        /// A direct attribute flow
        /// </summary>
        Direct = 1,

        /// <summary>
        /// A flow of an individual component of a DN
        /// </summary>
        DNComponent = 2,

        /// <summary>
        /// An advanced flow using a rules extension
        /// </summary>
        RulesExtension = 3,

        /// <summary>
        /// A flow of a constant value 
        /// </summary>
        Constant = 4,

        /// <summary>
        /// A flow controlled by a sync rule
        /// </summary>
        SyncRule = 5
    }
}
