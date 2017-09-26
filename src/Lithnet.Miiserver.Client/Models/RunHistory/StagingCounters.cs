using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class StagingCounters : XmlObjectBase
    {
        private Guid stepID;

        internal StagingCounters(XmlNode node, Guid stepID)
            : base(node)
        {
            this.stepID = stepID;
        }

        /// <summary>
        /// Gets information about the number of imported entries that were not changed.
        /// </summary>
        public int StageNoChange => this.StageNoChangeDetail.Count;

        public CounterDetail StageNoChangeDetail => this.GetObject<CounterDetail>("stage-no-change", this.stepID);
        
        /// <summary>
        /// Gets information about the number of imported entries that were added.
        /// </summary>
        public int StageAdd => this.StageAddDetail.Count;
        
        public CounterDetail StageAddDetail => this.GetObject<CounterDetail>("stage-add", this.stepID);

        /// <summary>
        /// Gets information about the number of imported entries that were updated.
        /// </summary>
        public int StageUpdate => this.StageUpdateDetail.Count;

        public CounterDetail StageUpdateDetail => this.GetObject<CounterDetail>("stage-update", this.stepID);
        
        /// <summary>
        /// Gets information about the number of imported entries that were renamed.
        /// </summary>
        public int StageRename => this.StageRenameDetail.Count;

        public CounterDetail StageRenameDetail => this.GetObject<CounterDetail>("stage-rename", this.stepID);
        
        /// <summary>
        /// Gets information about the number of imported entries that were deleted.
        /// </summary>
        public int StageDelete => this.StageDeleteDetail.Count;

        public CounterDetail StageDeleteDetail => this.GetObject<CounterDetail>("stage-delete", this.stepID);
        
        /// <summary>
        /// Gets information about the number of imported entries that were deleted then added.
        /// </summary>
        public int StageDeleteAdd => this.StageDeleteAddDetail.Count;

        public CounterDetail StageDeleteAddDetail => this.GetObject<CounterDetail>("stage-delete-add", this.stepID);
        
        /// <summary>
        /// Gets information about the number of import entry failures.
        /// </summary>
        public int StageFailure => this.StageFailureDetail.Count;

        public CounterDetail StageFailureDetail => this.GetObject<CounterDetail>("stage-failure", this.stepID);
        
        public bool HasChanges => this.StagingChanges > 0;

        public int StagingChanges => this.StageAdd + this.StageDelete + this.StageDeleteAdd + this.StageRename + this.StageUpdate;
    }
}
