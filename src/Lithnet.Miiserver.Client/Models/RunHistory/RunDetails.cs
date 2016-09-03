namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.IO;

    public class RunDetails : XmlObjectBase
    {
        internal RunDetails(XmlNode node)
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

        /// <summary>
        /// Gets information about the steps of the management agent run
        /// </summary>
        public IReadOnlyList<StepDetails> StepDetails
        {
            get
            {
                return this.GetReadOnlyObjectList<StepDetails>("step-details");
            }
        }

        public DateTime? StartTime
        {
            get
            {
                if (this.StepDetails.Count == 0)
                {
                    return null;
                }

                return this.StepDetails.Last().StartDate;
            }
        }

        public DateTime? EndTime
        {
            get
            {
                return this.StepDetails?.FirstOrDefault()?.EndDate;
            }
        }

        public string LastStepStatus
        {
            get
            {
                StepDetails step = this.StepDetails.FirstOrDefault();

                if (step == null)
                {
                    return null;
                }

                return step.StepResult;
            }
        }

        internal static IEnumerable<RunDetails> GetRunDetails(string xml)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);

            foreach (XmlNode node in d.SelectNodes("run-history/run-details"))
            {
                yield return new RunDetails(node);
            }
        }

        internal static IEnumerable<RunDetails> GetRunDetails(string xml, Guid maid)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);

            foreach (XmlNode node in d.SelectNodes(string.Format("run-history/run-details[ma-id = '{0}']", maid.ToMmsGuid())))
            {
                yield return new RunDetails(node);
            }
        }

        public static IEnumerable<RunDetails> LoadRunDetails(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            XmlDocument d = new XmlDocument();
            d.Load(path);

            if (d.DocumentElement.LocalName == "execution-histories")
            {
                foreach (XmlNode node in d.SelectNodes("/execution-histories/run-history/run-details"))
                {
                    yield return new RunDetails(node);
                }
            }
            else if (d.DocumentElement.LocalName == "run-history")
            {
                yield return new RunDetails(d.SelectSingleNode("/run-history/run-details"));
            }
            else
            {
                throw new InvalidOperationException("The specified file was not a saved execution history file");
            }
        }
    }
}
