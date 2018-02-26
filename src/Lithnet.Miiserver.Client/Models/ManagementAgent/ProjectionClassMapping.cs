using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ProjectionClassMapping : XmlObjectBase
    {
        internal ProjectionClassMapping(XmlNode node)
            : base(node)
        {
        }

        public Guid ID => this.GetValue<Guid>("@id");

        public string Type => this.GetValue<string>("@type");

        public string CDObjectType => this.GetValue<string>("@cd-object-type");

        public string MVObjectType => this.GetValue<string>("mv-object-type");

        public override string ToString()
        {
            return $"{this.CDObjectType} -> {this.MVObjectType}";
        }
    }
}

/*
  <projection>
    <class-mapping type="declared" id="{7C5C0774-C80B-4F55-81DC-F07CAD975119}" cd-object-type="organizationalUnit">
      <mv-object-type>organizationalUnit</mv-object-type>
    </class-mapping>
    <class-mapping type="declared" id="{3BC8455C-4DD2-4D92-BB8A-27BCE0282B2B}" cd-object-type="person">
      <mv-object-type>user</mv-object-type>
    </class-mapping>
  </projection>

*/
