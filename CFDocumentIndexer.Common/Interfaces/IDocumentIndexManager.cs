using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Interfaces
{
    /// <summary>
    /// Interface for document index manager
    /// </summary>
    public interface IDocumentIndexManager
    {
        /// <summary>
        /// Creates indexes for files
        /// </summary>
        /// <param name="documentFiles">Files to index</param>
        /// <param name="documentIndexConfig">Document group</param>
        /// <returns></returns>
        Task IndexDocumentsAsync(List<string> documentFiles, DocumentIndexConfig documentIndexConfig);
    }
}
