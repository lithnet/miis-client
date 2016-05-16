using System;
using System.Collections.Generic;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    using System.Linq;
    using System.Management;
    using System.Security;

    public class CSObject : CSObjectBase
    {
        private static MMSWebService ws = new MMSWebService();

        internal CSObject(XmlNode node)
            : base(node)
        {
        }

        public void SetConnectorState(ConnectorState connectorState)
        {
            string result = ws.SetConnectorState(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), (CONNECTORSTATE)connectorState);
            CSObject.ThrowExceptionOnReturnError(result);
            this.Refresh();
        }

        public void Disconnect(bool makeExplicit)
        {
            string result = ws.Disconnect(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid());
            CSObject.ThrowExceptionOnReturnError(result);

            if (makeExplicit)
            {
                result = ws.SetExplicit(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), makeExplicit);
                CSObject.ThrowExceptionOnReturnError(result);
            }

            this.Refresh();
        }

        public MVObject Project(string objectType)
        {
            string result = ws.Project(this.MAID.ToMmsGuid(), objectType, this.ID.ToMmsGuid());

            Guid mvid;

            if (Guid.TryParse(result, out mvid))
            {
                this.Refresh();
                return SyncServer.GetMVObject(mvid);
            }
            else
            {
                CSObject.ThrowExceptionOnReturnError(result);
                return null;
            }
        }

        public bool WillDeleteMVObjectOnDisconnect()
        {
            int willDelete = 0;
            string result = ws.CSObjectWillBeDeleted(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), ref willDelete);
            CSObject.ThrowExceptionOnReturnError(result);

            return willDelete == 1;
        }

        public void Disconnect()
        {
            this.Disconnect(false);
        }

        public IEnumerable<CSMVLink> ConnectedCSObjectLinks
        {
            get
            {
                if (this.MvGuid == null)
                {
                    return new List<CSMVLink>();
                }

                return CSObject.GetConnectedCSObjectLinks(this.MvGuid.Value);
            }
        }

        public SyncPreview Sync(bool commit, bool delta)
        {
            string result = ws.Preview(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), delta, commit);
            CSObject.ThrowExceptionOnReturnError(result);
            XmlDocument d = new XmlDocument();
            d.LoadXml(result);

            if (commit)
            {
                this.Refresh();
            }

            return new SyncPreview(d.SelectSingleNode("/preview"));
        }

        public void Join(MVObject mventry)
        {
            this.Join(mventry.ObjectType, mventry.DN);
            this.Refresh();
        }

        public void Join(string mvObjectType, Guid mvObjectId)
        {
            string result = ws.Join(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), mvObjectType, mvObjectId.ToMmsGuid());
            CSObject.ThrowExceptionOnReturnError(result);
            this.Refresh();
        }
        public IEnumerable<CSObject> ConnectedCSObjects
        {
            get
            {
                if (this.MvGuid == null)
                {
                    return new List<CSObject>();
                }

                return CSObject.GetConnectedCSObjects(this.MvGuid.Value);
            }
        }

        public static IEnumerable<CSObject> GetConnectedCSObjects(Guid mvObjectId)
        {
            foreach (CSMVLink link in CSObject.GetConnectedCSObjectLinks(mvObjectId))
            {
                yield return link.GetCSObject();
            }
        }

        public static IEnumerable<CSMVLink> GetConnectedCSObjectLinks(Guid mvObjectId)
        {
            string results = ws.GetMVConnectors(mvObjectId.ToMmsGuid());
            XmlDocument d = new XmlDocument();
            d.LoadXml(results);

            foreach (XmlNode node in d.SelectNodes("/cs-mv-links/cs-mv-value"))
            {
                yield return new CSMVLink(node);
            }
        }

        public static CSObject GetCSObject(Guid id)
        {
            return CSObject.GetCSObject(id.ToMmsGuid());
        }

        internal static CSObject GetCSObject(string id)
        {
            XmlNode node = CSObject.GetCSObjectXml(id);
            if (node != null)
            {
                return new CSObject(node);
            }
            else
            {
                return null;
            }
        }

        protected static XmlNode GetCSObjectXml(string id)
        {
            string xml = ws.GetCSObjects(new string[] { id }, 1, 0xFFFFFFFF, 0xFFFFFFFF, 0, null);
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);
            XmlNode node = d.SelectSingleNode("cs-objects/cs-object");

            return node;
        }

        public static void SetConnectorState(Guid id, Guid maid, ConnectorState connectorState)
        {
            CSObject.ws.SetConnectorState(maid.ToMmsGuid(), id.ToMmsGuid(), (CONNECTORSTATE)connectorState);
        }

        public void Refresh()
        {
            XmlNode node = CSObject.GetCSObjectXml(this.ID.ToMmsGuid());
            this.node = node;
            this.ClearCache();
        }

        public void SetPassword(SecureString password, bool forceChangeAtLogin, bool unlockAccount, bool enforcePasswordPolicy)
        {
            this.SetPassword(password.ConvertToUnsecureString(), forceChangeAtLogin, unlockAccount, enforcePasswordPolicy);
        }

        public void SetPassword(string password, bool forceChangeAtLogin, bool unlockAccount, bool enforcePasswordPolicy)
        {
            ManagementObject mo = CSObject.GetWmiObject(this.ID);

            string result = mo.InvokeMethod("SetPassword", new object[] { password, forceChangeAtLogin, unlockAccount, enforcePasswordPolicy }) as string;

            if (result == "access-denied")
            {
                throw new UnauthorizedAccessException();
            }
            else if (result != "success")
            {
                throw new MiiserverException($"The operation returned {result}");
            }
        }

        public void ChangePassword(SecureString oldPassword, SecureString newPassword)
        {
            this.ChangePassword(oldPassword.ConvertToUnsecureString(), newPassword.ConvertToUnsecureString());
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            ManagementObject mo = CSObject.GetWmiObject(this.ID);

            string result = mo.InvokeMethod("ChangePassword", new object[] { oldPassword, newPassword }) as string;

            if (result == "access-denied")
            {
                throw new UnauthorizedAccessException();
            }
            else if (result != "success")
            {
                throw new MiiserverException($"The operation returned {result}");
            }
        }
        private static void ThrowExceptionOnReturnError(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            XmlDocument d = new XmlDocument();
            XmlNode error = null;

            try
            {
                d.LoadXml(result);
                error = d.SelectSingleNode("/error");
            }
            catch { }

            if (error != null)
            {
                throw new MiiserverException(error.InnerText);
            }
        }

        private static ManagementObject GetWmiObject(Guid id)
        {
            ObjectQuery query = new ObjectQuery($"SELECT * FROM MIIS_CSObject where Guid='{id.ToMmsGuid()}'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.scope, query);
            ManagementObjectCollection results = searcher.Get();

            if (results.Count == 0)
            {
                return null;
            }
            else if (results.Count > 1)
            {
                throw new TooManyResultsException();
            }
            else
            {
                return results.OfType<ManagementObject>().First();
            }
        }
    }
}