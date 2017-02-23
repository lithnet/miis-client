using System;
using System.Diagnostics.CodeAnalysis;

namespace Lithnet.Miiserver.Client
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Flags]
    internal enum MARunData : uint
    {
        BFRUNDATA_ID = 1,
        BFRUNDATA_NAME = 2,
        BFRUNDATA_MA = 4,
        BFRUNDATA_RUNCONFIGURATION = 8,
        BFRUNDATA_VERSION = 0x10,
        BFRUNDATA_CREATION_TIME = 0x20,
        BFRUNDATA_MODIFICATION_TIME = 0x40,
        BFRUNDATA_HR = 0x80,
        BFRUNDATA_INVALIDPARTITION = 0x100,
        BFRUNDATA_ALLBITS = BFRUNDATA_ID | BFRUNDATA_NAME | BFRUNDATA_MA | BFRUNDATA_RUNCONFIGURATION | BFRUNDATA_VERSION | 
            BFRUNDATA_CREATION_TIME | BFRUNDATA_MODIFICATION_TIME | BFRUNDATA_HR | BFRUNDATA_INVALIDPARTITION
    }
}
