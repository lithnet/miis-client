using System;
using System.Collections.Generic;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
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
            this.ThrowExceptionOnReturnError(result);
            this.Refresh();
        }

        public void Disconnect(bool makeExplicit)
        {
            string result = ws.Disconnect(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid());
            this.ThrowExceptionOnReturnError(result);

            if (makeExplicit)
            {
                result = ws.SetExplicit(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), makeExplicit);
                this.ThrowExceptionOnReturnError(result);
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
                this.ThrowExceptionOnReturnError(result);
                return null;
            }
        }

        public bool WillDeleteMVObjectOnDisconnect()
        {
            int willDelete = 0;
            string result = ws.CSObjectWillBeDeleted(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), ref willDelete);
            this.ThrowExceptionOnReturnError(result);

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
            this.ThrowExceptionOnReturnError(result);
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

        public void Join(string mvObjectType, Guid mvObjectID)
        {
            string result = ws.Join(this.MAID.ToMmsGuid(), this.ID.ToMmsGuid(), mvObjectType, mvObjectID.ToMmsGuid());
            this.ThrowExceptionOnReturnError(result);
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

        public static IEnumerable<CSObject> GetConnectedCSObjects(Guid mvObjectID)
        {
            foreach (CSMVLink link in CSObject.GetConnectedCSObjectLinks(mvObjectID))
            {
                yield return link.GetCSObject();
            }
        }

        public static IEnumerable<CSMVLink> GetConnectedCSObjectLinks(Guid mvObjectID)
        {
            string results = ws.GetMVConnectors(mvObjectID.ToMmsGuid());
            XmlDocument d = new XmlDocument();
            d.LoadXml(results);

            foreach (XmlNode node in d.SelectNodes("/cs-mv-links/cs-mv-value"))
            {
                yield return new CSMVLink(node);
            }
        }

        public static CSObject GetCSObject(Guid ID)
        {
            return CSObject.GetCSObject(ID.ToMmsGuid());
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

        public static void SetConnectorState(Guid ID, Guid MAID, ConnectorState connectorState)
        {
            CSObject.ws.SetConnectorState(MAID.ToMmsGuid(), ID.ToMmsGuid(), (CONNECTORSTATE)connectorState);
        }

        public void Refresh()
        {
            XmlNode node = CSObject.GetCSObjectXml(this.ID.ToMmsGuid());
            this.node = node;
            this.ClearCache();
        }

        private void ThrowExceptionOnReturnError(string result)
        {
            if (!string.IsNullOrWhiteSpace(result))
            {
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
        }
    }
}