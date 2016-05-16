namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class MVRecall : NodeCache
    {
        internal MVRecall(XmlNode node)
            : base(node)
        {
        }

        public Error Error
        {
            get
            {
                return this.GetObject<Error>("import-attribute-flow/error");
            }
        }

        public bool HasError
        {
            get
            {
                return this.GetValue<bool>("import-attribute-flow/@has-error");
            }
        }

        public IReadOnlyList<RepopulationOperation> RepopulationOperations
        {
            get
            {
                return this.GetReadOnlyObjectList<RepopulationOperation>("import-attribute-flow/repopulation-operation");
            }
        }

        public Delta MVChanges
        {
            get
            {
                return this.GetObject<Delta>("import-attribute-flow/values/delta");
            }
        }

        public IReadOnlyList<CSObject> ReferencedConnectors
        {
            get
            {
                return this.GetReadOnlyObjectList<CSObject>("import-attribute-flow/referenced-connectors/cs-object");
            }
        }
    }
}