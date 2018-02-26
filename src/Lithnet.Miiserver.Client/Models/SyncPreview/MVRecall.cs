using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVRecall : XmlObjectBase
    {
        internal MVRecall(XmlNode node)
            : base(node)
        {
        }

        public Error Error => this.GetObject<Error>("import-attribute-flow/error");

        public bool HasError => this.GetValue<bool>("import-attribute-flow/@has-error");

        public IReadOnlyList<RepopulationOperation> RepopulationOperations => this.GetReadOnlyObjectList<RepopulationOperation>("import-attribute-flow/repopulation-operation");

        public Delta MVChanges => this.GetObject<Delta>("import-attribute-flow/values/delta");

        public IReadOnlyList<CSObject> ReferencedConnectors => this.GetReadOnlyObjectList<CSObject>("import-attribute-flow/referenced-connectors/cs-object");
    }
}