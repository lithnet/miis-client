﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lithnet.Miiserver.Client
{
    internal enum MAPartitionData : uint
    {
        BFPARTITION_ID = 1,
        BFPARTITION_NAME = 2,
        BFPARTITION_VERSION = 4,
        BFPARTITION_SELECTED = 8,
        BFPARTITION_CUSTOM_DATA = 0x10,
        BFPARTITION_FILTER = 0x20,
        BFPARTITION_FILTER_HINTS = 0x40,
        BFPARTITION_CREATION_TIME = 0x80,
        BFPARTITION_MODIFICATION_TIME = 0x100,
        BFPARTITION_ALLOWED_OPERATIONS = 0x200,
        BFPARTITION_CURRENT_BSEQ = 0x400,
        BFPARTITION_SUCCESSFUL_BATCH = 0x800,
        BFPARTITION_DISPLAYNAME = 0x1000,
        BFPARTITION_CURRENT_BATCH = 0x2000,
        BFPARTITION_CURRENT_SEQ = 0x4000,
        BFPARTITION_PASSWORD_SYNC = 0x8000,
        BFPARTITION_ALL = ((((((((((((((BFPARTITION_ID | BFPARTITION_NAME) | BFPARTITION_VERSION) | BFPARTITION_SELECTED) | BFPARTITION_CUSTOM_DATA) | BFPARTITION_FILTER) | BFPARTITION_FILTER_HINTS) | BFPARTITION_CREATION_TIME) | BFPARTITION_MODIFICATION_TIME) | BFPARTITION_ALLOWED_OPERATIONS) | BFPARTITION_CURRENT_BSEQ) | BFPARTITION_SUCCESSFUL_BATCH) | BFPARTITION_DISPLAYNAME) | BFPARTITION_CURRENT_BATCH) | BFPARTITION_CURRENT_SEQ) | BFPARTITION_PASSWORD_SYNC
    }
}