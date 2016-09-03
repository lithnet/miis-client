using System;

namespace Lithnet.Miiserver.Client
{
    [Flags]
    internal enum CSObjectParts : ulong
    {
        UnappliedExport = 0x1L,
        EscrowedExport = 0x2L,
        UnconfirmedExport = 0x4L,
        PendingImport = 0x8L,

        SynchronizedHologram = 0x10L,
        ImportHologram = 0x20L,
        ExportHologram = 0x40L,
        IsConnector = 0x80L,

        ConnectorState = 0x100L,
        IsSeenByImport = 0x200L,
        IsPlaceholderParent = 0x400L,
        IsPlaceholderLink = 0x800L,

        IsPlaceholderDelete = 0x1000L,
        PendingRefRetry = 0x2000L,
        IsPending = 0x4000L,
        PendingRenameRetry = 0x8000L,

        Sequences = 0x10000L,
        ManagementAgentID = 0x20000L,
        ManagementAgentName = 0x40000L,
        ManagementAgentPartitionID = 0x80000L,

        ImportErrorDetail = 0x100000L,
        ExportErrorDetail = 0x200000L,
        MetaverseLink = 0x400000L,
        //unknown element  = 0x800000L,

        EscrowedExportHologram = 0x1000000L,
        UnconfirmedExportHologram = 0x2000000L,
        DisconnectorInfo = 0x4000000L,
        LastImportDeltaTime = 0x8000000L,

        LastExportDeltaTime = 0x10000000L,
        ManagementAgentPartitionName = 0x20000000L,
        FullyQualifiedDomianName = 0x40000000L,
        DomainName = 0x80000000L,

        AccountName = 0x100000000L,
        UserPrincipalName = 0x200000000L,
        ManagementAgentPartitionDisplayName = 0x400000000L,
        PasswordChangeHistory = 0x800000000L,

        // unknown element = 0x1000000000L,
        RebuildInProgress = 0x2000000000L,
        PendingRefDelete = 0x4000000000L,
        Obsoletion = 0x8000000000L,

        NeedFullSync = 0x10000000000L,
        ImportDeltaOperation = 0x20000000000L,
        ExportDeltaOperation = 0x40000000000L,
        Anchor = 0x80000000000L,

        AllItems = 0xFFFFFFFFFFFFFFFF,
    }
}