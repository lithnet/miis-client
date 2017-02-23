using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class RunSummary : XmlObjectBase
    {
        internal RunSummary(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the GUID of the management agent.
        /// </summary>
        public Guid MAID => this.GetValue<Guid>("ma-id");

        /// <summary>
        /// Gets the display name of the management agent
        /// </summary>
        public string MAName => this.GetValue<string>("ma-name");

        /// <summary>
        /// Gets the sequence number of the run, starting with 1
        /// </summary>
        /// <remarks>
        /// Only runs that call "Execute" are given run-numbers
        /// </remarks>
        public int RunNumber => this.GetValue<int>("run-number");

        /// <summary>
        /// Gets the display name of the run profile that was used when the management agent was run.
        /// </summary>
        public string RunProfileName => this.GetValue<string>("run-profile-name");

        /// <summary>
        /// Gets the "domain\account name" of the account that invokes the management agent run
        /// </summary>
        public string SecurityID => this.GetValue<string>("security-id");

        public DateTime? StartTime => this.GetValue<DateTime?>("start-time");

        public DateTime? EndTime => this.GetValue<DateTime?>("end-time");

        public string Result => this.GetValue<string>("result");

        internal static IEnumerable<RunSummary> GetRunSummary(string xml)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);

            foreach (XmlNode node in d.SelectNodes("run-history/run-details"))
            {
                yield return new RunSummary(node);
            }
        }

        internal static IEnumerable<RunSummary> GetRunSummary(string xml, Guid maid)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);

            foreach (XmlNode node in d.SelectNodes(string.Format("run-history/run-details[ma-id = '{0}']", maid.ToMmsGuid())))
            {
                yield return new RunSummary(node);
            }
        }
    }
}
