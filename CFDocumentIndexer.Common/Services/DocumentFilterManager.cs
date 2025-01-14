using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Services
{
    public class DocumentFilterManager : IDocumentFilterManager
    {
        private readonly IIndexedDocumentService _indexedDocumentService;

        public DocumentFilterManager(IIndexedDocumentService indexedDocumentService)
        {
            _indexedDocumentService = indexedDocumentService;
        }

        public Task<IEnumerable<DocumentFilterItem>> GetFilteredAsync(DocumentFilterOptions filterOptions)   //, Action<IEnumerable<DocumentFilterItem>> streamResultsAction)
        {
            return Task.Factory.StartNew(() =>
            {
                IEnumerable<IndexedDocument> result = null;
                if (String.IsNullOrEmpty(filterOptions.TextToFind))
                {
                    result = _indexedDocumentService.GetAll(filterOptions.Group);                                        
                }
                else
                {
                    result = _indexedDocumentService.GetByFilter(filterOptions.Group, filterOptions.MaxDocuments, filterOptions.TextToFind, false);
                }

                return result.Select(r => new DocumentFilterItem()
                {
                    DocumentFile = r.DocumentFile
                    //ItemsFound = null
                });
            });
        }

        ///// <summary>
        ///// Filters document
        ///// </summary>
        ///// <param name="filterOptions"></param>
        ///// <param name="indexedDocument"></param>
        ///// <param name="semaphore"></param>
        ///// <returns></returns>
        //private Task<DocumentFilterItem> FilterDocumentAsync(DocumentFilterOptions filterOptions, IndexedDocument indexedDocument, SemaphoreSlim semaphore)
        //{
        //    var task = Task.Factory.StartNew(() =>
        //    {
        //        DocumentFilterItem documentFilterItem = null;
        //        try
        //        {
        //            // TODO: Search index
        //        }
        //        finally
        //        {
        //            semaphore.Release();
        //        }
        //        return documentFilterItem;
        //    });
        //    return task;
        //}

        //private void HandleSearchResults(List<Task<DocumentFilterItem>> tasks, Action<IEnumerable<DocumentFilterItem>> streamResultsAction)
        //{
        //    var completedTasks = tasks.Where(t => t.IsCompleted).ToList();

        //    while (completedTasks.Any())
        //    {
        //        var task = completedTasks[0];

        //        // Check result
        //        DocumentFilterItem documentFilterItem = task.Result;
        //        if (documentFilterItem != null &&
        //            documentFilterItem.ItemsFound != null &&
        //            documentFilterItem.ItemsFound.Any())
        //        {
        //            streamResultsAction(new List<DocumentFilterItem> { documentFilterItem });
        //        }

        //        tasks.Remove(task);
        //        completedTasks.Remove(task);
        //    }
        //}
    }
}
