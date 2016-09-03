using System;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines the types of run step
    /// </summary>
    [Flags]
    public enum RunStepType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// An export step
        /// </summary>
        Export = 1,

        /// <summary>
        /// A full import step
        /// </summary>
        FullImport = 2,

        /// <summary>
        /// A delta import step
        /// </summary>
        DeltaImport = 4,

        /// <summary>
        /// A full synchronization step
        /// </summary>
        FullSynchronization = 8,

        /// <summary>
        /// A delta synchronization step
        /// </summary>
        DeltaSynchronization = 16,

        /// <summary>
        /// A combined full import and full synchronization step
        /// </summary>
        FullImportFullSynchronization = 32,

        /// <summary>
        /// A combined delta import and delta synchronization step
        /// </summary>
        DeltaImportDeltaSynchronization = 64,

        /// <summary>
        /// A combined full import and delta synchronization step
        /// </summary>
        FullImportDeltaSynchronization = 128,
    }
}
