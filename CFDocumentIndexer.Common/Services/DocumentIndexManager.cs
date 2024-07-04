using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CFDocumentIndexer.Common.Services
{
    public class DocumentIndexManager : IDocumentIndexManager
    {
        private readonly IEnumerable<IDocumentIndexer> _documentIndexers;
        private readonly IIndexedDocumentService _indexedDocumentService;

        public DocumentIndexManager(IEnumerable<IDocumentIndexer> documentIndexers,
                                    IIndexedDocumentService indexedDocumentService)
        {
            _documentIndexers = documentIndexers;
            _indexedDocumentService = indexedDocumentService;
        }

        public Task CreateIndexesAsync(List<string> documentFiles, string group)
        {
            return Task.Factory.StartNew(() =>
            {
                // Index documents, limit max number
                var semaphore = new SemaphoreSlim(5, 5);
                var tasks = new List<Task<IndexedDocument>>();
                foreach (var documentFile in documentFiles)
                {
                    // Wait for free thread, handle indexed objects too
                    bool waited = false;
                    do
                    {
                        waited = semaphore.Wait(TimeSpan.FromMilliseconds(5000));

                        HandleIndexedDocuments(tasks);
                    } while (!waited);

                    // Start index
                    var task = CreateIndexAsync(documentFile, group, semaphore);
                    tasks.Add(task);
                }

                // Wait for tasks to completed
                while (tasks.Any())
                {
                    HandleIndexedDocuments(tasks);
                    if (tasks.Any())
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            });
        }
            
        private Task<IndexedDocument> CreateIndexAsync(string documentFile, string group, SemaphoreSlim semaphore)
        {
            //if (documentFile.ToLower().EndsWith(".docx"))
            //{                
            //    var objectIndexerTest = _documentIndexers.Where(di => di.CanIndex(documentFile))
            //                                     .OrderBy(di => di.Priority).FirstOrDefault();
            //    int xxx = 1000;
            //}

            var task = Task.Factory.StartNew(() =>
            {
                IndexedDocument indexedDocument = null;
                try
                {
                    // Get highest priority indexer. The generic text file indexer is lower priority so that it's only used if there's
                    // no higher priority indexer for the file type.
                    var objectIndexer = _documentIndexers.Where(di => di.CanIndex(documentFile))
                                                    .OrderBy(di => di.Priority).FirstOrDefault();
                    if (objectIndexer != null)                    
                    {
                        indexedDocument = objectIndexer.CreateIndex(documentFile);
                        indexedDocument.Group = group;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
                return indexedDocument;
            });

            return task;
        }

     
        /// <summary>
        /// Handles indexed documents
        /// </summary>
        /// <param name="tasks"></param>
        private void HandleIndexedDocuments(List<Task<IndexedDocument>> tasks)
        {
            // Get completed indexes
            var completedTasks = tasks.Where(t => t.IsCompleted).ToList();

            // Process completed indexes
            while (completedTasks.Any())
            {
                var task = completedTasks[0];

                _indexedDocumentService.Add(task.Result);

                tasks.Remove(task);
                completedTasks.Remove(task);
            }
        }
    }
}
