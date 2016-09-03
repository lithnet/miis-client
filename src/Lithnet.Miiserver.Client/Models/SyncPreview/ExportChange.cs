using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportChange : XmlObjectBase
    {
        internal ExportChange(XmlNode node)
            : base(node)
        {
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("@id");
            }
        }

        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("@ma-id");
            }
        }

        public string MAName
        {
            get
            {
                return this.GetValue<string>("@ma-name");
            }
        }

        public string CSOperation
        {
            get
            {
                return this.GetValue<string>("cs-operation");
            }
        }

        public Hologram Hologram
        {
            get
            {
                return this.GetObject<Hologram>("entry");
            }
        }

        public override string ToString()
        {
            return this.CSOperation;
        }
    }
}
