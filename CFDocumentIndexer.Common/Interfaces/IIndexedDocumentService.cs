using CFDocumentIndexer.Common.Models;

namespace CFDocumentIndexer.Common.Interfaces
{
    /// <summary>
    /// Interface for indexed documents
    /// </summary>
    public interface IIndexedDocumentService
    {
        /// <summary>
        /// Adds indexed document
        /// </summary>
        /// <param name="indexedDocument"></param>
        void Add(IndexedDocument indexedDocument);

        /// <summary>
        /// Deletes document files for group
        /// </summary>
        /// <param name="documentFiles"></param>
        /// <param name="group"></param>
        void Delete(List<string> documentFiles, string group);

        /// <summary>
        /// Gets all indexed documents for group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        IEnumerable<IndexedDocument> GetAll(string group);

        /// <summary>
        /// Returns indexed documents matching the filter criteria.
        /// </summary>
        /// <param name="group">Document group</param>
        /// <param name="maxDocuments">Max documents</param>
        /// <param name="textToFind">Text to find</param>
        /// <param name="returnItems">Whether to return index items (For memory optimisation)</param>
        /// <returns></returns>
        IEnumerable<IndexedDocument> GetByFilter(string group, long? maxDocuments, string textToFind, bool returnItems);
    }
}
