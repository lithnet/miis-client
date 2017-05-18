using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;

namespace Lithnet.Miiserver.Client
{
    public class ManagementAgent : ManagementAgentBase
    {
        protected ManagementAgent(XmlNode node, Guid id)
            : base(node, id)
        {
        }

        private static ManagementAgent GetManagementAgentFromID(Guid id)
        {
            XmlNode node = ManagementAgent.GetMaData(id);
            return new ManagementAgent(node, id);
        }

        public static ManagementAgent GetManagementAgent(string name)
        {
            return ManagementAgent.GetManagementAgentFromID((ManagementAgent.MANameToID(name)));
        }

        public static IEnumerable<ManagementAgent> GetManagementAgents()
        {
            ArrayList names;
            ArrayList ids;
            ManagementAgentBase.WebService.GetMAGuidList(out ids, out names);

            foreach (string id in ids.OfType<string>())
            {
                yield return ManagementAgent.GetManagementAgentFromID(new Guid(id));
            }
        }

        public static Guid MANameToID(string name)
        {
            ArrayList names;
            ArrayList ids;
            ManagementAgentBase.WebService.GetMAGuidList(out ids, out names);

            if (names.Count != ids.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(ids), "The sync engine returned a mis-matched number of management agents in the name-to-id mapping data");
            }

            for (int i = 0; i < ids.Count; i++)
            {
                if (string.Equals(name, (string)names[i], StringComparison.OrdinalIgnoreCase))
                {
                    return new Guid((string)ids[i]);
                }
            }

            throw new InvalidOperationException($"Management agent {name} was not found");
        }

        public bool IsIdle()
        {
            return ManagementAgentBase.WebService.IsMAIdle(this.ID.ToMmsGuid());
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
            string result = ManagementAgentBase.WebService.StopMA(this.ID.ToMmsGuid());
            SyncServer.ThrowExceptionOnReturnError(result);
        }

        public void StopAsync()
        {
            Task t = new Task(this.Stop);

            t.Start();
        }

        public void SuppressFullSyncWarning()
        {
            this.InvokeWmi("SuppressFullSyncWarning");
        }

        public string ExecuteRunProfile(string name)
        {
            string result = this.InvokeWmi("Execute", new object[] { name }) as string;

            if (result != "success" && !result.StartsWith("completed-"))
            {
                throw new MAExecutionException(result);
            }

            return result;
        }

        public string ExecuteRunProfile(string name, bool resumeLastRun)
        {
            string result = this.InvokeWmi("Execute", new object[] { name, resumeLastRun }) as string;

            if (result != "success" && !result.StartsWith("completed-"))
            {
                throw new MAExecutionException(result);
            }

            return result;
        }

        public Task<string> ExecuteRunProfileAsync(string name, bool resumeLastRun)
        {
            Task<string> t = new Task<string>(() => this.ExecuteRunProfile(name, resumeLastRun));

            t.Start();

            return t;
        }

        public Task<string> ExecuteRunProfileAsync(string name)
        {
            Task<string> t = new Task<string>(() => this.ExecuteRunProfile(name));

            t.Start();

            return t;
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

        public CSObject GetCSObject(string dn)
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

        public CSObject GetCSObject(Guid id)
        {
            string search = $"<searching><id>{id.ToMmsGuid()}</id></searching>";
            return this.GetSingleCSObject(search);
        }

        internal CSObjectEnumerator GetPendingImports(bool getAdds, bool getUpdates, bool getDeletes, CSObjectParts csParts, uint entryParts)
        {
            if (!(getAdds | getUpdates | getDeletes))
            {
                throw new ArgumentException("At least one change type must be specified");
            }

            string searchText = $"<criteria><pending-import add=\"{getAdds.ToString().ToLower()}\" modify=\"{getUpdates.ToString().ToLower()}\" delete=\"{getDeletes.ToString().ToLower()}\"></pending-import></criteria>";

            return this.ExportConnectorSpace(searchText, csParts, entryParts);
        }

        public CSObjectEnumerator GetPendingImports(bool getAdds, bool getUpdates, bool getDeletes)
        {
            return this.GetPendingImports(getAdds, getUpdates, getDeletes, CSObjectParts.AllItems, 0xffffffff);

        }

        public CSObjectEnumerator GetPendingExports(bool getAdds, bool getUpdates, bool getDeletes)
        {
            return this.GetPendingExports(getAdds, getUpdates, getDeletes, CSObjectParts.AllItems, 0xffffffff);
        }

        internal CSObjectEnumerator GetPendingExports(bool getAdds, bool getUpdates, bool getDeletes, CSObjectParts csParts, uint entryParts)
        {
            if (!(getAdds | getUpdates | getDeletes))
            {
                throw new ArgumentException("At least one change type must be specified");
            }

            string searchText = $"<criteria><pending-export add=\"{getAdds.ToString().ToLower()}\" modify=\"{getUpdates.ToString().ToLower()}\" delete=\"{getDeletes.ToString().ToLower()}\"></pending-export></criteria>";

            return this.ExportConnectorSpace(searchText, csParts, entryParts);
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
            using (CSObjectEnumerator e = this.GetPendingExports(true, true, true, 0, 0))
            {
                return e.BatchCount > 0;
            }
        }

        public bool HasPendingImports()
        {
            using (CSObjectEnumerator e = this.GetPendingImports(true, true, true, 0, 0))
            {
                return e.BatchCount > 0;
            }
        }

        public IEnumerable<RunSummary> GetRunSummary()
        {
            ulong ts = 0;
            int reload;
            uint returned;

            string result = ManagementAgentBase.WebService.GetExecSummary(ref ts, out reload, out returned);
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
            string result = ManagementAgentBase.WebService.GetExecutionHistory(query);
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
                throw new ArgumentOutOfRangeException("count", "Run history count must be greater than zero");
            }

            string query = $"<execution-history-req ma=\"{this.ID.ToMmsGuid()}\"><num-req>{count}</num-req></execution-history-req>";

            string result = ManagementAgentBase.WebService.GetExecutionHistory(query);
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

            string result = ManagementAgentBase.WebService.GetExecutionHistory(query);
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
            string result = ManagementAgent.WebService.ExportManagementAgent(this.Name, true, includeIafs, timestamp);
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

        public void ExportManagementAgent(string file, bool includeIAFs)
        {
            this.ExportManagementAgent(file, includeIAFs, DateTime.Now.ToMmsDateString());
        }

        internal void ExportManagementAgent(string file, bool includeIAFs, string timestamp)
        {
            string data = this.ExportManagementAgent(includeIAFs, timestamp);
            System.IO.File.WriteAllText(file, data);
        }

        private XmlNode GetMaData(MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            string result = ManagementAgentBase.WebService.GetMaData(this.ID.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata);
            SyncServer.ThrowExceptionOnReturnError(result);

            XmlDocument d = new XmlDocument();
            d.LoadXml(result);
            return d.SelectSingleNode("/");
        }

        private CSObjectEnumerator ExportConnectorSpace(string critieria, CSObjectParts csParts, uint entryParts)
        {
            string token = ManagementAgentBase.WebService.ExportConnectorSpace(this.Name, critieria, true);
            SyncServer.ThrowExceptionOnReturnError(token);

            return new CSObjectEnumerator(ManagementAgentBase.WebService, token, true, csParts, entryParts);
        }

        private CSObjectEnumerator ExportConnectorSpace(string critieria)
        {
            return this.ExportConnectorSpace(critieria, CSObjectParts.AllItems, 0xFFFFFFFF);
        }

        private CSObject GetSingleCSObject(string criteria)
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

        private CSObjectEnumerator ExecuteCSSearch(string criteria)
        {
            return this.ExecuteCSSearch(criteria, CSObjectParts.AllItems, 0xFFFFFFFF);
        }

        private CSObjectEnumerator ExecuteCSSearch(string criteria, CSObjectParts csParts, uint entryParts)
        {
            string token = ManagementAgentBase.WebService.ExecuteCSSearch(this.ID.ToMmsGuid(), criteria);
            SyncServer.ThrowExceptionOnReturnError(token);

            return new CSObjectEnumerator(ManagementAgentBase.WebService, token, false, csParts, entryParts);
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
    }
}

