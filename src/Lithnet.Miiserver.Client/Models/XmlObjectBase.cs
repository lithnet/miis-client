using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents the base object used by sync service objects that are represented via XML nodes
    /// </summary>
    [Serializable]
    public abstract class XmlObjectBase : IDisposable, ISerializable
    {
        private const string DsmlUri = "http://www.dsml.org/DSML";
        private const string MsdsmlUri = "http://www.microsoft.com/MMS/DSML";
        private const string DsmlPrefix = "dsml";
        private const string MsdsmlPrefix = "ms-dsml";

        private bool disposedValue;

        protected internal XmlNamespaceManager NsManager;

        private Dictionary<string, object> cachedProperties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the internal XML node object
        /// </summary>
        protected internal XmlNode XmlNode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the XmlObjectBase class
        /// </summary>
        /// <param name="node"></param>
        protected XmlObjectBase(XmlNode node)
        {
            this.XmlNode = node;
            XmlDocument d = this.XmlNode.OwnerDocument;
            if (d != null)
            {
                this.NsManager = XmlObjectBase.GetNSManager(d.NameTable);
            }
        }

        protected XmlObjectBase(SerializationInfo info, StreamingContext context)
        {
            string xml = (string)info.GetValue("raw-xml", typeof(string));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            this.XmlNode = doc.FirstChild;
        }

        /// <summary>
        /// Clears the cache and reloads the object from the specified node
        /// </summary>
        /// <param name="node"></param>
        protected internal void Reload(XmlNode node)
        {
            this.XmlNode = node;
            this.ClearCache();
        }

        /// <summary>
        /// Clears the cache of deserialized objects
        /// </summary>
        protected internal void ClearCache()
        {
            this.cachedProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the value from the internal XML structure with the specified node name
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Returns a typed value for the specified element name</returns>
        protected internal T GetValue<T>(string name)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                T value = this.XmlNode.SelectSingleNode(name, this.NsManager).ReadInnerText<T>();
                this.cachedProperties.Add(name, value);
            }

            return (T)this.cachedProperties[name];
        }

        /// <summary>
        /// Gets a value from the internal XML structure as a typed read-only list
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Returns a read only list of values for the specified element name</returns>
        protected internal IReadOnlyList<T> GetReadOnlyValueList<T>(string name)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                List<T> list = new List<T>();

                foreach (XmlNode n1 in this.XmlNode.SelectNodes(name, this.NsManager))
                {
                    T v1 = n1.ReadInnerText<T>();

                    if (v1 != null)
                    {
                        list.Add(v1);
                    }
                }

                this.cachedProperties.Add(name, list.AsReadOnly());
            }

            return (IReadOnlyList<T>)this.cachedProperties[name];
        }

        /// <summary>
        /// Gets a read-only list of objects from an internal XML structure
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="name">The name of the attribute</param>
        /// <param name="parameters">A list of parameters to pass to the constructor for the type</param>
        /// <returns>Returns a read only list of objects for the specified element name</returns>
        protected internal IReadOnlyList<T> GetReadOnlyObjectList<T>(string name, params object[] parameters)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                List<T> list = new List<T>();

                foreach (XmlNode n1 in this.XmlNode.SelectNodes(name, this.NsManager))
                {
                    List<object> args = new List<object> { n1 };

                    if (parameters != null)
                    {
                        args.AddRange(parameters);
                    }

                    T v1 = (T)Activator.CreateInstance(
                        typeof(T),
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                        null,
                        args.ToArray(),
                        null);

                    list.Add(v1);
                }

                this.cachedProperties.Add(name, list.AsReadOnly());
            }

            return (IReadOnlyList<T>)this.cachedProperties[name];
        }

        /// <summary>
        /// Gets a read-only dictionary of objects from an internal XML structure
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <typeparam name="TValue">The type of the value</typeparam>
        /// <param name="name">The name of the xml node</param>
        /// <param name="keySelector">A function that returns the key from the created object</param>
        /// <param name="keyComparer">A comparer used to compare equality of the keys in the dictionary</param>
        /// <param name="parameters">A list of parameters to pass to the constructor for the type</param>
        /// <returns>Returns a read only dictionary of objects for the specified element name</returns>
        protected internal IReadOnlyDictionary<TKey, TValue> GetReadOnlyObjectDictionary<TKey, TValue>(string name, Func<TValue, TKey> keySelector, IEqualityComparer<TKey> keyComparer, params object[] parameters)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                Dictionary<TKey, TValue> list = new Dictionary<TKey, TValue>(keyComparer);

                foreach (XmlNode n1 in this.XmlNode.SelectNodes(name, this.NsManager))
                {
                    List<object> args = new List<object> { n1 };

                    if (parameters != null)
                    {
                        args.AddRange(parameters);
                    }

                    TValue v1 = (TValue)Activator.CreateInstance(
                        typeof(TValue),
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                        null,
                        args.ToArray(),
                        null);

                    list.Add(keySelector.Invoke(v1), v1);
                }

                this.cachedProperties.Add(name, new ReadOnlyDictionary<TKey, TValue>(list));
            }

            return (IReadOnlyDictionary<TKey, TValue>)this.cachedProperties[name];
        }

        /// <summary>
        /// Gets an object represented by the specified XML node
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="nodeName">The name of the XML node</param>
        /// <param name="passNodeListToConstructor">A flag that indicated if a single or multiple nodes are expected to match the given name</param>
        /// <returns>Returns an object for the specified element name</returns>
        protected internal T GetObject<T>(string nodeName, bool passNodeListToConstructor) where T : class
        {
            if (!passNodeListToConstructor)
            {
                return this.GetObject<T>(nodeName);
            }

            if (!this.cachedProperties.ContainsKey(nodeName))
            {
                this.cachedProperties.Add(
                    nodeName,
                    Activator.CreateInstance(
                        typeof(T),
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                        null,
                        new object[] { this.XmlNode.SelectNodes(nodeName, this.NsManager) },
                        null));
            }

            return (T)this.cachedProperties[nodeName];
        }

        /// <summary>
        /// Gets an object from the specified node name
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="nodeName">The name of the XML node</param>
        /// <param name="parameters">The parameters to pass to the constructor of the specified type</param>
        /// <returns>An object representing the specified node</returns>
        protected internal T GetObject<T>(string nodeName, params object[] parameters) where T : class
        {
            if (!this.cachedProperties.ContainsKey(nodeName))
            {
                List<object> args = new List<object>();

                XmlNode n1 = this.XmlNode.SelectSingleNode(nodeName, this.NsManager);
                if (n1 == null)
                {
                    this.cachedProperties.Add(nodeName, null);
                }
                else
                {
                    args.Add(n1);
                    if (parameters != null)
                    {
                        args.AddRange(parameters);
                    }

                    this.cachedProperties.Add(
                        nodeName,
                        Activator.CreateInstance(
                            typeof(T),
                            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                            null,
                            args.ToArray(),
                            null));
                }
            }

            return (T)this.cachedProperties[nodeName];
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        /// <param name="disposing">A flag that indicates if the object is being disposed, or finalized through the GC</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.cachedProperties = null;
                    this.XmlNode = null;
                    this.NsManager = null;
                }

                this.disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Gets the XML representation of the object
        /// </summary>
        /// <returns>An XML string</returns>
        public string GetOuterXml()
        {
            return this.XmlNode.OuterXml;
        }

        /// <summary>
        /// Writes the outer XML to a file
        /// </summary>
        /// <param name="file">The file to write to</param>
        internal void DropOuterXml(string file)
        {
            System.IO.File.WriteAllText(file, this.GetOuterXml());
        }

        /// <summary>
        /// Gets the namespace manager for the specified XmlNameTable
        /// </summary>
        /// <param name="n">The XmlNameTable to build the namespace manager for</param>
        /// <returns>An XmlNamespaceManager</returns>
        internal static XmlNamespaceManager GetNSManager(XmlNameTable n)
        {
            XmlNamespaceManager nsmanager = new XmlNamespaceManager(n);
            nsmanager.AddNamespace(XmlObjectBase.DsmlPrefix, XmlObjectBase.DsmlUri);
            nsmanager.AddNamespace(XmlObjectBase.MsdsmlPrefix, XmlObjectBase.MsdsmlUri);

            return nsmanager;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("raw-xml", this.GetOuterXml(), typeof(string));
        }

        public string ToJson()
        {
            return JsonConvert.SerializeXmlNode(this.XmlNode);
        }
    }
}