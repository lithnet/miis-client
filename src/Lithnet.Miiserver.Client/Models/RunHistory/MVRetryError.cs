namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class MVRetryError :NodeCache
    {
        internal MVRetryError(XmlNode node)
            :base(node)
        {
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

        public string DisplayName
        {
            get
            {
                return this.GetValue<string>("@display-name");
            }
        }
    }
}
