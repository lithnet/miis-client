namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Diagnostics;

    public class ExportError : NodeCache
    {
        internal ExportError(XmlNode node)
            : base(node)
        {
        }

        public DateTime DateOccurred
        {
            get
            {
                return this.GetValue<DateTime>("date-occurred");
            }
        }

        public DateTime FirstOccurred
        {
            get
            {
                return this.GetValue<DateTime>("first-occurred");
            }
        }

        public int RetryCount
        {
            get
            {
                return this.GetValue<int>("retry-count");
            }
        }

        public string ErrorType
        {
            get
            {
                return this.GetValue<string>("error-type");
            }
        }

        public ExportCDError CDError
        {
            get
            {
                return this.GetObject<ExportCDError>("cd-error");
            }
        }

        public Guid CSGuid
        {
            get
            {
                return this.GetValue<Guid>("@cs-guid");
            }
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("@dn");
            }
        }
    }
}
