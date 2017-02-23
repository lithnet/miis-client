using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVObjectCollection : XmlObjectBase, IReadOnlyList<MVObject>
    {
        internal MVObjectCollection(XmlDocument node)
            :base(node)
        {
            this.MVObjects = this.GetReadOnlyObjectList<MVObject>("/results/mv-objects/mv-object");
        }

        private IReadOnlyList<MVObject> MVObjects { get; set; }
       
        public int Count => this.MVObjects.Count;

        public MVObject this[int index] => this.MVObjects[index];

        public IEnumerator<MVObject> GetEnumerator()
        {
            return this.MVObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.MVObjects.GetEnumerator();
        }
    }
}
