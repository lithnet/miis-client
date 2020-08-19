using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

namespace Lithnet.Miiserver.Client
{
    public static class SyncServer
    {
        internal static ManagementScope Scope = new ManagementScope(@"\\.\ROOT\MicrosoftIdentityIntegrationServer");

        private static PowerShell psSession;

        private static object schemaSyncObject = new object();

        private static MMSWebService ws = new MMSWebService();

        public static string TranslateCOMException(COMException ex)
        {
            return WebServiceUtils.TranslateMMSError(ex);
        }

        public static MVObject GetMVObject(Guid id)
        {
            string result = ws.GetMVObjects(new[] { id.ToMmsGuid() }, 1, 0xffffffff, 0xffffffff, 0, null);
            SyncServer.ThrowExceptionOnReturnError(result);

            XmlDocument d = new XmlDocument();
            d.LoadXml(result);

            return new MVObject(d.SelectSingleNode("/mv-objects/mv-object"));
        }

        public static MVObjectCollection GetMVObjects(MVQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.ObjectType == null && query.QueryItems.Count == 0)
            {
                throw new InvalidOperationException("Either an object type or attribute queries must be provided");
            }

            StringBuilder resultSet = new StringBuilder();
            resultSet.AppendLine("<results>");

            string results = ws.SearchMV(query.GetXml());
            SyncServer.ThrowExceptionOnReturnError(results);

            if (string.IsNullOrEmpty(results))
            {
                return new MVObjectCollection(new XmlDocument());
            }

            resultSet.AppendLine(results);
            resultSet.AppendLine("</results>");

            XmlDocument d = new XmlDocument();
            d.LoadXml(resultSet.ToString());

            return new MVObjectCollection(d);
        }

        public static DsmlSchema GetMVSchema()
        {
            string schema = ws.GetMVData(MVData.MV_SCHEMA);
            SyncServer.ThrowExceptionOnReturnError(schema);

            return DsmlSchema.GetMVSchema(schema);
        }

        private static PowerShell PSSession
        {
            get
            {
                if (SyncServer.psSession == null)
                {
                    SyncServer.psSession = PowerShell.Create();
                    SyncServer.psSession.AddCommand("Import-Module");
                    SyncServer.psSession.AddParameter("Assembly", AssemblyProvider.MiisPSModule);
                    SyncServer.psSession.Invoke();
                }

                return SyncServer.psSession;
            }
        }

        public static void ClearRunHistory()
        {
            SyncServer.ClearRunHistory(DateTime.UtcNow);
        }

        public static void ClearRunHistory(DateTime clearBeforeDate)
        {
            string date = clearBeforeDate.ToMmsDateString();

            ManagementObject mo = SyncServer.GetServerManagementObject();

            string result = mo.InvokeMethod("ClearRuns", new object[] { date }) as string;

            if (result == "access-denied")
            {
                throw new UnauthorizedAccessException();
            }
            else if (result != "success")
            {
                throw new MiiserverException($"The operation returned {result}");
            }
        }

        public static void ClearPasswordHistory(DateTime clearBeforeDate)
        {
            string date = clearBeforeDate.ToMmsDateString();

            ManagementObject mo = SyncServer.GetServerManagementObject();

            string result = mo.InvokeMethod("ClearPasswordHistory", new object[] { date }) as string;

            if (result == "access-denied")
            {
                throw new UnauthorizedAccessException();
            }
            else if (result != "success")
            {
                throw new MiiserverException($"The operation returned {result}");
            }
        }

        public static void ClearPasswordQueue()
        {
            ManagementObject mo = SyncServer.GetServerManagementObject();

            string result = mo.InvokeMethod("ClearPasswordQueue", new object[] { }) as string;

            if (result == "access-denied")
            {
                throw new UnauthorizedAccessException();
            }
            else if (result != "success")
            {
                throw new MiiserverException($"The operation returned {result}");
            }
        }

        private static ManagementObject GetServerManagementObject()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM MIIS_Server");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.Scope, query);
            ManagementObjectCollection moc = searcher.Get();

            if (moc.Count == 0)
            {
                throw new InvalidOperationException();
            }

            ManagementObject mo = moc.OfType<ManagementObject>().First();
            return mo;
        }

        public static IEnumerable<RunSummary> GetRunSummary()
        {
            ulong ts = 0;

            string result = ws.GetExecSummary(ref ts, out _, out _);
            SyncServer.ThrowExceptionOnReturnError(result);

            return RunSummary.GetRunSummary(result);
        }

        public static RunDetails GetRunDetail(RunSummary summary)
        {
            return SyncServer.GetRunDetail(summary.MAID, summary.RunNumber);
        }

        public static RunDetails GetRunDetail(Guid maid, int runNumber)
        {
            string query = $"<execution-history-req ma=\"{maid.ToMmsGuid()}\"><run-number>{runNumber}</run-number></execution-history-req>";
            string result = ws.GetExecutionHistory(query);
            SyncServer.ThrowExceptionOnReturnError(result);

            return result != null ? RunDetails.GetRunDetails(result).FirstOrDefault() : null;
        }

        public static void ClearRunHistory(DateTime clearBeforeDate, string filename)
        {
            SyncServer.SaveRunHistory(clearBeforeDate, filename);
            SyncServer.ClearRunHistory(clearBeforeDate);
        }

        public static bool IsAdmin()
        {
            try
            {
                return GetRole().HasFlag(SyncServiceRole.Administrator);
            }
            catch (MiiserverException e)
            {
                Trace.WriteLine(e);
                return false;
            }
        }

        public static bool IsOperator()
        {
            try
            {
                SyncServiceRole r = GetRole();
                return r.HasFlag(SyncServiceRole.Administrator) || r.HasFlag(SyncServiceRole.Operator);
            }
            catch (MiiserverException e)
            {
                Trace.WriteLine(e);
                return false;
            }
        }

        public static SyncServiceRole GetRole()
        {
            string result = ws.GetRole();
            SyncServer.ThrowExceptionOnReturnError(result);

            int role = Convert.ToInt32(result);

            return (SyncServiceRole)role;
        }

        public static void SaveRunHistory(string filename)
        {
            SyncServer.SaveRunHistory(DateTime.UtcNow, filename);
        }

        public static void SaveRunHistory(DateTime beforeDate, string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement executionHistories = doc.CreateElement("execution-histories");
            XmlElement runHistoryElement = doc.CreateElement("run-history");
            executionHistories.AppendChild(runHistoryElement);

            int count = 0;

            foreach (RunSummary item in SyncServer.GetRunSummary())
            {
                if (!item.StartTime.HasValue || !item.EndTime.HasValue)
                {
                    continue;
                }

                if (item.EndTime < beforeDate)
                {
                    count++;
                    RunDetails d = SyncServer.GetRunDetail(item);
                    runHistoryElement.AppendChild(doc.ImportNode(d.XmlNode, true));
                }
            }

            doc.AppendChild(executionHistories);

            if (count == 0)
            {
                return;
            }

            using (XmlWriter w = XmlWriter.Create(filename, new XmlWriterSettings() { Indent = true }))
            {
                doc.WriteTo(w);
                w.Close();
            }
        }

        public static void ClearRunHistory(string filename)
        {
            SyncServer.ClearRunHistory(DateTime.UtcNow, filename);
        }

        public static void SetProvisionRulesExtension(bool enabled)
        {
            SyncServer.PSSession.Commands.Clear();
            SyncServer.PSSession.AddCommand("Set-ProvisioningRulesExtension");
            SyncServer.PSSession.AddParameter("Value", enabled.ToString());
            SyncServer.PSSession.Invoke();
        }

        public static void ExportMetaverseConfiguration(string path)
        {
            SyncServer.ValidateEmptyDirectory(path);
            string timestamp = DateTime.Now.ToMmsDateString();
            string data = ws.ExportMetaverse(timestamp);
            string filename = Path.Combine(path, "MV.XML");
            System.IO.File.WriteAllText(filename, data);

            foreach (ManagementAgent ma in ManagementAgent.GetManagementAgents())
            {
                filename = Path.Combine(path, string.Format("MA-{0}.XML", ma.ID.ToMmsGuid()));
                ma.ExportManagementAgent(filename, false, timestamp);
            }
        }

        private static void ValidateEmptyDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (Directory.GetFiles(path).Any())
            {
                throw new ArgumentException("The specified directory must be empty");
            }
        }

        public static void ExportMVConfiguration(string path)
        {
            Process p = AssemblyProvider.InitializeSvrExportProcess(path);
            p.Start();
            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                throw new ApplicationException(p.StandardOutput.ReadToEnd());
            }
        }

        public static MVConfiguration GetMVConfiguration()
        {
            string result = SyncServer.GetMVData();
            XmlDocument d = new XmlDocument();
            d.LoadXml(result);
            return new MVConfiguration(d.SelectSingleNode("mv-data"));
        }
      
        public static IEnumerable<CSObjectRef> GetStepDetailCSObjectRefs(Guid stepID, string statisticsType)
        {
            return GetStepDetailCSObjectRefs(stepID, statisticsType, new CancellationToken());
        }

        public static IEnumerable<CSObjectRef> GetStepDetailCSObjectRefs(Guid stepID, string statisticsType, CancellationToken t, uint pageSize = 10)
        {
            string query = $"<step-object-details-filter step-id='{stepID.ToMmsGuid()}'><statistics type='{statisticsType}'/></step-object-details-filter>";
            string token = null;

            try
            {
                token = ws.ExecuteStepObjectDetailsSearch(query);

                while (!t.IsCancellationRequested)
                {
                    int count = 0;

                    string xml = ws.GetStepObjectResults(token, pageSize);
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
                    ws.ReleaseSessionObjects(new[] { token });
                }
            }
        }

        private static DsmlSchema schemaCache;

        private static DsmlSchema SchemaCache
        {
            get
            {
                if (SyncServer.schemaCache == null)
                {
                    lock (schemaSyncObject)
                    {
                        SyncServer.schemaCache = SyncServer.GetMVSchema();
                    }
                }

                return SyncServer.schemaCache;
            }
        }

        public static void RefreshSchemaCache()
        {
            lock (schemaSyncObject)
            {
                SyncServer.schemaCache = null;
            }
        }

        internal static void ThrowOnInvalidObjectType(string objectType)
        {
            if (!SyncServer.SchemaCache.ObjectClasses.ContainsKey(objectType))
            {
                throw new MiiserverException($"The object type {objectType} was not found in the schema");
            }
        }

        /// <summary>
        /// Throws an exception when the web service returns an error in an XML result
        /// </summary>
        /// <param name="result">The XML result from the server to parse</param>
        internal static void ThrowExceptionOnReturnError(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (result.StartsWith("<error>"))
            {
                result = result.Substring(7, result.Length - 15);
                throw new MiiserverException(result);
            }
        }

        private static string GetMVData()
        {
            string result = ws.GetMVData(MVData.MV_ALL);
            SyncServer.ThrowExceptionOnReturnError(result);
            return result;
        }

        public static XmlNode GetImportAttributeFlows()
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(SyncServer.GetImportFlows());

            return d.SelectSingleNode("mv-data/import-attribute-flow");
        }

        private static string GetImportFlows()
        {
            string result = ws.GetMVData(MVData.MV_IMPORT_ATTR_FLOW);
            SyncServer.ThrowExceptionOnReturnError(result);
            return result;
        }

        public static SecurityIdentifier GetAdministratorsGroupSid()
        {
            return GetGroupSid("administrators_sid");
        }

        public static SecurityIdentifier GetOperatorsGroupSid()
        {
            return GetGroupSid("operators_sid");
        }

        public static SecurityIdentifier GetAccountJoinersGroupSid()
        {
            return GetGroupSid("account_joiners_sid");
        }

        public static SecurityIdentifier GetBrowsersGroupSid()
        {
            return GetGroupSid("browse_sid");
        }

        public static SecurityIdentifier GetPasswordSetGroupSid()
        {
            return GetGroupSid("passwordset_sid");
        }

        private static SecurityIdentifier GetGroupSid(string name)
        {
            string queryString = $"SELECT TOP 1 {name} FROM dbo.mms_server_configuration";

            using (SqlConnection connection = new SqlConnection(MiiserverConfig.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (!(reader[0] is byte[] data))
                    {
                        return null;
                    }

                    return new SecurityIdentifier(data, 0);
                }

                reader.Close();
            }

            return null;
        }
    }
}