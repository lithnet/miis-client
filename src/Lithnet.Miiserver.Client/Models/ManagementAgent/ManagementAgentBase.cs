using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Xml;
using System.Runtime.InteropServices;

namespace Lithnet.Miiserver.Client
{
    public abstract class ManagementAgentBase : XmlObjectBase
    {
        protected static MMSWebService WebService = new MMSWebService();
        
        private IReadOnlyList<MAImportFlowSet> importFlows;

        protected ManagementAgentBase(XmlNode node, Guid id)
            : base(node)

        {
            this.ID = id;
            this.Refresh();
        }

        public MAStatistics Statistics
        {
            get
            {
                string lastRunXml;
                uint mvObjectCount;

                string result = ManagementAgentBase.WebService.GetMAStatistics(this.ID.ToMmsGuid(), out lastRunXml, out mvObjectCount);

                SyncServer.ThrowExceptionOnReturnError(result);

                XmlDocument d = new XmlDocument();
                d.LoadXml(result);

                return new MAStatistics(d.SelectSingleNode("total-summary"));
            }
        }

        public string Name => this.GetValue<string>("name");

        public string Description => this.GetValue<string>("description");

        public Guid ID { get; private set; }

        public string ListName => this.GetValue<string>("ma-listname");

        public string Category => this.GetValue<string>("category");

        public string SubType => this.GetValue<string>("subtype");

        public string CompanyName => this.GetValue<string>("ma-companyname");

        public string Version => this.GetValue<string>("version");

        public DateTime CreationTime => this.GetValue<DateTime>("creation-time");

        public DateTime LastModificationTime => this.GetValue<DateTime>("last-modification-time");

        public bool PasswordSyncAllowed => this.GetValue<string>("password-sync-allowed") == "1";

        public IReadOnlyDictionary<string, FilterSet> ConnectorFilterRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, FilterSet>("stay-disconnector/filter-set", t => t.CDObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public IReadOnlyDictionary<string, ProjectionClassMapping> ProjectionRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, ProjectionClassMapping>("projection/class-mapping", t => t.CDObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public IReadOnlyList<ExportFlowSet> ExportAttributeFlows => this.GetReadOnlyObjectList<ExportFlowSet>("export-attribute-flow/export-flow-set");

        public IReadOnlyList<MAImportFlowSet> ImportAttributeFlows
        {
            get
            {
                if (this.importFlows == null)
                {
                    this.importFlows = this.GetImportFlows();
                }

                return this.importFlows;
            }
        }

        public DsmlSchema Schema => this.GetObject<DsmlSchema>("schema/dsml:dsml");

        public PasswordSyncSettings PasswordSyncTargetSettings => this.PasswordSyncAllowed ? this.GetObject<PasswordSyncSettings>("password-sync") : null;


        public IReadOnlyList<string> SelectedAttributes => this.GetReadOnlyValueList<string>("attribute-inclusion/attribute");

        public IReadOnlyDictionary<string, JoinProfile> JoinRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, JoinProfile>("join/join-profile", t => t.ObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string DeprovisioningAction => this.GetValue<string>("provisioning-cleanup/action");

        public string DeprovisioningActionType => this.GetValue<string>("provisioning-cleanup/@type");

        public string RulesExtension => this.GetValue<string>("extension/assembly-name");

        public bool RulesExtensionInSeperateProcess => this.GetValue<string>("extension/application-protection") != "low";

        public string MAArchitecture => this.GetValue<string>("controller-configuration/application-architecture");

        public bool MAInSeperateProcess => this.GetValue<string>("controller-configuration/application-protection") != "low";

        public IReadOnlyDictionary<string, RunConfiguration> RunProfiles
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, RunConfiguration>("ma-run-data/run-configuration", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public XmlNode GetPrivateData()
        {
            return this.XmlNode.SelectSingleNode("private-configuration");
        }

        private XmlNode GetMaData()
        {
            return ManagementAgentBase.GetMaData(this.ID);
        }

        private IReadOnlyList<MAImportFlowSet> GetImportFlows()
        {
            List<MAImportFlowSet> sets = new List<MAImportFlowSet>();

            XmlNode n1 = SyncServer.GetImportAttributeFlows();

            if (n1 == null)
            {
                return sets.AsReadOnly();
            }

            foreach (XmlNode n2 in n1.SelectNodes("import-flow-set"))
            {
                List<MAImportFlow> flows = new List<MAImportFlow>();

                string mvObjectType = n2.SelectSingleNode("@mv-object-type").InnerText;

                foreach (XmlNode n3 in n2.SelectNodes("import-flows"))
                {
                    string mvAttribute = n3.SelectSingleNode("@mv-attribute").InnerText;

                    foreach (XmlNode n4 in n3.SelectNodes(string.Format("import-flow[@src-ma='{0}']", this.ID.ToMmsGuid())))
                    {
                        MAImportFlow f = new MAImportFlow(n4, mvObjectType, mvAttribute);
                        flows.Add(f);
                    }
                }

                foreach (IGrouping<string, MAImportFlow> g in flows.GroupBy(t => t.CSObjectType))
                {
                    MAImportFlowSet set = new MAImportFlowSet(g.Key, mvObjectType, g.ToList().AsReadOnly());
                    sets.Add(set);
                }
            }

            return sets.AsReadOnly();
        }

        protected object InvokeWmi(string method, params object[] arguments)
        {
            try
            {
                using (ManagementObject wmiObject = ManagementAgentBase.GetManagementAgentWmiObject(this.ID))
                {
                    return wmiObject.InvokeMethod(method, arguments);
                }
            }
            catch (COMException ex)
            {
                throw new MiiserverException(SyncServer.TranslateCOMException(ex), ex);
            }
        }

        public string ExecuteRunProfileNative(string runProfileName)
        {
            string result = ManagementAgentBase.WebService.RunMA(this.ID.ToMmsGuid(), this.GetRunConfiguration(runProfileName), false);
            SyncServer.ThrowExceptionOnReturnError(result);
            return result;
        }

        protected string GetRunConfiguration(string runProfileName)
        {
            XmlNode madata = ManagementAgentBase.GetMaData(this.ID,
                MAData.MA_PARTITION_DATA |
                MAData.MA_RUN_DATA,
                MAPartitionData.BFPARTITION_SELECTED |
                MAPartitionData.BFPARTITION_CUSTOM_DATA |
                MAPartitionData.BFPARTITION_ID |
                MAPartitionData.BFPARTITION_NAME |
                MAPartitionData.BFPARTITION_ALLOWED_OPERATIONS,
                MARunData.BFRUNDATA_NAME |
                MARunData.BFRUNDATA_ID |
                MARunData.BFRUNDATA_VERSION |
                MARunData.BFRUNDATA_RUNCONFIGURATION);


            XmlNode node = madata.SelectSingleNode($"/ma-data/ma-run-data/run-configuration[name='{runProfileName}']");


            if (node == null)
            {
                throw new InvalidOperationException("No such run profile " + runProfileName);
            }
            
            return node.OuterXml;
        }


        private static ManagementObject GetManagementAgentWmiObject(Guid id)
        {
            ObjectQuery query = new ObjectQuery($"SELECT * FROM MIIS_ManagementAgent where Guid='{id.ToMmsGuid()}'");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.Scope, query))
            {
                using (ManagementObjectCollection results = searcher.Get())
                {
                    if (results.Count == 0)
                    {
                        throw new MiiserverException($"The specified management agent ({id}) was not found");
                    }
                    else if (results.Count > 1)
                    {
                        throw new TooManyResultsException();
                    }
                    else
                    {
                        return results.OfType<ManagementObject>().First();
                    }
                }
            }
        }

        internal static XmlNode GetMaData(Guid id, MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            string result = ManagementAgentBase.WebService.GetMaData(id.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata);
            SyncServer.ThrowExceptionOnReturnError(result);

            XmlDocument d = new XmlDocument();
            d.LoadXml(result);
            return d.SelectSingleNode("/ma-data");
        }

        internal static XmlNode GetMaData(Guid id)
        {
            return ManagementAgentBase.GetMaData(id, MAData.MA_ALLBITS, MAPartitionData.BFPARTITION_ALL, MARunData.BFRUNDATA_ALLBITS);
        }

        public void Refresh()
        {
            this.Reload(this.GetMaData());
            this.importFlows = null;
            this.ClearCache();
        }
    }
}