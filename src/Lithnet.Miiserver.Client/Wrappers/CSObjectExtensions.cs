using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security;
using System.Xml;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents a connector space object
    /// </summary>
    public static class CSObjectExtensions
    {
        private static MMSWebService ws = new MMSWebService();


        public static CSObjectBase GetCSObject(this CSObjectRef cSObjectRef)
        {
            return CSObjectExtensions.GetCSObject(cSObjectRef.ID);
        }

        /// <summary>
        /// Sets the state of the connector space object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="connectorState">The state to set the connector to</param>
        public static void SetConnectorState(this CSObjectBase csobject, ConnectorState connectorState)
        {
            string result = ws.SetConnectorState(csobject.MAID.ToMmsGuid(), csobject.ID.ToMmsGuid(), (CONNECTORSTATE)connectorState);
            SyncServer.ThrowExceptionOnReturnError(result);
            csobject.Refresh();
        }

        /// <summary>
        /// Disconnects a connector space object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="makeExplicit">A value indicating whether an explicit disconnector should be created</param>
        public static void Disconnect(this CSObjectBase csobject, bool makeExplicit)
        {
            string result = ws.Disconnect(csobject.MAID.ToMmsGuid(), csobject.ID.ToMmsGuid());
            SyncServer.ThrowExceptionOnReturnError(result);

            if (makeExplicit)
            {
                result = ws.SetExplicit(csobject.MAID.ToMmsGuid(), csobject.ID.ToMmsGuid(), true);
                SyncServer.ThrowExceptionOnReturnError(result);
            }

            csobject.Refresh();
        }

        /// <summary>
        /// Projects a connector space object to the metaverse
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="objectType">The name of the metaverse object type to project the connector space object as</param>
        /// <returns>Returns the newly created metaverse object</returns>
        public static MVObject Project(this CSObjectBase csobject, string objectType)
        {
            SyncServer.ThrowOnInvalidObjectType(objectType);

            string result = ws.Project(csobject.MAID.ToMmsGuid(), objectType, csobject.ID.ToMmsGuid());

            if (Guid.TryParse(result, out Guid mvid))
            {
                csobject.Refresh();
                return SyncServer.GetMVObject(mvid);
            }
            else
            {
                SyncServer.ThrowExceptionOnReturnError(result);
                return null;
            }
        }

        /// <summary>
        /// Gets a value that indicates if the metaverse object will be deleted when this object is disconnected
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <returns>This method returns true if the object deletion rule of the object will be satisfied as a result of disconnecting the object</returns>
        public static bool WillDeleteMVObjectOnDisconnect(this CSObjectBase csobject)
        {
            int willDelete = 0;
            string result = ws.CSObjectWillBeDeleted(csobject.MAID.ToMmsGuid(), csobject.ID.ToMmsGuid(), ref willDelete);
            SyncServer.ThrowExceptionOnReturnError(result);

            return willDelete == 1;
        }

        /// <summary>
        /// Disconnects the object from the metaverse, leaving it as a normal disconnector
        /// </summary>
        public static void Disconnect(this CSObjectBase csobject)
        {
            csobject.Disconnect(false);
        }

        /// <summary>
        /// Gets the links to the other connector space objects linked to this object via the metaverse
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        public static IEnumerable<CSMVLink> GetConnectedCSObjectLinks(this CSObjectBase csobject)
        {
            if (csobject.MvGuid == null)
            {
                return new List<CSMVLink>();
            }

            return CSObjectExtensions.GetConnectedCSObjectLinks(csobject.MvGuid.Value);
        }

        /// <summary>
        /// Performs a synchronization of the connector space object, and optionally commits the result of the synchronization to the metaverse
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="commit">A value indicating if the synchronization should be committed</param>
        /// <param name="delta">A value indicating that a delta synchronization, rather than a full synchronization should be performed</param>
        /// <returns>An object representing the results of the synchronization operation</returns>
        public static SyncPreview Sync(this CSObjectBase csobject, bool commit, bool delta)
        {
            string result = ws.Preview(csobject.MAID.ToMmsGuid(), csobject.ID.ToMmsGuid(), delta, commit);
            SyncServer.ThrowExceptionOnReturnError(result);
            XmlDocument d = new XmlDocument();
            d.LoadXml(result);

            if (commit)
            {
                csobject.Refresh();
            }

            return new SyncPreview(d.SelectSingleNode("/preview"));
        }

        /// <summary>
        /// Joins the connector space object to the specified metaverse entry
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="mventry">The metaverse entry to join</param>
        public static void Join(this CSObjectBase csobject, MVObject mventry)
        {
            csobject.Join(mventry.ObjectType, mventry.DN);
            csobject.Refresh();
        }

        /// <summary>
        /// Joins the connector space object to the metaverse object with the specified type and ID
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="mvObjectType">The type of metaverse object to join</param>
        /// <param name="mvObjectId">The ID of the metaverse object to join</param>
        public static void Join(this CSObjectBase csobject, string mvObjectType, Guid mvObjectId)
        {
            SyncServer.ThrowOnInvalidObjectType(mvObjectType);

            string result = ws.Join(csobject.MAID.ToMmsGuid(), csobject.ID.ToMmsGuid(), mvObjectType, mvObjectId.ToMmsGuid());
            SyncServer.ThrowExceptionOnReturnError(result);
            csobject.Refresh();
        }

        /// <summary>
        /// Gets the connector space objects linked to this object via the metaverse object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        public static IEnumerable<CSObjectBase> GetConnectedCSObjects(this CSObjectBase csobject)
        {
            if (csobject.MvGuid == null)
            {
                return new List<CSObjectBase>();
            }

            return CSObjectExtensions.GetConnectedCSObjects(csobject.MvGuid.Value);
        }

        /// <summary>
        /// Gets the connector space objects connected to the specified metaverse object
        /// </summary>
        /// <param name="mvObjectId">The ID of the metaverse object</param>
        /// <returns>An enumeration of connector space objects</returns>
        public static IEnumerable<CSObjectBase> GetConnectedCSObjects(Guid mvObjectId)
        {
            return CSObjectExtensions.GetConnectedCSObjectLinks(mvObjectId).Select(link => link.GetCSObject());
        }

        /// <summary>
        /// Gets the connector space object links for the specified metaverse object
        /// </summary>
        /// <param name="mvObjectId">The ID of the metaverse object</param>
        /// <returns>An enumeration of connector space object links</returns>
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

        /// <summary>
        /// Gets a connector space object
        /// </summary>
        /// <param name="id">The ID of the connector space object</param>
        /// <returns>A connector space object</returns>
        public static CSObjectBase GetCSObject(Guid id)
        {
            XmlNode node = CSObjectExtensions.GetCSObjectXml(id);
            if (node != null)
            {
                return new CSObjectBase(node);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a connector space object
        /// </summary>
        /// <param name="id">The ID of the object to get</param>
        /// <returns>A connector space object</returns>
        internal static XmlNode GetCSObjectXml(Guid id)
        {
            string xml = ws.GetCSObjects(new[] { id.ToMmsGuid() }, 1, 0xFFFFFFFF, 0xFFFFFFFF, 0, null);
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);
            XmlNode node = d.SelectSingleNode("cs-objects/cs-object");

            return node;
        }

        /// <summary>
        /// Sets the connector state of a specified connector
        /// </summary>
        /// <param name="id">The ID of the connector space object</param>
        /// <param name="maid">The ID of the management agent containing the connector space object</param>
        /// <param name="connectorState">The state of the connector to set</param>
        public static void SetConnectorState(Guid id, Guid maid, ConnectorState connectorState)
        {
            CSObjectExtensions.ws.SetConnectorState(maid.ToMmsGuid(), id.ToMmsGuid(), (CONNECTORSTATE)connectorState);
        }

        /// <summary>
        /// Reloads the connector space object from the metadirectory
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        public static void Refresh(this CSObjectBase csobject)
        {
            XmlNode node = CSObjectExtensions.GetCSObjectXml(csobject.ID);
            csobject.Reload(node);
        }

        /// <summary>
        /// Sets the password for the connector space object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="password">The password to set</param>
        /// <param name="forceChangeAtLogin">A value that indicates whether the password should be changed at the next login</param>
        /// <param name="unlockAccount">A value that indicates whether the account should be unlocked</param>
        /// <param name="enforcePasswordPolicy">A value that indicates whether the password policy should be enforced</param>
        public static void SetPassword(this CSObjectBase csobject, SecureString password, bool forceChangeAtLogin, bool unlockAccount, bool enforcePasswordPolicy)
        {
            csobject.SetPassword(password.ConvertToUnsecureString(), forceChangeAtLogin, unlockAccount, enforcePasswordPolicy);
        }

        /// <summary>
        /// Sets the password for the connector space object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="password">The password to set</param>
        /// <param name="forceChangeAtLogin">A value that indicates whether the password should be changed at the next login</param>
        /// <param name="unlockAccount">A value that indicates whether the account should be unlocked</param>
        /// <param name="enforcePasswordPolicy">A value that indicates whether the password policy should be enforced</param>
        public static void SetPassword(this CSObjectBase csobject, string password, bool forceChangeAtLogin, bool unlockAccount, bool enforcePasswordPolicy)
        {
            ManagementObject mo = CSObjectExtensions.GetWmiObject(csobject.ID);

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

        /// <summary>
        /// Changes the password for a connector space object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="oldPassword">The user's old password</param>
        /// <param name="newPassword">The user's new password</param>
        public static void ChangePassword(this CSObjectBase csobject, SecureString oldPassword, SecureString newPassword)
        {
            csobject.ChangePassword(oldPassword.ConvertToUnsecureString(), newPassword.ConvertToUnsecureString());
        }

        /// <summary>
        /// Changes the password for a connector space object
        /// </summary>
        /// <param name="csobject">A connector space object</param>
        /// <param name="oldPassword">The user's old password</param>
        /// <param name="newPassword">The user's new password</param>
        public static void ChangePassword(this CSObjectBase csobject, string oldPassword, string newPassword)
        {
            ManagementObject mo = CSObjectExtensions.GetWmiObject(csobject.ID);

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

        /// <summary>
        /// Gets the connector space object from WMI
        /// </summary>
        /// <param name="id">The ID of the connector space object</param>
        /// <returns>A WMI object representing the connector space object</returns>
        private static ManagementObject GetWmiObject(Guid id)
        {
            ObjectQuery query = new ObjectQuery($"SELECT * FROM MIIS_CSObject where Guid='{id.ToMmsGuid()}'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(SyncServer.Scope, query);
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