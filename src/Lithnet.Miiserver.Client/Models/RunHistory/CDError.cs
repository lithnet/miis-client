namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public partial class CDError : NodeCache
    {
        internal CDError(XmlNode node)
            :base(node)
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
    }
}
