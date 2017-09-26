using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class Partition : XmlObjectBase
    {
        internal Partition(XmlNode node)
            : base(node)
        {
        }

        public Guid ID => this.GetValue<Guid>("id");

        public string Name => this.GetValue<string>("name");

        public DateTime CreationTime => this.GetValue<DateTime>("creation-time");

        public DateTime LastModificationTime => this.GetValue<DateTime>("last-modification-time");

        public int Version => this.GetValue<int>("version");

        public bool Selected => this.GetValue<int>("selected") == 1;

        public int AllowedOperations => this.GetValue<int>("allowed-operations");

        public int CurrentBatchNumber => this.GetValue<int>("current/batch-number");

        public int LastSuccessfulBatch => this.GetValue<int>("last-successful-batch");

        public int CurrentSequenceNumber => this.GetValue<int>("current/sequence-number");

        public PartitionFilter Filter => this.GetObject<PartitionFilter>("filter");

        public PartitionPasswordSyncSettings PasswordSyncSettings => this.GetObject<PartitionPasswordSyncSettings>("password-sync");

        public XmlNode CustomData()
        {
            return this.XmlNode.SelectSingleNode("custom-data");
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
