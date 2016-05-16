namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Diagnostics;

    public class MAObjectError : NodeCache
    {
        internal MAObjectError(XmlNode node)
            : base(node)
        {
        }

        public string ErrorType
        {
            get
            {
                return this.GetValue<string>("error-type");
            }
        }

        public int EntryNumber
        {
            get
            {
                return this.GetValue<int>("entry-number");
            }
        }

        public int LineNumber
        {
            get
            {
                return this.GetValue<int>("line-number");
            }
        }

        public int ColumnNumber
        {
            get
            {
                return this.GetValue<int>("column-number");
            }
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("dn");
            }
        }

        public EncodedValue Anchor
        {
            get
            {
                return this.GetObject<EncodedValue>("anchor");
            }
        }

        public string AttributeName
        {
            get
            {
                return this.GetValue<string>("attribute-name");
            }
        }

        public MAObjectCDError CDError
        {
            get
            {
                return this.GetObject<MAObjectCDError>("cd-error");
            }
        }
    }
}
