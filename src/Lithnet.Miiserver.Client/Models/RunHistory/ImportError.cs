namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class ImportError : NodeCache
    {
        internal ImportError(XmlNode node)
            : base(node)
        {
        }

        public DateTime FirstOccured
        {
            get
            {
                return this.GetValue<DateTime>("first-occurred");
            }
        }

        public int RetryCount
        {
            get
            {
                return this.GetValue<int>("retry-count");
            }
        }

        public DateTime DateOccurred
        {
            get
            {
                return this.GetValue<DateTime>("date-occurred");
            }
        }

        public string ErrorType
        {
            get
            {
                return this.GetValue<string>("error-type");
            }
        }

        public AlgorithmStep AlgorithmStep
        {
            get
            {
                return this.GetObject<AlgorithmStep>("algorithm-step");
            }
        }

        public ExportChangeNotReimportedError ChangeNotReimported
        {
            get
            {
                return this.GetObject<ExportChangeNotReimportedError>("change-not-reimported");
            }
        }

        public ExtensionErrorInfo ExtensionErrorInfo
        {
            get
            {
                return this.GetObject<ExtensionErrorInfo>("extension-error-info");
            }
        }

        public RulesErrorInfoContext RulesErrorInfo
        {
            get
            {
                return this.GetObject<RulesErrorInfoContext>("rules-error-info/context");
            }
        }

        public Guid CSGuid
        {
            get
            {
                return this.GetValue<Guid>("@cs-guid");
            }
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("@dn");
            }
        }
    }
}
