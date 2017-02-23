using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportCDError : XmlObjectBase
    {
        internal ExportCDError(XmlNode node)
            :base(node)
        {
        }

        public string ErrorCode => this.GetValue<string>("error-code");

        public string ErrorLiteral => this.GetValue<string>("error-literal");

        public string ServerErrorDetail => this.GetValue<string>("server-error-detail");
    }
}
