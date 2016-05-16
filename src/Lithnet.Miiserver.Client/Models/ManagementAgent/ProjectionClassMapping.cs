namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class ProjectionClassMapping : NodeCache
    {
        internal ProjectionClassMapping(XmlNode node)
            : base(node)
        {
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("@id");
            }
        }

        public string Type
        {
            get
            {
                return this.GetValue<string>("@type");
            }
        }

        public string CDObjectType 
        {
            get
            {
                return this.GetValue<string>("@cd-object-type");
            }
        }

        public string MVObjectType
        {
            get
            {
                return this.GetValue<string>("mv-object-type");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", this.CDObjectType, this.MVObjectType);
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
