using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class StagingCounters : XmlObjectBase
    {
        internal StagingCounters(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets information about the number of imported entries that were not changed.
        /// </summary>
        public int StageNoChange => this.GetValue<int>("stage-no-change");

        /// <summary>
        /// Gets information about the number of imported entries that were added.
        /// </summary>
        public int StageAdd => this.GetValue<int>("stage-add");

        /// <summary>
        /// Gets information about the number of imported entries that were updated.
        /// </summary>
        public int StageUpdate => this.GetValue<int>("stage-update");

        /// <summary>
        /// Gets information about the number of imported entries that were renamed.
        /// </summary>
        public int StageRename => this.GetValue<int>("stage-rename");

        /// <summary>
        /// Gets information about the number of imported entries that were deleted.
        /// </summary>
        public int StageDelete => this.GetValue<int>("stage-delete");

        /// <summary>
        /// Gets information about the number of imported entries that were deleted then added.
        /// </summary>
        public int StageDeleteAdd => this.GetValue<int>("stage-delete-add");

        /// <summary>
        /// Gets information about the number of import entry failures.
        /// </summary>
        public int StageFailure => this.GetValue<int>("stage-failure");

        public bool HasChanges => this.StagingChanges > 0;

        public int StagingChanges => this.StageAdd + this.StageDelete + this.StageDeleteAdd + this.StageRename + this.StageUpdate;
    }
}
