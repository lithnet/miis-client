namespace Lithnet.Miiserver.Client
{
    using System.Collections.Generic;
    using System.Xml;

    public class MAObjectCDError : XmlObjectBase
    {
        internal MAObjectCDError(XmlNode node)
        : base(node)
        {
        }

        public string ErrorCode
        {
            get
            {
                return this.GetValue<string>("error-code");
            }
        }

        public string ErrorLiteral
        {
            get
            {
                return this.GetValue<string>("error-literal");
            }
        }

        public string ServerErrorDetail
        {
            get
            {
                return this.GetValue<string>("server-error-detail");
            }
        }

        public IReadOnlyCollection<string> Value
        {
            get
            {
                return this.GetReadOnlyValueList<string>("value");
            }
        }
    }
}
