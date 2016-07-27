using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public abstract class CSObjectBase : NodeCache
    {
        protected CSObjectBase(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the security account manager (SAM) account name for the user
        /// </summary>
        public string AccountName
        {
            get
            {
                return this.GetValue<string>("account-name");
            }
        }

        /// <summary>
        /// Gets the distinguished name of the connector space object.
        /// </summary>
        public string DN
        {
            get
            {
                return this.GetValue<string>("@cs-dn");
            }
        }

        /// <summary>
        /// Gets the domain name of the connector space object.
        /// </summary>
        public string DomainName
        {
            get
            {
                return this.GetValue<string>("domain-name");
            }
        }

        /// <summary>
        /// Gets the domain name of the connector space object in domain name system (DNS) format.
        /// </summary>
        public string FullyQualifiedDomainName
        {
            get
            {
                return this.GetValue<string>("fully-qualified-domain-name");
            }
        }

        /// <summary>
        /// Gets the GUID that is used to identify the connector space object in the database.
        /// </summary>
        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("@id");
            }
        }

        /// <summary>
        /// Gets the GUID of the management agent for the connector space object.
        /// </summary>
        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("ma-id");
            }
        }

        /// <summary>
        /// Gets the name of the management agent for the connector space object.
        /// </summary>
        public string MaName
        {
            get
            {
                return this.GetValue<string>("ma-name");
            }
        }

        /// <summary>
        /// Gets the GUID of the metaverse object that is joined to the connector space object.
        /// </summary>
        public Guid? MvGuid
        {
            get
            {
                return this.MetaverseLink?.MVObjectID;
            }
        }

        /// <summary>
        /// Gets the name of the primary object type of the connector space object.
        /// </summary>
        public string ObjectType
        {
            get
            {
                return this.GetValue<string>("@object-type");
            }
        }

        /// <summary>
        /// Gets the GUID of the management agent partition with the connector space object.
        /// </summary>
        public Guid? PartitionGuid
        {
            get
            {
                return this.GetValue<Guid?>("@partition-id");
            }
        }

        /// <summary>
        /// Gets the display name of the management agent partition with the connector space object.
        /// </summary>
        public string PartitionName
        {
            get
            {
                return this.GetValue<string>("partition-name");
            }
        }

        public bool PendingRefDelete
        {
            get
            {
                return this.GetValue<bool>("pending-ref-delete");
            }
        }

        public bool SeenByImport
        {
            get
            {
                return this.GetValue<bool>("seen-by-import");
            }
        }

        public bool IsConnector
        {
            get
            {
                return this.GetValue<bool>("connector");
            }
        }

        public ConnectorState ConnectorState
        {
            get
            {
                return this.GetValue<ConnectorState>("connector-state");
            }
        }

        public bool RebuildInProgress
        {
            get
            {
                return this.GetValue<bool>("rebuild-in-progress");
            }
        }

        public bool Obsoletion
        {
            get
            {
                return this.GetValue<bool>("obsoletion");
            }
        }

        public bool NeedFullSync
        {
            get
            {
                return this.GetValue<bool>("need-full-sync");
            }
        }

        public bool IsPlaceholderParent
        {
            get
            {
                return this.GetValue<bool>("placeholder-parent");
            }
        }

        public bool IsPlaceholderLink
        {
            get
            {
                return this.GetValue<bool>("placeholder-link");
            }
        }

        public bool IsPlaceholderDelete
        {
            get
            {
                return this.GetValue<bool>("placeholder-delete");
            }
        }

        public bool IsPending
        {
            get
            {
                return this.GetValue<bool>("pending");
            }
        }

        public bool PendingReferenceRetry
        {
            get
            {
                return this.GetValue<bool>("ref-retry");
            }
        }

        public bool PendingRenameRetry
        {
            get
            {
                return this.GetValue<bool>("rename-retry");
            }
        }

        public string ImportDeltaOperation
        {
            get
            {
                return this.GetValue<string>("import-delta-operation");
            }
        }

        public string ExportDeltaOperation
        {
            get
            {
                return this.GetValue<string>("export-delta-operation");
            }
        }

        public DateTime? LastImportDeltaTime
        {
            get
            {
                return this.GetValue<DateTime?>("last-import-delta-time");
            }
        }

        public DateTime? LastExportDeltaTime
        {
            get
            {
                return this.GetValue<DateTime?>("last-export-delta-time");
            }
        }

        public string DisconnectionType
        {
            get
            {
                return this.GetValue<string>("disconnection-type");
            }
        }

        public Guid? DisconnectionID
        {
            get
            {
                return this.GetValue<Guid?>("disconnection-id");
            }
        }
         
        public DateTime? DisconnectionTime
        {
            get
            {
                return this.GetValue<DateTime?>("disconnection-time");
            }
        }

        public MVLink MetaverseLink
        {
            get
            {
                return this.GetObject<MVLink>("mv-link");
            }
        }

        public Delta UnappliedExportDelta
        {
            get
            {
                return this.GetObject<Delta>("unapplied-export/delta");
            }
        }

        public Delta EscrowedExportDelta
        {
            get
            {
                return this.GetObject<Delta>("escrowed-export/delta");
            }
        }

        public Delta UnconfirmedExportDelta
        {
            get
            {
                return this.GetObject<Delta>("unconfirmed-export/delta");
            }
        }

        public Delta PendingImportDelta
        {
            get
            {
                return this.GetObject<Delta>("pending-import/delta");
            }
        }

        public Hologram SynchronizedHologram
        {
            get
            {
                return this.GetObject<Hologram>("synchronized-hologram/entry");
            }
        }

        public Hologram UnappliedExportHologram
        {
            get
            {
                return this.GetObject<Hologram>("unapplied-export-hologram/entry");
            }
        }

        public Hologram EscrowedExportHologram
        {
            get
            {
                return this.GetObject<Hologram>("escrowed-export-hologram/entry");
            }
        }

        public Hologram UnconfirmedExportHologram
        {
            get
            {
                return this.GetObject<Hologram>("unconfirmed-export-hologram/entry");
            }
        }

        public Hologram PendingImportHologram
        {
            get
            {
                return this.GetObject<Hologram>("pending-import-hologram/entry");
            }
        }

        public override string ToString()
        {
            return this.DN;
        }
    }
}