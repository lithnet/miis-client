namespace Lithnet.Miiserver.Client
{
    using System.Xml;

    public class ExportChangeNotReimportedError : NodeCache
    {
        internal ExportChangeNotReimportedError (XmlNode node)
            :base(node)
        {
        }

        public Delta Delta
        {
            get
            {
                return this.GetObject<Delta>("delta");
            }
        }

        public Hologram Entry
        {
            get
            {
                return this.GetObject<Hologram>("entry");
            }
        }
    }
}
