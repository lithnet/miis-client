﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

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

        public Guid Partition => this.GetValue<Guid>("partition");

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

        internal static IEnumerable<CSObjectRef> GetStepDetailCSObjectRefs(Guid stepID, string queryType)
        {
            return RunStep.GetStepDetailCSObjectRefs(stepID, queryType, new CancellationToken());
        }

        internal static IEnumerable<CSObjectRef> GetStepDetailCSObjectRefs(Guid stepID, string queryType, CancellationToken t)
        {
            MMSWebService ws = new MMSWebService();

            string query = string.Format("<step-object-details-filter step-id='{0}'><statistics type='{1}'/></step-object-details-filter>", stepID.ToMmsGuid(), queryType);
            string token = null;

            try
            {
                token = ws.ExecuteStepObjectDetailsSearch(query);

                while (!t.IsCancellationRequested)
                {
                    int count = 0;

                    string xml = ws.GetStepObjectResults(token, 10);
                    SyncServer.ThrowExceptionOnReturnError(xml);

                    XmlDocument d = new XmlDocument();
                    d.LoadXml(xml);

                    foreach (XmlNode node in d.SelectNodes("step-object-details/cs-object"))
                    {
                        yield return new CSObjectRef(node);

                        count++;
                    }

                    if (count == 0)
                    {
                        break;
                    }
                }
            }
            finally
            {
                if (token != null)
                {
                    ws.ReleaseSessionObjects(new string[] { token });
                }
            }
        }

        /*
         <step-object-details step-id="{7C44FD84-3CA4-4286-A750-3114FE7A0223}">
 <cs-object id="{F650712A-7013-E711-9160-005056B50BB9}" cs-dn="dn1"/>
 <cs-object id="{EAA1C9A7-6F13-E711-9160-005056B50BB9}" cs-dn="dn2"/>
</step-object-details>

             */

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
