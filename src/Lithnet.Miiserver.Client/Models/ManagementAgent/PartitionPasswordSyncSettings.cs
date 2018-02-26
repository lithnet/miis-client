using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class PartitionPasswordSyncSettings : XmlObjectBase
    {
        internal PartitionPasswordSyncSettings(XmlNode node)
            : base(node)
        {
        }

        public bool PasswordSyncSourceEnabled => this.GetValue<int>("password-sync-source-enabled") == 1;

        public IReadOnlyList<Guid> PasswordSyncTargetMAIDs => this.GetReadOnlyValueList<Guid>("password-sync-target-mas/password-sync-target-ma");

        public IEnumerable<ManagementAgent> PasswordSyncTargetMAs => this.PasswordSyncTargetMAIDs.Select(ManagementAgent.GetManagementAgent);

        public bool MaxChangedAllowedEnabled => this.GetValue<int>("max-changes-allowed-enabled") == 1;

        public int MaxChangesAllowed => this.GetValue<int>("max-changes-allowed");

        public IReadOnlyList<string> ObjectClasses => this.GetReadOnlyValueList<string>("object-classes/object-class");

        public IReadOnlyList<string> ContainerExclusions => this.GetReadOnlyValueList<string>("containers/exclusions/exclusion");

        public IReadOnlyList<string> ContainerInclusions => this.GetReadOnlyValueList<string>("containers/inclusions/inclusion");
    }
}
