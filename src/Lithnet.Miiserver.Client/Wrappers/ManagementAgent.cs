using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

namespace Lithnet.Miiserver.Client
{
    public class ManagementAgent : ManagementAgentBase
    {
        private IReadOnlyList<MAImportFlowSet> importFlows;

        protected MMSWebService WebService = new MMSWebService();

        public MAStatistics Statistics
        {
            get
            {
                string result = this.WebService.GetMAStatistics(this.ID.ToMmsGuid(), out string lastRunXml, out uint mvObjectCount);

                SyncServer.ThrowExceptionOnReturnError(result);

                XmlDocument d = new XmlDocument();
                d.LoadXml(result);

                return new MAStatistics(d.SelectSingleNode("total-summary"));
            }
        }


        protected object InvokeWmi(string method, params object[] arguments)
        {
            try
            {
                using (ManagementObject wmiObject = ManagementAgent.GetManagementAgentWmiObject(this.ID))
                {
                    return wmiObject.InvokeMethod(method, arguments);
                }
            }
            catch (COMException ex)
            {
                throw new MiiserverException(SyncServer.TranslateCOMException(ex), ex);
            }
        }

        public XmlNode GetPrivateData()
        {
            return this.XmlNode.SelectSingleNode("private-configuration");
        }

        private XmlNode GetMaData()
        {
            return this.GetMaData(this.ID);
        }


        public void Refresh()
        {
            this.Reload(this.GetMaData());
            this.importFlows = null;
            this.ClearCache();
        }

        public string ExecuteRunProfileNative(string runProfileName)
        {
            string result = this.WebService.RunMA(this.ID.ToMmsGuid(), this.GetRunConfiguration(runProfileName), false);
            SyncServer.ThrowExceptionOnReturnError(result);
            return result;
        }

        protected string GetRunConfiguration(string runProfileName)
        {
            XmlNode madata = this.GetMaData(this.ID,
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

        internal static XmlNode GetMaData(MMSWebService ws, Guid id, MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            string result = ws.GetMaData(id.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata);
            SyncServer.ThrowExceptionOnReturnError(result);

            XmlDocument d = new XmlDocument();
            d.LoadXml(result);
            return d.SelectSingleNode("/ma-data");
        }

        internal static XmlNode GetMaData(MMSWebService ws, Guid id)
        {
            return ManagementAgent.GetMaData(ws, id, MAData.MA_ALLBITS, MAPartitionData.BFPARTITION_ALL, MARunData.BFRUNDATA_ALLBITS);
        }

        internal XmlNode GetMaData(Guid id, MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            return ManagementAgent.GetMaData(this.WebService, id, madata, partitionData, rundata);
        }

        internal XmlNode GetMaData(Guid id)
        {
            return ManagementAgent.GetMaData(this.WebService, id, MAData.MA_ALLBITS, MAPartitionData.BFPARTITION_ALL, MARunData.BFRUNDATA_ALLBITS);
        }

        protected ManagementAgent(XmlNode node, Guid id)
            : base(node, id)
        {
            this.Refresh();
        }

        public static ManagementAgent GetManagementAgent(Guid id)
        {
            MMSWebService ws = new MMSWebService();
            XmlNode node = ManagementAgent.GetMaData(ws, id);
            return new ManagementAgent(node, id);
        }

        public static ManagementAgent GetManagementAgent(string name)
        {
            return ManagementAgent.GetManagementAgent(ManagementAgent.MANameToID(name));
        }

        public static IEnumerable<ManagementAgent> GetManagementAgents()
        {
            foreach (KeyValuePair<Guid, string> k in GetManagementAgentNameAndIDPairs())
            {
                yield return ManagementAgent.GetManagementAgent(k.Key);

            }
        }

        public static Guid MANameToID(string name)
        {
            Guid id = ManagementAgent.GetManagementAgentNameAndIDPairs().FirstOrDefault(t => string.Equals(t.Value, name, StringComparison.CurrentCultureIgnoreCase)).Key;

            if (id == Guid.Empty)
            {
                throw new InvalidOperationException($"Management agent {name} was not found");
            }

            return id;
        }

        /// <summary>
        /// Gets the current ID of a management based on its last known name or ID. This function can find management agents that have been renamed, or that have been recreated with a new ID
        /// </summary>
        /// <param name="name">The last known name of the management agent</param>
        /// <param name="id">The last known ID of the management agent</param>
        /// <returns>The current ID of the management agent, or null if the management agent could not be found</returns>
        public static Guid? FindManagementAgentID(string name, Guid id)
        {
            var mapping = GetManagementAgentNameAndIDPairs();

            foreach (KeyValuePair<Guid, string> k in mapping)
            {
                if (id == k.Key)
                {
                    return k.Key;
                }

                if (string.Equals(name, k.Value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return k.Key;
                }
            }

            return null;
        }

        public static Dictionary<Guid, string> GetManagementAgentNameAndIDPairs()
        {
            Dictionary<Guid, string> mapping = new Dictionary<Guid, string>();

            MMSWebService ws = new MMSWebService();

            ws.GetMAGuidList(out ArrayList ids, out ArrayList names);

            if (names.Count != ids.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(ids), "The sync engine returned a mis-matched number of management agents in the name-to-id mapping data");
            }

            for (int i = 0; i < ids.Count; i++)
            {
                mapping.Add(new Guid((string)ids[i]), (string)(names[i]));
            }

            return mapping;
        }

        public bool IsIdle()
        {
            return this.WebService.IsMAIdle(this.ID.ToMmsGuid());
        }

        public void Wait()
        {
            while (!this.IsIdle())
            {
                Thread.Sleep(1000);
            }
        }

        public void Wait(CancellationToken token)
        {
            while (!this.IsIdle())
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        public string Status
        {
            get
            {
                if (this.IsIdle())
                {
                    return null;
                }
                else
                {
                    return this.GetLastRun()?.LastStepStatus;
                }
            }
        }

        public string ExecutingRunProfileName
        {
            get
            {
                if (this.IsIdle())
                {
                    return null;
                }
                else
                {
                    return this.GetLastRun()?.RunProfileName;
                }
            }
        }

        public void Stop()
        {
            string result = this.WebService.StopMA(this.ID.ToMmsGuid());
            SyncServer.ThrowExceptionOnReturnError(result);
        }

        public async Task StopAsync()
        {
            await Task.Run(() => this.Stop());
        }

        public void SuppressFullSyncWarning()
        {
            this.InvokeWmi("SuppressFullSyncWarning");
        }

        public string ExecuteRunProfile(string name)
        {
            string result = this.InvokeWmi("Execute", name) as string;

            if (result != "success" && !result.StartsWith("completed-"))
            {
                throw new MAExecutionException(result);
            }

            return result;
        }

        public string ExecuteRunProfile(string name, bool resumeLastRun)
        {
            string result = this.InvokeWmi("Execute", name, resumeLastRun) as string;

            if (result != "success" && !result.StartsWith("completed-"))
            {
                throw new MAExecutionException(result);
            }

            return result;
        }

        public async Task<string> ExecuteRunProfileAsync(string name, bool resumeLastRun)
        {
            return await Task.FromResult(this.ExecuteRunProfile(name, resumeLastRun));
        }

        public async Task<string> ExecuteRunProfileAsync(string name)
        {
            return await Task.FromResult(this.ExecuteRunProfile(name));
        }

        public string ExecuteRunProfile(string name, bool resumeLastRun, CancellationToken waitCancellationToken)
        {
            Task<string> t = this.ExecuteRunProfileAsync(name, resumeLastRun);
            try
            {
                t.Wait(waitCancellationToken);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Count == 1)
                {
                    throw e.InnerExceptions.First();
                }
                else
                {
                    throw;
                }
            }

            if (t.IsCanceled)
            {
                return "cancelled";
            }
            else
            {
                if (t.IsFaulted)
                {
                    throw t.Exception.InnerExceptions.First();
                }
                else
                {
                    return t.Result;
                }
            }
        }

        public string ExecuteRunProfile(string name, CancellationToken waitCancellationToken)
        {
            Task<string> t = this.ExecuteRunProfileAsync(name);
            try
            {
                t.Wait(waitCancellationToken);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Count == 1)
                {
                    throw e.InnerExceptions.First();
                }
                else
                {
                    throw;
                }
            }

            if (t.IsCanceled)
            {
                return "cancelled";
            }
            else
            {
                if (t.IsFaulted)
                {
                    throw t.Exception.InnerExceptions.First();
                }
                else
                {
                    return t.Result;
                }
            }
        }

        public CSObjectBase GetCSObject(string dn)
        {
            string search = $"<searching><dn recursive=\"false\">{dn.EscapeXmlElementText()}</dn></searching>";
            return this.GetSingleCSObject(search);
        }
        
        public CSObjectEnumerator GetCSObjects(string dn, bool searchSubTree)
        {
            string search = $"<searching><dn recursive=\"{searchSubTree.ToString().ToLower()}\">{dn.EscapeXmlElementText()}</dn></searching>";
            return this.ExecuteCSSearch(search);
        }

        public CSObjectEnumerator GetCSObjects(string rdn)
        {
            string search = $"<searching><rdn>{rdn.EscapeXmlElementText()}</rdn></searching>";
            return this.ExecuteCSSearch(search);
        }

        public CSObjectBase GetCSObject(Guid id)
        {
            return CSObjectExtensions.GetCSObject(id);
        }

        internal CSObjectEnumerator GetPendingImports(bool getAdds, bool getUpdates, bool getDeletes, int pageSize, CSObjectParts csParts, uint entryParts)
        {
            if (!(getAdds | getUpdates | getDeletes))
            {
                throw new ArgumentException("At least one change type must be specified");
            }

            string searchText = $"<criteria><pending-import add=\"{getAdds.ToString().ToLower()}\" modify=\"{getUpdates.ToString().ToLower()}\" delete=\"{getDeletes.ToString().ToLower()}\"></pending-import></criteria>";

            return this.ExportConnectorSpace(searchText, pageSize, csParts, entryParts);
        }

        public CSObjectEnumerator GetPendingImports(bool getAdds, bool getUpdates, bool getDeletes, int pageSize)
        {
            return this.GetPendingImports(getAdds, getUpdates, getDeletes, pageSize, CSObjectParts.AllItems, 0xffffffff);

        }

        public CSObjectEnumerator GetPendingExports(bool getAdds, bool getUpdates, bool getDeletes, int pageSize)
        {
            return this.GetPendingExports(getAdds, getUpdates, getDeletes, pageSize, CSObjectParts.AllItems, 0xffffffff);
        }

        internal CSObjectEnumerator GetPendingExports(bool getAdds, bool getUpdates, bool getDeletes, int pageSize, CSObjectParts csParts, uint entryParts)
        {
            if (!(getAdds | getUpdates | getDeletes))
            {
                throw new ArgumentException("At least one change type must be specified");
            }

            string searchText = $"<criteria><pending-export add=\"{getAdds.ToString().ToLower()}\" modify=\"{getUpdates.ToString().ToLower()}\" delete=\"{getDeletes.ToString().ToLower()}\"></pending-export></criteria>";

            return this.ExportConnectorSpace(searchText, pageSize, csParts, entryParts);
        }

        public CSObjectEnumerator GetPendingExportCSObjectIDAndPartitions()
        {
            return this.GetPendingExports(true, true, true, 10, CSObjectParts.ManagementAgentPartitionID, 0);
        }

        public CSObjectEnumerator GetPendingImportCSObjectIDAndPartitions()
        {
            return this.GetPendingImports(true, true, true, 10, CSObjectParts.ManagementAgentPartitionID, 0);
        }
        
        public CSObjectEnumerator GetImportErrors()
        {
            string searchText = "<criteria><import-error>true</import-error></criteria>";

            return this.ExportConnectorSpace(searchText);
        }

        public CSObjectEnumerator GetExportErrors()
        {
            string searchText = "<criteria><export-error>true</export-error></criteria>";

            return this.ExportConnectorSpace(searchText);
        }

        public CSObjectEnumerator GetDisconnectors(ConnectorState state)
        {
            string type;

            switch (state)
            {
                case ConnectorState.Normal:
                    type = "normal";
                    break;

                case ConnectorState.Explicit:
                    type = "explicit";
                    break;

                case ConnectorState.Filtered:
                    type = "stay";
                    break;

                default:
                    throw new InvalidOperationException("Unknown connector type");
            }

            string searchText = $"<criteria><connector>false</connector><connector-state>{type}</connector-state></criteria>";
            return this.ExportConnectorSpace(searchText);
        }

        public CSObjectEnumerator GetConnectors(ConnectorState state)
        {
            string type;

            switch (state)
            {
                case ConnectorState.Normal:
                    type = "normal";
                    break;

                case ConnectorState.Explicit:
                    type = "explicit";
                    break;

                case ConnectorState.Filtered:
                default:
                    throw new InvalidOperationException("Unsupported connector type");
            }

            string searchText = $"<criteria><connector>true</connector><connector-state>{type}</connector-state></criteria>";
            return this.ExportConnectorSpace(searchText);
        }

        public CSObjectEnumerator GetDisconnectors()
        {
            string searchText = "<criteria><connector>false</connector></criteria>";
            return this.ExportConnectorSpace(searchText);
        }
        
        public CSObjectEnumerator GetConnectors()
        {
            string searchText = "<criteria><connector>true</connector></criteria>";
            return this.ExportConnectorSpace(searchText);
        }
        
        public bool HasPendingExports()
        {
            using (CSObjectEnumerator e = this.GetPendingExports(true, true, true, 10, 0, 0))
            {
                return e.BatchCount > 0;
            }
        }

        public bool HasPendingImports()
        {
            using (CSObjectEnumerator e = this.GetPendingImports(true, true, true, 10, 0, 0))
            {
                return e.BatchCount > 0;
            }
        }

        public IEnumerable<RunSummary> GetRunSummary()
        {
            ulong ts = 0;

            string result = this.WebService.GetExecSummary(ref ts, out _, out _);
            SyncServer.ThrowExceptionOnReturnError(result);

            return RunSummary.GetRunSummary(result, this.ID);
        }

        public RunDetails GetRunDetail(RunSummary summary)
        {
            return this.GetRunDetail(summary.RunNumber);
        }

        public RunDetails GetRunDetail(int runNumber)
        {
            string query = $"<execution-history-req ma=\"{this.ID.ToMmsGuid()}\"><run-number>{runNumber}</run-number></execution-history-req>";
            string result = this.WebService.GetExecutionHistory(query);
            SyncServer.ThrowExceptionOnReturnError(result);

            if (result != null)
            {
                return RunDetails.GetRunDetails(result).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<RunDetails> GetRunHistory(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Run history count must be greater than zero");
            }

            string query = $"<execution-history-req ma=\"{this.ID.ToMmsGuid()}\"><num-req>{count}</num-req></execution-history-req>";

            string result = this.WebService.GetExecutionHistory(query);
            SyncServer.ThrowExceptionOnReturnError(result);

            if (result != null)
            {
                return RunDetails.GetRunDetails(result);
            }
            else
            {
                return null;
            }
        }

        public RunDetails GetLastRun()
        {
            string query = $"<execution-history-req ma=\"{this.ID.ToMmsGuid()}\"><num-req>1</num-req></execution-history-req>";

            string result = this.WebService.GetExecutionHistory(query);
            SyncServer.ThrowExceptionOnReturnError(result);

            if (result != null)
            {
                return RunDetails.GetRunDetails(result).FirstOrDefault();
            }

            return null;
        }

        public override string ToString()
        {
            return this.Name;
        }

        internal string ExportManagementAgent(bool includeIafs, string timestamp)
        {
            string result = this.WebService.ExportManagementAgent(this.Name, true, includeIafs, timestamp);
            SyncServer.ThrowExceptionOnReturnError(result);
            return result;
        }

        public string ExportManagementAgent()
        {
            return this.ExportManagementAgent(true, DateTime.Now.ToMmsDateString());
        }

        public void ExportManagementAgent(string file)
        {
            this.ExportManagementAgent(file, true);
        }

        public void ExportManagementAgent(string file, bool includeIafs)
        {
            this.ExportManagementAgent(file, includeIafs, DateTime.Now.ToMmsDateString());
        }

        internal void ExportManagementAgent(string file, bool includeIafs, string timestamp)
        {
            string data = this.ExportManagementAgent(includeIafs, timestamp);
            System.IO.File.WriteAllText(file, data);
        }

        private XmlNode GetMaData(MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            string result = this.WebService.GetMaData(this.ID.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata);
            SyncServer.ThrowExceptionOnReturnError(result);

            XmlDocument d = new XmlDocument();
            d.LoadXml(result);
            return d.SelectSingleNode("/");
        }
        
        private CSObjectEnumerator ExportConnectorSpace(string critieria, int pageSize = 10, CSObjectParts csParts = CSObjectParts.AllItems, uint entryParts = 0xFFFFFFFF)
        {
            string token = this.WebService.ExportConnectorSpace(this.Name, critieria, true);
            SyncServer.ThrowExceptionOnReturnError(token);

            return new CSObjectEnumerator(this.WebService, token, true, pageSize, csParts, entryParts);
        }

        private CSObjectBase GetSingleCSObject(string criteria)
        {
            using (CSObjectEnumerator x = this.ExecuteCSSearch(criteria))
            {
                if (x.BatchCount == 0)
                {
                    return null;
                }
                else if (x.BatchCount > 1)
                {
                    throw new InvalidOperationException("The search returned too many results");
                }
                else
                {
                    return x.First();
                }
            }
        }

        private CSObjectEnumerator ExecuteCSSearch(string criteria, int pageSize = 10, CSObjectParts csParts = CSObjectParts.AllItems, uint entryParts = 0xFFFFFFFF)
        {
            string token = this.WebService.ExecuteCSSearch(this.ID.ToMmsGuid(), criteria);
            SyncServer.ThrowExceptionOnReturnError(token);

            return new CSObjectEnumerator(this.WebService, token, false, pageSize, csParts, entryParts);
        }

        private static ManagementObject GetManagementAgentWmiObject(string id)
        {
            ObjectQuery query = new ObjectQuery($"SELECT * FROM MIIS_ManagementAgent where Guid='{id}'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.Scope, query);
            ManagementObjectCollection results = searcher.Get();

            if (results.Count == 0)
            {
                return null;
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
    }
}

