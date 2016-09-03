using System;
using System.Diagnostics.CodeAnalysis;

namespace Lithnet.Miiserver.Client
{
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal enum MVData
    {
        MV_VERSION = 1,
        MV_SCHEMA = 2,
        MV_IMPORT_ATTR_FLOW = 4,
        MV_PROVISIONING = 8,
        MV_DELETE = 0x10,
        MV_EXTENSION = 0x20,
        MV_FORMATVERSION = 0x40,
        MV_PASSWORDCHANGEHISTORYSIZE = 0x80,
        MV_PASSWORDSYNC = 0x100,
        MV_ALL = MV_VERSION | MV_SCHEMA | MV_IMPORT_ATTR_FLOW | MV_PROVISIONING | MV_DELETE | MV_EXTENSION |
            MV_FORMATVERSION | MV_PASSWORDCHANGEHISTORYSIZE | MV_PASSWORDSYNC
    }
}
