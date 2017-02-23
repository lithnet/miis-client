using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RunStep : XmlObjectBase
    {
        private RunStepType internalType = 0;

        internal RunStep(XmlNode node)
            : base(node)
        {
        }

        public RunStepType Type
        {
            get
            {
                if (this.internalType == RunStepType.Unknown)
                {
                    this.ResolveType();
                }

                return this.internalType;
            }
        }

        public bool DropAuditFile { get; private set; }

        public bool DropAuditFileOnly { get; private set; }

        public bool ResumeFromAuditFile { get; private set; }

        public int ObjectLimit => this.GetValue<int>("threshold/object");

        public int BatchSize => this.GetValue<int>("threshold/batch-size");

        public string DropFileName => this.GetValue<string>("dropfile-name");

        public string Partition => this.GetValue<string>("partition");

        public bool IsImportStep => this.Type == RunStepType.DeltaImport ||
                                    this.Type == RunStepType.DeltaImportDeltaSynchronization ||
                                    this.Type == RunStepType.FullImport ||
                                    this.Type == RunStepType.FullImportDeltaSynchronization ||
                                    this.Type == RunStepType.FullImportFullSynchronization;

        public bool IsSyncStep => this.Type == RunStepType.DeltaImportDeltaSynchronization ||
                                  this.Type == RunStepType.DeltaSynchronization ||
                                  this.Type == RunStepType.FullImportDeltaSynchronization ||
                                  this.Type == RunStepType.FullImportFullSynchronization ||
                                  this.Type == RunStepType.FullSynchronization;

        public bool IsDeltaSyncStep => this.Type == RunStepType.DeltaImportDeltaSynchronization ||
                                       this.Type == RunStepType.DeltaSynchronization ||
                                       this.Type == RunStepType.FullImportDeltaSynchronization;

        public bool IsFullSyncStep => this.Type == RunStepType.FullImportFullSynchronization ||
                                      this.Type == RunStepType.FullSynchronization;

        public bool IsExportStep => this.Type == RunStepType.Export;

        public bool IsTestRun => this.DropAuditFileOnly || this.ResumeFromAuditFile;

        public bool IsCombinedStep => this.Type == RunStepType.DeltaImportDeltaSynchronization ||
                                      this.Type == RunStepType.FullImportDeltaSynchronization ||
                                      this.Type == RunStepType.FullImportFullSynchronization;

        public string StepTypeDescription
        {
            get
            {
                switch (this.Type)
                {
                    case RunStepType.Export:
                        if (this.DropAuditFileOnly)
                        {
                            return "Export (drop file and stop run)";
                        }
                        else if (this.DropAuditFile)
                        {
                            return "Export (drop audit file)";
                        }
                        else if (this.ResumeFromAuditFile)
                        {
                            return "Export (resume run from file)";
                        }
                        else
                        {
                            return "Export";
                        }

                    case RunStepType.DeltaImport:
                        if (this.DropAuditFileOnly)
                        {
                            return "Delta import (stage only) (drop file and stop run)";
                        }
                        else if (this.DropAuditFile)
                        {
                            return "Delta import (stage only) (drop audit file)";
                        }
                        else if (this.ResumeFromAuditFile)
                        {
                            return "Delta import (stage only) (resume run from file)";
                        }
                        else
                        {
                            return "Delta import";
                        }


                    case RunStepType.FullImport:
                        if (this.DropAuditFileOnly)
                        {
                            return "Full import (stage only) (drop file and stop run)";
                        }
                        else if (this.DropAuditFile)
                        {
                            return "Full import (stage only) (drop audit file)";
                        }
                        else if (this.ResumeFromAuditFile)
                        {
                            return "Full import (stage only) (resume run from file)";
                        }
                        else
                        {
                            return "Full import";
                        }

                    case RunStepType.DeltaSynchronization:
                        return "Delta synchronization";

                    case RunStepType.FullSynchronization:
                        return "Full synchronization";

                    case RunStepType.DeltaImportDeltaSynchronization:
                        if (this.DropAuditFile)
                        {
                            return "Delta import and delta synchronization (drop audit file)";
                        }
                        else
                        {
                            return "Delta import and delta synchronization";
                        }

                    case RunStepType.FullImportDeltaSynchronization:
                        if (this.DropAuditFile)
                        {
                            return "Full import and delta synchronization (drop audit file)";
                        }
                        else
                        {
                            return "Full import and delta synchronization";
                        }


                    case RunStepType.FullImportFullSynchronization:
                        if (this.DropAuditFile)
                        {
                            return "Full import and full synchronization (drop audit file)";
                        }
                        else
                        {
                            return "Full import and full synchronization";
                        }

                    default:
                    case RunStepType.Unknown:
                        return "Unknown operation type";
                }
            }
        }

        public string CustomData => this.XmlNode.SelectSingleNode("custom-data")?.InnerXml;

        public override string ToString()
        {
            return this.StepTypeDescription;
        }

        private void ResolveType()
        {
            string stepType = this.XmlNode.SelectSingleNode("step-type/@type")?.InnerText;

            switch (stepType)
            {
                case "delta-import":
                    this.ResolveDeltaImportType();
                    break;

                case "full-import":
                    this.ResolveFullImportType();
                    break;

                case "export":
                    this.ResolveExportType();
                    break;

                case "apply-rules":
                    this.ResolveSyncType();
                    break;

                case "full-import-reevaluate-rules":
                    this.ResolveFullImportSyncType();
                    break;

                default:
                    break;
            }
        }

        private void ResolveFullImportSyncType()
        {
            this.internalType = RunStepType.FullImportFullSynchronization;

            foreach (XmlNode n2 in this.XmlNode.SelectNodes("step-type/import-subtype"))
            {
                if (n2.InnerText == "to-file")
                {
                    this.DropAuditFile = true;
                    return;
                }
            }
        }

        private void ResolveExportType()
        {
            foreach (XmlNode n2 in this.XmlNode.SelectNodes("step-type/export-subtype"))
            {
                if (n2.InnerText == "to-file")
                {
                    this.DropAuditFile = true;
                }
                else if (n2.InnerText == "resume-from-file")
                {
                    this.ResumeFromAuditFile = true;
                }
            }

            this.DropAuditFileOnly = this.DropAuditFile && !this.ResumeFromAuditFile;

            if (this.ResumeFromAuditFile && this.DropAuditFile)
            {
                this.ResumeFromAuditFile = false;
            }

            this.internalType = RunStepType.Export;
        }

        private void ResolveSyncType()
        {
            string subtype = this.XmlNode.SelectSingleNode("step-type/apply-rules-subtype")?.InnerText;

            if (subtype == "reevaluate-flow-connectors")
            {
                this.internalType = RunStepType.FullSynchronization;
            }
            else if (subtype == "apply-pending")
            {
                this.internalType = RunStepType.DeltaSynchronization;
            }
        }

        private void ResolveFullImportType()
        {
            bool toCS = false;
            foreach (XmlNode n2 in this.XmlNode.SelectNodes("step-type/import-subtype"))
            {
                if (n2.InnerText == "to-cs")
                {
                    toCS = true;
                }
                else if (n2.InnerText == "to-file")
                {
                    this.DropAuditFile = true;
                }
                else if (n2.InnerText == "resume-from-file")
                {
                    this.ResumeFromAuditFile = true;
                }
            }

            this.DropAuditFileOnly = this.DropAuditFile && !this.ResumeFromAuditFile;

            if (this.ResumeFromAuditFile && this.DropAuditFile)
            {
                this.ResumeFromAuditFile = false;
            }

            if (toCS)
            {
                this.internalType = RunStepType.FullImport;
            }
            else
            {
                this.internalType = RunStepType.FullImportDeltaSynchronization;
            }
        }

        private void ResolveDeltaImportType()
        {
            bool toCS = false;
            foreach (XmlNode n2 in this.XmlNode.SelectNodes("step-type/import-subtype"))
            {
                if (n2.InnerText == "to-cs")
                {
                    toCS = true;
                }
                else if (n2.InnerText == "to-file")
                {
                    this.DropAuditFile = true;
                }
                else if (n2.InnerText == "resume-from-file")
                {
                    this.ResumeFromAuditFile = true;
                }
            }

            this.DropAuditFileOnly = this.DropAuditFile && !this.ResumeFromAuditFile;

            if (this.ResumeFromAuditFile && this.DropAuditFile)
            {
                this.ResumeFromAuditFile = false;
            }

            if (toCS)
            {
                this.internalType = RunStepType.DeltaImport;
            }
            else
            {
                this.internalType = RunStepType.DeltaImportDeltaSynchronization;
            }
        }
    }
}
