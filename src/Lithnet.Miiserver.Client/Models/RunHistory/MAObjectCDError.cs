using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAObjectCDError : XmlObjectBase
    {
        internal MAObjectCDError(XmlNode node)
        : base(node)
        {
        }

        public string ErrorCode => this.GetValue<string>("error-code");

        public string ErrorLiteral => this.GetValue<string>("error-literal");

        public string ServerErrorDetail => this.GetValue<string>("server-error-detail");

        public IReadOnlyCollection<string> Value => this.GetReadOnlyValueList<string>("value");
    }
}
