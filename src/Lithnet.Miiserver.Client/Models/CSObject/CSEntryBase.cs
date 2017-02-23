using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// A common base object that represents all types of CSEntry objects
    /// </summary>
    public abstract class CSEntryBase : XmlObjectBase
    {
        internal CSEntryBase(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the anchor value of the object
        /// </summary>
        public EncodedValue Anchor => this.GetObject<EncodedValue>("anchor");

        /// <summary>
        /// Gets the anchor value of this object's parent
        /// </summary>
        public EncodedValue ParentAnchor => this.GetObject<EncodedValue>("parent-anchor");

        /// <summary>
        /// Gets the DN attribute for this object
        /// </summary>
        public IReadOnlyDictionary<string, DNAttribute> DNAttributes => this.GetReadOnlyObjectDictionary<string, DNAttribute>("dn-attr", (t) => t.Name, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the DN of this object
        /// </summary>
        public string DN => this.GetValue<string>("@dn");
        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.DN;
        }
    }
}