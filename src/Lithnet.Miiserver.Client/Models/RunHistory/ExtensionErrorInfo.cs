namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    [Serializable]
    [DebuggerStepThroughAttribute]
    public class ExtensionErrorInfo : NodeCache
    {
        internal ExtensionErrorInfo(XmlNode node)
            :base(node)
        {
        }

        public string ExtensionName
        {
            get
            {
                return this.GetValue<string>("extension-name");
            }
        }

        public ExtensionCallSite ExtensionCallSite
        {
            get
            {
                return this.GetValue<ExtensionCallSite>("extension-callsite");
            }
        }

        public string ExtensionContext
        {
            get
            {
                return this.GetValue<string>("extension-context");
            }
        }

        public string CallStack
        {
            get
            {
                return this.GetValue<string>("call-stack");
            }
        }
    }
}
