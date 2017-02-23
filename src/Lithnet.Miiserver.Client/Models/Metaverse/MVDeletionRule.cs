using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVDeletionRule : XmlObjectBase
    {
        internal MVDeletionRule(XmlNode node)
            :base (node)
        {
        }

        public string ObjectType => this.GetValue<string>("@mv-object-type");

        public Guid ID => this.GetValue<Guid>("@id");

        public string Type => this.GetValue<string>("@type");

        public IReadOnlyList<Guid> SourceMAs => this.GetReadOnlyValueList<Guid>("src-ma");
    }
}
/*
<mv-deletion-rule mv-object-type="calendar" id="{8FEAC6EA-054B-4C4B-9A8A-DB5E9539BF3D}" type="declared-any">
      <src-ma>{B19F1D5A-4DB6-4E11-83CD-76E1C5AF3D8A}</src-ma>
</mv-deletion-rule>
*/
