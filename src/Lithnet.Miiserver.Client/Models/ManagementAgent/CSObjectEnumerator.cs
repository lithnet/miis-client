using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class CSObjectEnumerator : IEnumerator<CSObject>, IEnumerable<CSObject>, IDisposable
    {
        private string token;

        private MMSWebService webService;

        private int currentIndex = -1;

        private CSObjectSearchResultBatch currentResultSet;

        private bool disposed = false;

        private bool exporting;

        private CSObjectParts csParts;

        private uint entryParts;

        internal CSObjectEnumerator(MMSWebService ws, string token, bool exporting, CSObjectParts csParts, uint entryParts)
        {
            this.token = token;
            this.webService = ws;
            this.exporting = exporting;
            this.csParts = csParts;
            this.entryParts = entryParts;

            if (token == null)
            {
                this.currentResultSet = new CSObjectSearchResultBatch(new XmlDocument());
            }
            else
            {
                this.GetNextPage();
            }
        }

        internal int BatchCount
        {
            get
            {
                return this.currentResultSet.Count;
            }
        }

        public IEnumerator<CSObject> GetEnumerator()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            return this;
        }

        public CSObject Current
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("SearchEnumerator");
                }

                return this.currentResultSet.CSObjects[this.currentIndex];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("SearchEnumerator");
                }

                return this.Current;
            }
        }

        public bool MoveNext()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            this.currentIndex++;

            if (this.currentIndex >= this.currentResultSet.Count)
            {
                this.GetNextPage();
                this.currentIndex = 0;

                if (this.currentResultSet.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Reset()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            this.currentIndex = -1;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (this.token != null)
                {
                    this.webService.ReleaseSessionObjects(new string[] { this.token });
                }
            }
            catch { }

            if (this.currentResultSet != null)
            {
                this.currentResultSet.Dispose();
            }

            disposed = true;
        }

        private void GetNextPage()
        {
            string response;

            if (exporting)
            {
                response = this.webService.ExportConnectorSpaceGetNext(this.token, (ulong)this.csParts, this.entryParts, 10);
            }
            else
            {
                response = this.webService.GetCSResults(this.token, 10, (ulong)this.csParts, this.entryParts, 0, null);
            }

            XmlDocument d = new XmlDocument();
            d.LoadXml(response);

            if (d.SelectSingleNode("error") != null)
            {
                this.currentResultSet = new CSObjectSearchResultBatch(d);
            }
            else
            {
                this.currentResultSet = new CSObjectSearchResultBatch(d.SelectSingleNode("batch"));
            }
        }
    }
}
