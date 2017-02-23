using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportError : XmlObjectBase
    {
        internal ExportError(XmlNode node)
            : base(node)
        {
        }

        public DateTime DateOccurred => this.GetValue<DateTime>("date-occurred");

        public DateTime FirstOccurred => this.GetValue<DateTime>("first-occurred");

        public int RetryCount => this.GetValue<int>("retry-count");

        public string ErrorType => this.GetValue<string>("error-type");

        public ExportCDError CDError => this.GetObject<ExportCDError>("cd-error");

        public Guid CSGuid => this.GetValue<Guid>("@cs-guid");

        public string DN => this.GetValue<string>("@dn");
    }
}
