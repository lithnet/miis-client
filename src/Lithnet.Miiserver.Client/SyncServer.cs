﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Instrumentation;
using System.Reflection;
using System.IO;
using System.Management.Automation;
using System.Diagnostics;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Runtime.InteropServices;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public static class SyncServer
    {
        internal static ManagementScope scope = new ManagementScope(@"\\.\ROOT\MicrosoftIdentityIntegrationServer");

        private static PowerShell psSession;

        private static MMSWebService ws = new MMSWebService();

        public static string TranslateCOMException(COMException ex)
        {
            return WebServiceUtils.TranslateMMSError(ex);
        }

        public static MVObject GetMVObject(Guid ID)
        {
            string result = ws.GetMVObjects(new string[] { ID.ToMmsGuid() }, 1, 0xffffffff, 0xffffffff, 0, null);

            XmlDocument d = new XmlDocument();
            d.LoadXml(result);

            return new MVObject(d.SelectSingleNode("/mv-objects/mv-object"));
        }

        public static MVObjectCollection GetMVObjects(MVQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (query.ObjectType == null && query.QueryItems.Count == 0)
            {
                throw new InvalidOperationException("Either an object type or attribute queries must be provided");
            }

            string results = ws.SearchMV(query.GetXml());

            if (string.IsNullOrEmpty(results))
            {
                return new MVObjectCollection(new XmlDocument());
            }

            XmlDocument d = new XmlDocument();
            d.LoadXml(results);

            return new MVObjectCollection(d);
        }

        public static DsmlSchema GetMVSchema()
        {
            string schema = ws.GetMVData(MVData.MV_SCHEMA);
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

            ObjectQuery query = new ObjectQuery(string.Format("SELECT * FROM MIIS_Server"));
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection moc = searcher.Get();

            if (moc.Count == 0)
            {
                throw new InvalidOperationException();
            }

            ManagementObject mo = moc.OfType<ManagementObject>().First();

            mo.InvokeMethod("ClearRuns", new object[] { date });
        }

        public static IEnumerable<RunSummary> GetRunSummary()
        {
            ulong ts = 0;
            int reload;
            uint returned;

            string result = ws.GetExecSummary(ref ts, out reload, out returned);

            return RunSummary.GetRunSummary(result);
        }

        public static RunDetails GetRunDetail(RunSummary summary)
        {
            return SyncServer.GetRunDetail(summary.MAID, summary.RunNumber);
        }

        public static RunDetails GetRunDetail(Guid maid, int runNumber)
        {
            string query = string.Format("<execution-history-req ma=\"{0}\"><run-number>{1}</run-number></execution-history-req>", maid.ToMmsGuid(), runNumber);
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

        public static void ClearRunHistory(DateTime clearBeforeDate, string filename)
        {
            SyncServer.SaveRunHistory(clearBeforeDate, filename);
            SyncServer.ClearRunHistory(clearBeforeDate);
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

        private static string GetMVData()
        {
            return ws.GetMVData(MVData.MV_ALL);
        }

        public static XmlNode GetImportAttributeFlows()
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(SyncServer.GetImportFlows());

            return d.SelectSingleNode("/mv-data/import-attribute-flow");
        }

        private static string GetImportFlows()
        {
            return ws.GetMVData(MVData.MV_IMPORT_ATTR_FLOW);
        }
    }
}