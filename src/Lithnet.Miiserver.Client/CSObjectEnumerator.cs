using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Microsoft.DirectoryServices.MetadirectoryServices.UI.WebServices;

namespace Lithnet.Miiserver.Client
{
    public class CSObjectEnumerator : IEnumerator<CSObjectBase>, IEnumerable<CSObjectBase>, IDisposable
    {
        private string token;

        private MMSWebService webService;

        private int currentIndex = -1;

        private CSObjectSearchResultBatch currentResultSet;

        private bool disposed = false;

        private bool exporting;

        private CSObjectParts csParts;

        private uint entryParts;

        private uint pageSize;

        internal CSObjectEnumerator(MMSWebService ws, string token, bool exporting, int pageSize, CSObjectParts csParts, uint entryParts)
        {
            this.token = token;
            this.webService = ws;
            this.exporting = exporting;
            this.csParts = csParts;
            this.entryParts = entryParts;
            this.pageSize = (uint)pageSize;

            if (token == null)
            {
                this.currentResultSet = new CSObjectSearchResultBatch(new XmlDocument());
            }
            else
            {
                this.GetNextPage();
            }
        }

        internal int BatchCount => this.currentResultSet.Count;

        public IEnumerator<CSObjectBase> GetEnumerator()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            return this;
        }

        public CSObjectBase Current
        {
            get
            {
                if (this.disposed)
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
                if (this.disposed)
                {
                    throw new ObjectDisposedException("SearchEnumerator");
                }

                return this.Current;
            }
        }

        public bool MoveNext()
        {
            if (this.disposed)
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
            if (this.disposed)
            {
                throw new ObjectDisposedException("SearchEnumerator");
            }

            this.currentIndex = -1;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
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

            this.currentResultSet?.Dispose();

            this.disposed = true;
        }

        private void GetNextPage()
        {
            string response;

            if (this.exporting)
            {
                response = this.webService.ExportConnectorSpaceGetNext(this.token, (ulong)this.csParts, this.entryParts, this.pageSize);
                SyncServer.ThrowExceptionOnReturnError(response);
            }
            else
            {
                response = this.webService.GetCSResults(this.token, this.pageSize, (ulong)this.csParts, this.entryParts, 0, null);
                SyncServer.ThrowExceptionOnReturnError(response);
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
