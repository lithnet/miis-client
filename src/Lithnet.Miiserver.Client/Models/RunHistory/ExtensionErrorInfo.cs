using System;
using System.Xml;
using System.Diagnostics;

namespace Lithnet.Miiserver.Client
{
    [Serializable]
    [DebuggerStepThroughAttribute]
    public class ExtensionErrorInfo : XmlObjectBase
    {
        internal ExtensionErrorInfo(XmlNode node)
            :base(node)
        {
        }

        public string ExtensionName => this.GetValue<string>("extension-name");

        public string ExtensionCallSite => this.GetValue<string>("extension-callsite");

        public string ExtensionContext => this.GetValue<string>("extension-context");

        public string CallStack => this.GetValue<string>("call-stack");
    }
}
