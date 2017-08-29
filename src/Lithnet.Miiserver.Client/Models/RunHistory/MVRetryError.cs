using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVRetryError :XmlObjectBase
    {
        internal MVRetryError(XmlNode node)
            :base(node)
        {
        }

        public DateTime DateOccurred => this.GetValue<DateTime>("date-occurred");

        public string ErrorType => this.GetValue<string>("error-type");

        public AlgorithmStep AlgorithmStep => this.GetObject<AlgorithmStep>("algorithm-step");

        public ExtensionErrorInfo ExtensionErrorInfo => this.GetObject<ExtensionErrorInfo>("extension-error-info");

        public RulesErrorInfoContext RulesErrorInfo => this.GetObject<RulesErrorInfoContext>("rules-error-info/context");

        public string DisplayName => this.GetValue<string>("@display-name");

        public string MVID => this.GetValue<string>("@mv-guid");
    }
}
