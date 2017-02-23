using System.Xml;
using System.Collections.Generic;

namespace Lithnet.Miiserver.Client
{
    public class ProvisioningResult : XmlObjectBase
    {
        internal ProvisioningResult(XmlNode node)
            : base(node)
        {
        }

        public bool HasError => this.GetValue<bool>("@has-error");

        public IReadOnlyList<ConnectorAdd> ConnectorAdds => this.GetReadOnlyObjectList<ConnectorAdd>("add-connector");

        public IReadOnlyList<ConnectorRename> ConnectorRenames => this.GetReadOnlyObjectList<ConnectorRename>("rename-connector");

        public IReadOnlyList<Delta> InitialFlows => this.GetReadOnlyObjectList<Delta>("export-flow-rules/export-attribute-flow/values");

        public Error Error => this.GetObject<Error>("error");
    }
}

/*
<provisioning has-error="false">
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
      <export-flow-rules>
        <export-attribute-flow>
          <values object-id="{9161B355-2208-E611-A04C-005056BA017A}">
            <delta operation="add" dn="CN=S-mnhs-sobs-physiology-TBI\0ACNF:e91356f8-c2ae-4b88-adb6-ac7644774fd1,OU=Public,OU=Groups,OU=IdM Managed Objects,DC=zaf,DC=local">
              <attr name="cn" type="string" multivalued="false">
                <value>
                  S-mnhs-sobs-physiology-TBI
                  CNF:e91356f8-c2ae-4b88-adb6-ac7644774fd1
                </value>
              </attr>
              <attr name="groupType" type="integer" multivalued="false">
                <value>0xffffffff80000002</value>
              </attr>
            </delta>
          </values>
        </export-attribute-flow>
      </export-flow-rules>
      <error type="extension-dll-exception">
        <extension-error-info>
          <extension-name>ADExportMVExtension.dll</extension-name>
          <extension-callsite>provisioning</extension-callsite>
          <call-stack>
            Microsoft.MetadirectoryServices.ObjectAlreadyExistsException: An object with DN "CN=S-mnhs-sobs-physiology-TBI,OU=Public,OU=Groups,OU=IdM Managed Objects,DC=zaf,DC=local" already exists in management agent "zaf.local MA".
            at Microsoft.MetadirectoryServices.Impl.ConnectorImpl.Commit()
            at Mms_Metaverse.ADObject.AddGroupToConnectorSpace(MVEntry mventry, ConnectedMA managementAgent) in D:\FIM Extension Source\ADExportMVExtension\ADObject.cs:line 135
            at Mms_Metaverse.ADObject.AddADObjectToConnectorSpace(MVEntry mventry, ConnectedMA managementAgent) in D:\FIM Extension Source\ADExportMVExtension\ADObject.cs:line 82
            at Mms_Metaverse.ADExportMVExtension.Microsoft.MetadirectoryServices.IMVSynchronization.Provision(MVEntry mventry) in D:\FIM Extension Source\ADExportMVExtension\ADExportMVExtension.cs:line 55
          </call-stack>
        </extension-error-info>
      </error>
    </provisioning>
</provisioning-rules>
*/
