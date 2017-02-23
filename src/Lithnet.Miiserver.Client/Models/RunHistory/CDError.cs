using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class CDError : XmlObjectBase
    {
        internal CDError(XmlNode node)
            :base(node)
        {
        }

        public string ErrorCode => this.GetValue<string>("error-code");

        public string ErrorLiteral => this.GetValue<string>("error-literal");
    }
}
