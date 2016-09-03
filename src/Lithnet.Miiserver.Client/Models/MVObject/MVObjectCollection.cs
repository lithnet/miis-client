using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
       
        public int Count
        {
            get
            {
                return this.MVObjects.Count;
            }
        }

        public MVObject this[int index]
        {
            get
            {
                return this.MVObjects[index];
            }
        }

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
