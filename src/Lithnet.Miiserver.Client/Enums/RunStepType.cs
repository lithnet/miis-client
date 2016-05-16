namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Flags]
    public enum RunStepType
    {
        Unknown = 0,
        Export = 1,
        FullImport = 2,
        DeltaImport = 4,
        FullSynchronization = 8,
        DeltaSynchronization = 16,
        FullImportFullSynchronization = 32,
        DeltaImportDeltaSynchronization = 64,
        FullImportDeltaSynchronization = 128,
    }
}
