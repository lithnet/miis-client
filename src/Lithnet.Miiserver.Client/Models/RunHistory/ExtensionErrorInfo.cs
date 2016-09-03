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
    public class ExtensionErrorInfo : XmlObjectBase
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

        public string ExtensionCallSite
        {
            get
            {
                return this.GetValue<string>("extension-callsite");
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
