namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class ConnectorAdd : XmlObjectBase
    {
        internal ConnectorAdd(XmlNode node)
            : base(node)
        {
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("@status");
            }
        }

        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("ma-guid");
            }
        }

        public string MAName
        {
            get
            {
                return this.GetValue<string>("ma-name");
            }
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("dn");
            }
        }

        public Guid ID
        {
            get
            {
                return this.GetValue<Guid>("object-id");
            }
        }

        public string ObjectClass
        {
            get
            {
                return this.GetValue<string>("primary-objectclass");
            }
        }

        public IReadOnlyList<string> ObjectClasses
        {
            get
            {
                return this.GetReadOnlyValueList<string>("objectclass/oc-value");
            }
        }

        public override string ToString()
        {
            return this.DN;
        }
    }
}

/*
 <add-connector status="success">
    <ma-guid>{1ADABA09-3898-4D08-8134-3DDDA6EB6F91}</ma-guid>
    <ma-name>zaf.local MA</ma-name>
    <dn>CN=S-mnhs-sobs-physiology-TBI\0ACNF:e91356f8-c2ae-4b88-adb6-ac7644774fd1,OU=Public,OU=Groups,OU=IdM Managed Objects,DC=zaf,DC=local</dn>
    <object-id>{9161B355-2208-E611-A04C-005056BA017A}</object-id>
    <primary-objectclass>group</primary-objectclass>
    <objectclass>
        <oc-value>group</oc-value>
    </objectclass>
</add-connector>
*/
