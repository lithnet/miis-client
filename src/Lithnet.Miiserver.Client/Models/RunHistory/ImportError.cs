using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ImportError : XmlObjectBase
    {
        internal ImportError(XmlNode node)
            : base(node)
        {
        }

        public DateTime FirstOccured => this.GetValue<DateTime>("first-occurred");

        public int RetryCount => this.GetValue<int>("retry-count");

        public DateTime DateOccurred => this.GetValue<DateTime>("date-occurred");

        public string ErrorType => this.GetValue<string>("error-type");

        public AlgorithmStep AlgorithmStep => this.GetObject<AlgorithmStep>("algorithm-step");

        public ExportChangeNotReimportedError ChangeNotReimported => this.GetObject<ExportChangeNotReimportedError>("change-not-reimported");

        public ExtensionErrorInfo ExtensionErrorInfo => this.GetObject<ExtensionErrorInfo>("extension-error-info");

        public RulesErrorInfoContext RulesErrorInfo => this.GetObject<RulesErrorInfoContext>("rules-error-info/context");

        public Guid CSGuid => this.GetValue<Guid>("@cs-guid");

        public string DN => this.GetValue<string>("@dn");
    }
}
