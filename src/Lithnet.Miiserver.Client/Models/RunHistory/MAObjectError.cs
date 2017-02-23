using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAObjectError : XmlObjectBase
    {
        internal MAObjectError(XmlNode node)
            : base(node)
        {
        }

        public string ErrorType => this.GetValue<string>("error-type");

        public int EntryNumber => this.GetValue<int>("entry-number");

        public int LineNumber => this.GetValue<int>("line-number");

        public int ColumnNumber => this.GetValue<int>("column-number");

        public string DN => this.GetValue<string>("dn");

        public EncodedValue Anchor => this.GetObject<EncodedValue>("anchor");

        public string AttributeName => this.GetValue<string>("attribute-name");

        public MAObjectCDError CDError => this.GetObject<MAObjectCDError>("cd-error");
    }
}
