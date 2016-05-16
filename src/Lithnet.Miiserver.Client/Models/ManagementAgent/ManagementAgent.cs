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

namespace Lithnet.Miiserver.Client
{
    public class ManagementAgent : ManagementAgentBase
    {
        protected ManagementAgent(XmlNode node, Guid id)
            :base (node, id)
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
            ArrayList names = new ArrayList();
            ArrayList ids = new ArrayList();
            ws.GetMAGuidList(out ids, out names);

            foreach (string id in ids.OfType<string>())
            {
                yield return ManagementAgent.GetManagementAgentFromID(new Guid(id));
            }
        }

        public static Guid MANameToID(string name)
        {
            ArrayList names = new ArrayList();
            ArrayList ids = new ArrayList();
            ws.GetMAGuidList(out ids, out names);

            if (names.Count != ids.Count)
            {
                throw new ArgumentOutOfRangeException("The sync engine returned a mis-matched number of management agents in the name-to-id mapping data");
            }

            for (int i = 0; i < ids.Count; i++)
            {
                if (string.Equals(name, (string)names[i], StringComparison.OrdinalIgnoreCase))
                {
                    return new Guid((string)ids[i]);
                }
            }

            throw new InvalidOperationException(string.Format("Management agent {0} was not found", name));
        }

        public bool IsIdle()
        {
            return ws.IsMAIdle(this.ID.ToMmsGuid());
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
            ws.StopMA(this.ID.ToMmsGuid());
        }

        public void StopAsync()
        {
            Task t = new Task(() =>
            {
                this.Stop();
            });

            t.Start();
        }

        public void SuppressFullSyncWarning()
        {
            ws.SuppressRunStepWarning(this.ID.ToMmsGuid());
        }

        public void ExecuteRunProfile(string name)
        {
            this.ExecuteRunProfile(name, false);
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
            Task<string> t = new Task<string>(() =>
            {
                return this.ExecuteRunProfile(name, resumeLastRun);
            });

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

        public CSObject GetCSObject(string dn)
        {
            string search = string.Format("<searching><dn recursive=\"false\">{0}</dn></searching>", dn);
            return this.GetSingleCSObject(search);
        }

        public CSObjectEnumerator GetCSObjects(string dn, bool searchSubTree)
        {
            string search = string.Format("<searching><dn recursive=\"{0}\">{1}</dn></searching>", searchSubTree.ToString().ToLower(), dn);
            return this.ExecuteCSSearch(search);
        }

        public CSObjectEnumerator GetCSObjects(string rdn)
        {
            string search = string.Format("<searching><rdn>{0}</rdn></searching>", rdn);
            return this.ExecuteCSSearch(search);
        }

        public CSObject GetCSObject(Guid id)
        {
            string search = string.Format("<searching><id>{0}</id></searching>", id.ToMmsGuid());
            return this.GetSingleCSObject(search);
        }
        public CSObjectEnumerator GetPendingImports(bool getAdds, bool getUpdates, bool getDeletes)
        {
            if (!(getAdds | getUpdates | getDeletes))
            {
                throw new ArgumentException("At least one change type must be specified");
            }

            string searchText = string.Format("<criteria><pending-import add=\"{0}\" modify=\"{1}\" delete=\"{2}\"></pending-import></criteria>",
                getAdds.ToString().ToLower(),
                getUpdates.ToString().ToLower(),
                getDeletes.ToString().ToLower());

            return this.ExportConnectorSpace(searchText);
        }

        public CSObjectEnumerator GetPendingExports(bool getAdds, bool getUpdates, bool getDeletes)
        {
            if (!(getAdds | getUpdates | getDeletes))
            {
                throw new ArgumentException("At least one change type must be specified");
            }

            string searchText = string.Format("<criteria><pending-export add=\"{0}\" modify=\"{1}\" delete=\"{2}\"></pending-export></criteria>",
                getAdds.ToString().ToLower(),
                getUpdates.ToString().ToLower(),
                getDeletes.ToString().ToLower());

            return this.ExportConnectorSpace(searchText);
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

            string searchText = string.Format("<criteria><connector>false</connector><connector-state>{0}</connector-state></criteria>", type);
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

            string searchText = string.Format("<criteria><connector>true</connector><connector-state>{0}</connector-state></criteria>", type);
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
            using (CSObjectEnumerator e = this.GetPendingExports(true, true, true))
            {
                return e.BatchCount > 0;
            }
        }

        public bool HasPendingImports()
        {
            using (CSObjectEnumerator e = this.GetPendingImports(true, true, true))
            {
                return e.BatchCount > 0;
            }
        }

        public IEnumerable<RunSummary> GetRunSummary()
        {
            ulong ts = 0;
            int reload;
            uint returned;

            string result = ws.GetExecSummary(ref ts, out reload, out returned);

            return RunSummary.GetRunSummary(result, this.ID);
        }

        public RunDetails GetRunDetail(RunSummary summary)
        {
            return this.GetRunDetail(summary.RunNumber);
        }

        public RunDetails GetRunDetail(int runNumber)
        {
            string query = string.Format("<execution-history-req ma=\"{0}\"><run-number>{1}</run-number></execution-history-req>", this.ID.ToMmsGuid(), runNumber);
            string result = ws.GetExecutionHistory(query);

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

            string query = string.Format("<execution-history-req ma=\"{0}\"><num-req>{1}</num-req></execution-history-req>", this.ID.ToMmsGuid(), count);

            string result = ws.GetExecutionHistory(query);

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
            string query = string.Format("<execution-history-req ma=\"{0}\"><num-req>1</num-req></execution-history-req>", this.ID.ToMmsGuid());

            string result = ws.GetExecutionHistory(query);

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

        internal string ExportManagementAgent(bool includeIAFs, string timestamp)
        {
            return ManagementAgent.ws.ExportManagementAgent(this.Name, true, includeIAFs, timestamp);
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
            this.ExportManagementAgent(includeIAFs, DateTime.Now.ToMmsDateString());
        }

        internal void ExportManagementAgent(string file, bool includeIAFs, string timestamp)
        {
            string data = this.ExportManagementAgent(includeIAFs, timestamp);
            System.IO.File.WriteAllText(file, data);
        }

        private XmlNode GetMaData(MAData madata, MAPartitionData partitionData, MARunData rundata)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(ws.GetMaData(this.ID.ToMmsGuid(), (uint)madata, (uint)partitionData, (uint)rundata));
            return d.SelectSingleNode("/");
        }

        private CSObjectEnumerator ExportConnectorSpace(string critieria)
        {
            string token = ws.ExportConnectorSpace(this.Name, critieria, true);
            return new CSObjectEnumerator(ws, token, true);
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
            string token = ws.ExecuteCSSearch(this.ID.ToMmsGuid(), criteria);
            return new CSObjectEnumerator(ws, token, false);
        }

        private static ManagementObject GetManagementAgentWmiObject(string id)
        {
            ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM MIIS_ManagementAgent where Guid='{0}'", id));
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.scope, query);
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

