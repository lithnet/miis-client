using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Collections.ObjectModel;

namespace Lithnet.Miiserver.Client
{
    public abstract class NodeCache : IDisposable
    {
        private const string dsmlUri = "http://www.dsml.org/DSML";
        private const string msdsmlUri = "http://www.microsoft.com/MMS/DSML";
        private const string dsmlPrefix = "dsml";
        private const string msdsmlPrefix = "ms-dsml";

        private bool disposedValue = false;

        protected XmlNode node;

        protected XmlNamespaceManager nsmanager;

        protected Dictionary<string, object> cachedProperties = new Dictionary<string, object>();

        protected NodeCache(XmlNode node)
        {
            this.node = node;
            XmlDocument d = this.node.OwnerDocument;
            if (d != null)
            {
                this.nsmanager = NodeCache.GetNSManager(d.NameTable);
            }
        }

        protected void ClearCache()
        {
            this.cachedProperties = new Dictionary<string, object>();
        }

        protected T GetValue<T>(string name)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                T value = this.node.SelectSingleNode(name, this.nsmanager).ReadInnerText<T>();
                this.cachedProperties.Add(name, value);
            }

            return (T)(object)this.cachedProperties[name];
        }

        protected IReadOnlyList<T> GetReadOnlyValueList<T>(string name)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                List<T> list = new List<T>();

                foreach (XmlNode n1 in this.node.SelectNodes(name, this.nsmanager))
                {
                    T v1 = n1.ReadInnerText<T>();

                    if (v1 != null)
                    {
                        list.Add(v1);
                    }
                }

                this.cachedProperties.Add(name, list.AsReadOnly());
            }

            return (IReadOnlyList<T>)(object)this.cachedProperties[name];
        }

        protected IReadOnlyList<T> GetReadOnlyObjectList<T>(string name, params object[] parameters)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                List<T> list = new List<T>();

                foreach (XmlNode n1 in this.node.SelectNodes(name, this.nsmanager))
                {
                    List<object> args = new List<object>();

                    args.Add(n1);
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

                    if (v1 != null)
                    {
                        list.Add(v1);
                    }
                }

                this.cachedProperties.Add(name, list.AsReadOnly());
            }

            return (IReadOnlyList<T>)(object)this.cachedProperties[name];
        }

        protected IReadOnlyDictionary<TKey, TValue> GetReadOnlyObjectDictionary<TKey, TValue>(string name, Func<TValue, TKey> keySelector, IEqualityComparer<TKey> keyComparer, params object[] parameters)
        {
            if (!this.cachedProperties.ContainsKey(name))
            {
                Dictionary<TKey, TValue> list = new Dictionary<TKey, TValue>(keyComparer);

                foreach (XmlNode n1 in this.node.SelectNodes(name, this.nsmanager))
                {
                    List<object> args = new List<object>();

                    args.Add(n1);
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

                    if (v1 != null)
                    {
                        list.Add(keySelector.Invoke(v1), v1);
                    }
                }

                this.cachedProperties.Add(name, new ReadOnlyDictionary<TKey, TValue>(list));
            }

            return (IReadOnlyDictionary<TKey, TValue>)(object)this.cachedProperties[name];
        }

        protected T GetObject<T>(string nodeName, bool passNodeListToConstructor) where T : class
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
                        new object[] { this.node.SelectNodes(nodeName, nsmanager) },
                        null));
            }

            return (T)this.cachedProperties[nodeName];
        }

        protected T GetObject<T>(string nodeName, params object[] parameters) where T : class
        {
            if (!this.cachedProperties.ContainsKey(nodeName))
            {
                List<object> args = new List<object>();

                XmlNode n1 = this.node.SelectSingleNode(nodeName, this.nsmanager);
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.cachedProperties = null;
                    this.node = null;
                    this.nsmanager = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public string GetOuterXml()
        {
            return this.node.OuterXml;
        }

        internal void DropOuterXml(string file)
        {
            System.IO.File.WriteAllText(file, this.GetOuterXml());
        }

        internal XmlNode GetNode()
        {
            return this.node;
        }

        internal static XmlNamespaceManager GetNSManager(XmlNameTable n)
        {
            XmlNamespaceManager nsmanager = new XmlNamespaceManager(n);
            nsmanager.AddNamespace(dsmlPrefix, dsmlUri);
            nsmanager.AddNamespace(msdsmlPrefix, msdsmlUri);

            return nsmanager;
        }

    }
}
