using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportChangeNotReimportedError : XmlObjectBase
    {
        internal ExportChangeNotReimportedError (XmlNode node)
            :base(node)
        {
        }

        public Delta Delta => this.GetObject<Delta>("delta");

        public Hologram Entry => this.GetObject<Hologram>("entry");
    }
}
