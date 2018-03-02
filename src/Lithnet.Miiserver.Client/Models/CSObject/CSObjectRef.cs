using System;
using System.Runtime.Serialization;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// A lightweight reference to a CSObject
    /// </summary>
    [Serializable]
    public class CSObjectRef : XmlObjectBase
    {
        internal CSObjectRef(XmlNode node)
            : base(node)
        {
        }

        protected CSObjectRef(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        /*
        <step-object-details step-id="{7C44FD84-3CA4-4286-A750-3114FE7A0223}">
        <cs-object id="{F650712A-7013-E711-9160-005056B50BB9}" cs-dn="dn1"/>
        <cs-object id="{EAA1C9A7-6F13-E711-9160-005056B50BB9}" cs-dn="dn2"/>
        </step-object-details>*/

        /// <summary>
        /// Gets the anchor value of the object
        /// </summary>
        public Guid ID => this.GetValue<Guid>("@id");

        /// <summary>
        /// Gets the anchor value of this object's parent
        /// </summary>
        public string DN => this.GetValue<string>("@cs-dn");

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.DN;
        }
    }
}