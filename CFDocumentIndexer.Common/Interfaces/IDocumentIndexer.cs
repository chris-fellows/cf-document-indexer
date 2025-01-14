using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Interfaces
{
    /// <summary>
    /// Interface for indexing a document
    /// </summary>
    public interface IDocumentIndexer
    {
        /// <summary>
        /// Priority for multiple IDocumentIndexer instances that can process a document. Lower value is higher priority.
        /// It enables a lower priority instance to be used (E.g. Text indexer) if the document type doesn't have a 
        /// more specific (higer priority) indexer.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Creates index asynchronously
        /// </summary>
        /// <param name="documentFile"></param>
        /// <returns></returns>
        Task<IndexedDocument> CreateIndexAsync(string documentFile);

        /// <summary>
        /// Whether instance can index this document
        /// </summary>
        /// <param name="documentFile"></param>
        /// <returns></returns>
        bool CanIndex(string documentFile);
    }
}
