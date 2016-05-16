namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class RunSummary : NodeCache
    {
        internal RunSummary(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the GUID of the management agent.
        /// </summary>
        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("ma-id");
            }
        }

        /// <summary>
        /// Gets the display name of the management agent
        /// </summary>
        public string MAName
        {
            get
            {
                return this.GetValue<string>("ma-name");
            }
        }

        /// <summary>
        /// Gets the sequence number of the run, starting with 1
        /// </summary>
        /// <remarks>
        /// Only runs that call "Execute" are given run-numbers
        /// </remarks>
        public int RunNumber
        {
            get
            {
                return this.GetValue<int>("run-number");
            }
        }

        /// <summary>
        /// Gets the display name of the run profile that was used when the management agent was run.
        /// </summary>
        public string RunProfileName
        {
            get
            {
                return this.GetValue<string>("run-profile-name");
            }
        }

        /// <summary>
        /// Gets the "domain\account name" of the account that invokes the management agent run
        /// </summary>
        public string SecurityID
        {
            get
            {
                return this.GetValue<string>("security-id");
            }
        }

        public DateTime? StartTime
        {
            get
            {
                return this.GetValue<DateTime?>("start-time");
            }
        }

        public DateTime? EndTime
        {
            get
            {
                return this.GetValue<DateTime?>("end-time");
            }
        }

        public string Result
        {
            get
            {
                return this.GetValue<string>("result");
            }
        }

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
