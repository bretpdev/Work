using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ionic.Zip;

namespace CommercialArchiveImport
{
    public partial class MainProcessor
    {
        public class ThreadedDocumentProcessor : IDisposable
        {
            public static List<ThreadedDocumentProcessor> CalculateThreads(MainProcessor parent, List<Document> allDocs, int numThreads)
            {
                List<ThreadedDocumentProcessor> threads = new List<ThreadedDocumentProcessor>();
                var numDocs = allDocs.Count / numThreads;
                int nextStart = 0;
                for (int i = 0; i < numThreads && nextStart < allDocs.Count; i++)
                {
                    int start = nextStart;
                    int end = nextStart + numDocs;
                    if (end >= allDocs.Count)
                        end = allDocs.Count - 1;
                    else if (TifQueue.IsTif(allDocs[end].ImagePath))
                        while (end < allDocs.Count - 1 && TifQueue.IsTif(allDocs[end + 1].ImagePath))
                            end++;
                    threads.Add(new ThreadedDocumentProcessor(allDocs.Skip(start).Take(end - start + 1).ToList(), parent));
                    nextStart = end + 1;
                }
                return threads;
            }
            private MainProcessor Parent { get; set; }
            private List<Document> docs;
            public ThreadedDocumentProcessor(List<Document> docs, MainProcessor parent)
            {
                this.docs = docs;
                this.Parent = parent;
            }
            Thread t;
            public void Start()
            {
                Running = true; //this line MUST be synchronous or the main processor may bypass all threads on very small files
                t = new Thread(() =>
                {
                    using (ZipFile zip = new ZipFile(Parent.ZipLocation))
                        for (int lineNumber = 0; lineNumber < docs.Count; lineNumber++)
                        {
                            Document doc = docs[lineNumber];
                            lock (Parent.Zip)
                                Parent.ValidateDocument(doc);
                            while (!doc.Invalid && lineNumber < docs.Count && TifQueue.IsTif(doc.ImagePath)) //working with a multi-page tif split across one file per page
                            {
                                TifQueue tf = new TifQueue(docs, lineNumber, Parent);
                                lineNumber += tf.Tifs.Count;
                                if (lineNumber < docs.Count)
                                    doc = docs[lineNumber];
                            }
                            if (doc.Invalid)
                            {
                                ProgressHelper.Increment();
                                continue;
                            }
                            if (doc == null || lineNumber >= docs.Count)
                                continue;
                            Parent.ProcessLine(doc, zip);
                            ProgressHelper.Increment();
                        }

                    Running = false;
                });
                t.Start();
            }

            public bool Running { get; set; }

            #region IDisposable Members

            public void Dispose()
            {
                t.Abort();
            }

            #endregion
        }
    }
}
