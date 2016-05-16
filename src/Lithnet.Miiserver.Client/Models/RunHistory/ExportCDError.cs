namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class ExportCDError : NodeCache
    {
        internal ExportCDError(XmlNode node)
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

        public string ServerErrorDetail
        {
            get
            {
                return this.GetValue<string>("server-error-detail");
            }
        }
    }
}
