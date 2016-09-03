namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class StagingCounters : XmlObjectBase
    {
        internal StagingCounters(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets information about the number of imported entries that were not changed.
        /// </summary>
        public int StageNoChange
        {
            get
            {
                return this.GetValue<int>("stage-no-change");
            }
        }
        /// <summary>
        /// Gets information about the number of imported entries that were added.
        /// </summary>
        public int StageAdd
        {
            get
            {
                return this.GetValue<int>("stage-add");
            }
        }

        /// <summary>
        /// Gets information about the number of imported entries that were updated.
        /// </summary>
        public int StageUpdate
        {
            get
            {
                return this.GetValue<int>("stage-update");
            }
        }

        /// <summary>
        /// Gets information about the number of imported entries that were renamed.
        /// </summary>
        public int StageRename
        {
            get
            {
                return this.GetValue<int>("stage-rename");
            }
        }

        /// <summary>
        /// Gets information about the number of imported entries that were deleted.
        /// </summary>
        public int StageDelete
        {
            get
            {
                return this.GetValue<int>("stage-delete");
            }
        }

        /// <summary>
        /// Gets information about the number of imported entries that were deleted then added.
        /// </summary>
        public int StageDeleteAdd
        {
            get
            {
                return this.GetValue<int>("stage-delete-add");
            }
        }

        /// <summary>
        /// Gets information about the number of import entry failures.
        /// </summary>
        public int StageFailure
        {
            get
            {
                return this.GetValue<int>("stage-failure");
            }
        }

        public bool HasChanges
        {
            get
            {
                return this.StagingChanges > 0;
            }
        }

        public int StagingChanges
        {
            get
            {
                return this.StageAdd + this.StageDelete + this.StageDeleteAdd + this.StageRename + this.StageUpdate;
            }
        }
    }
}
