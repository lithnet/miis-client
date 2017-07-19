using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Management.Automation;
using System.Diagnostics;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Xml;
using System.Threading.Tasks;

namespace Lithnet.Miiserver.Client
{
    public static class SyncServer
    {
        internal static ManagementScope Scope = new ManagementScope(@"\\.\ROOT\MicrosoftIdentityIntegrationServer");

        private static PowerShell psSession;

        private static MMSWebService ws = new MMSWebService();

        public static string TranslateCOMException(COMException ex)
        {
            return WebServiceUtils.TranslateMMSError(ex);
        }

        public static MVObject GetMVObject(Guid id)
        {
            string result = ws.GetMVObjects(new string[] {id.ToMmsGuid()}, 1, 0xffffffff, 0xffffffff, 0, null);
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

        static SyncServer()
        {
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

            string result = mo.InvokeMethod("ClearRuns", new object[] {date}) as string;

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

            string result = mo.InvokeMethod("ClearPasswordHistory", new object[] {date}) as string;

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
            ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM MIIS_Server"));
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
            int reload;
            uint returned;

            string result = ws.GetExecSummary(ref ts, out reload, out returned);
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
            string result = ws.GetRole();
            SyncServer.ThrowExceptionOnReturnError(result);

            int role = Convert.ToInt32(result);

            return (1 & role) == 1;
        }

        public static void SaveRunHistory(string filename)
        {
            SyncServer.SaveRunHistory(DateTime.UtcNow, filename);
        }

        public static void SaveRunHistory(DateTime beforeDate, string filename)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement parent = doc.CreateElement("execution-histories");
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
                    parent.AppendChild(doc.ImportNode(d.GetNode(), true));
                }
            }

            doc.AppendChild(parent);

            if (count == 0)
            {
                return;
            }

            using (XmlWriter w = XmlWriter.Create(filename, new XmlWriterSettings() {Indent = true}))
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

        [Obsolete]
        private static bool Ping()
        {
            Task t = Task.Run(() => ws.GetMAList());

            if (t.Wait(5000))
            {
                return true;
            }
            else
            {
                return false;
            }
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
                    byte[] data = reader[0] as byte[];

                    if (data == null)
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