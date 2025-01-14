using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Extensions.DependencyInjection;

namespace CFDocumentIndexer.Services
{
    public class DocumentIndexManager : IDocumentIndexManager
    {        
        private readonly IIndexedDocumentService _indexedDocumentService;
        private readonly IServiceProvider _serviceProvider;

        public DocumentIndexManager(IIndexedDocumentService indexedDocumentService,
                                    IServiceProvider serviceProvider)
        {     
            _indexedDocumentService = indexedDocumentService;
            _serviceProvider = serviceProvider;            
        }

        public Task IndexDocumentsAsync(List<string> documentFiles, DocumentIndexConfig documentIndexConfig)
        {
            return Task.Factory.StartNew(() =>
            {                
                // Index documents, limit max number
                var semaphore = new SemaphoreSlim(documentIndexConfig.MaxConcurrentTasks, documentIndexConfig.MaxConcurrentTasks);
                var tasks = new List<Task<IndexedDocument>>();
                foreach (var documentFile in documentFiles)
                {
                    // Wait for free thread, handle indexed objects too
                    bool waited = false;
                    do
                    {
                        waited = semaphore.Wait(TimeSpan.FromMilliseconds(100));

                        HandleIndexedDocuments(tasks);
                    } while (!waited);

                    // Start index
                    var task = IndexDocumentAsync(documentFile, documentIndexConfig.Group, semaphore);
                    tasks.Add(task);
                }

                // Wait for tasks to completed
                while (tasks.Any())
                {
                    HandleIndexedDocuments(tasks);                    
                    System.Threading.Thread.Sleep(100);                    
                }
            });
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentFile"></param>
        /// <param name="group"></param>
        /// <param name="semaphore"></param>
        /// <returns></returns>
        private Task<IndexedDocument?> IndexDocumentAsync(string documentFile, string group, SemaphoreSlim semaphore)
        {           
            var task = Task.Factory.StartNew(() =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IndexedDocument? indexedDocument = null;
                    try
                    {
                        // Get document indexers
                        var documentIndexers = scope.ServiceProvider.GetServices<IDocumentIndexer>();

                        // Get highest priority indexer. The generic text file indexer is lower priority so that it's only used if there's
                        // no higher priority indexer for the file type.
                        var documentIndexer = documentIndexers.Where(di => di.CanIndex(documentFile))
                                                        .OrderBy(di => di.Priority).FirstOrDefault();
                        if (documentIndexer != null)
                        {
                            indexedDocument = documentIndexer.CreateIndexAsync(documentFile).Result;
                            indexedDocument.Group = group;
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                    return indexedDocument;
                }
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
