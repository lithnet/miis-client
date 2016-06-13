using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Data;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;

namespace Lithnet.Miiserver.Client
{
    public abstract class ManagementAgentBase : NodeCache
    {
        protected static MMSWebService ws = new MMSWebService();

        //protected ManagementObject wmiobject;

        private IReadOnlyList<MAImportFlowSet> importFlows;

        protected ManagementAgentBase(XmlNode node, Guid id)
            : base(node)

        {
            this.ID = id;
            //this.wmiobject = ManagementAgent.GetManagementAgentWmiObject(this.ID);
            this.Refresh();
        }

        public MAStatistics Statistics
        {
            get
            {
                string lastRunXml;
                uint mvObjectCount;

                XmlDocument d = new XmlDocument();
                d.LoadXml(ws.GetMAStatistics(this.ID.ToMmsGuid(), out lastRunXml, out mvObjectCount));

                return new MAStatistics(d.SelectSingleNode("total-summary"));
            }
        }

        public string Name
        {
            get
            {
                return this.GetValue<string>("name");
            }
        }

        public string Description
        {
            get
            {
                return this.GetValue<string>("description");
            }
        }

        public Guid ID { get; private set; }

        public string ListName
        {
            get
            {
                return this.GetValue<string>("ma-listname");
            }
        }

        public string Category
        {
            get
            {
                return this.GetValue<string>("category");
            }
        }

        public string SubType
        {
            get
            {
                return this.GetValue<string>("subtype");
            }
        }

        public string CompanyName
        {
            get
            {
                return this.GetValue<string>("ma-companyname");
            }
        }

        public string Version
        {
            get
            {
                return this.GetValue<string>("version");
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return this.GetValue<DateTime>("creation-time");
            }
        }

        public DateTime LastModificationTime
        {
            get
            {
                return this.GetValue<DateTime>("last-modification-time");
            }
        }

        public bool PasswordSyncAllowed
        {
            get
            {
                return this.GetValue<string>("password-sync-allowed") == "1";
            }
        }

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

        public IReadOnlyList<ExportFlowSet> ExportAttributeFlows
        {
            get
            {
                return this.GetReadOnlyObjectList<ExportFlowSet>("export-attribute-flow/export-flow-set");
            }
        }

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

        private DsmlSchema schema;

        public DsmlSchema Schema
        {
            get
            {
                return this.GetObject<DsmlSchema>("schema/dsml:dsml");
            }
        }

        public PasswordSyncSettings PasswordSyncTargetSettings
        {
            get
            {
                return this.PasswordSyncAllowed ? this.GetObject<PasswordSyncSettings>("password-sync") : null;
            }
        }


        public IReadOnlyList<string> SelectedAttributes
        {
            get
            {
                return this.GetReadOnlyValueList<string>("attribute-inclusion/attribute");
            }
        }

        public IReadOnlyDictionary<string, JoinProfile> JoinRules
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, JoinProfile>("join/join-profile", t => t.ObjectType, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string DeprovisioningAction
        {
            get
            {
                return this.GetValue<string>("provisioning-cleanup/action");
            }
        }

        public string DeprovisioningActionType
        {
            get
            {
                return this.GetValue<string>("provisioning-cleanup/@type");
            }
        }

        public string RulesExtension
        {
            get
            {
                return this.GetValue<string>("extension/assembly-name");
            }
        }

        public bool RulesExtensionInSeperateProcess
        {
            get
            {
                return this.GetValue<string>("extension/application-protection") != "low";
            }
        }

        public string MAArchitecture
        {
            get
            {
                return this.GetValue<string>("controller-configuration/application-architecture");
            }
        }

        public bool MAInSeperateProcess
        {
            get
            {
                return this.GetValue<string>("controller-configuration/application-protection") != "low";
            }
        }

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
            return this.node.SelectSingleNode("private-configuration");
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
                List<MAImportFlow> importFlows = new List<MAImportFlow>();

                string mvObjectType = n2.SelectSingleNode("@mv-object-type").InnerText;

                foreach (XmlNode n3 in n2.SelectNodes("import-flows"))
                {
                    string mvAttribute = n3.SelectSingleNode("@mv-attribute").InnerText;

                    foreach (XmlNode n4 in n3.SelectNodes(string.Format("import-flow[@src-ma='{0}']", this.ID.ToMmsGuid())))
                    {
                        MAImportFlow f = new MAImportFlow(n4, mvObjectType, mvAttribute);
                        importFlows.Add(f);
                    }
                }

                foreach (IGrouping<string, MAImportFlow> g in importFlows.GroupBy(t => t.CSObjectType))
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
            return ws.RunMA(this.ID.ToMmsGuid(), this.GetRunConfiguration(runProfileName), false);
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


            XmlNode node = madata.SelectSingleNode(string.Format("/ma-data/ma-run-data/run-configuration[name='{0}']", runProfileName));


            if (node == null)
            {
                throw new InvalidOperationException("No such run profile " + runProfileName);
            }


            return node.OuterXml;
        }


        private static ManagementObject GetManagementAgentWmiObject(Guid id)
        {
            ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM MIIS_ManagementAgent where Guid='{0}'", id.ToMmsGuid()));

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.scope, query))
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
            XmlDocument d = new XmlDocument();
            d.LoadXml(ws.GetMaData(id.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata));
            return d.SelectSingleNode("/ma-data");
        }

        internal static XmlNode GetMaData(Guid id)
        {
            return ManagementAgentBase.GetMaData(id, MAData.MA_ALLBITS, MAPartitionData.BFPARTITION_ALL, MARunData.BFRUNDATA_ALLBITS);
        }

        public void Refresh()
        {
            this.node = this.GetMaData();
            this.importFlows = null;
            this.ClearCache();
        }
    }
}