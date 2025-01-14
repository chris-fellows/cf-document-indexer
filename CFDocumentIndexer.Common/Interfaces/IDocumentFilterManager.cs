using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Interfaces
{
    /// <summary>
    /// Interface for filtering documents
    /// </summary>
    public interface IDocumentFilterManager
    {
        /// <summary>
        /// Gets documents matching filter
        /// </summary>
        /// <param name="filterOptions"></param>
        /// <param name="streamResultsAction"></param>
        /// <returns></returns>
        Task<IEnumerable<DocumentFilterItem>> GetFilteredAsync(DocumentFilterOptions filterOptions); //, Action<IEnumerable<DocumentFilterItem>> streamResultsAction);
    }
}
