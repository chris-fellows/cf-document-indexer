using CFDocumentIndexer.Common.Models;

namespace CFDocumentIndexer.Common.Interfaces
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
        /// <param name="group">Document group</param>
        /// <returns></returns>
        Task CreateIndexesAsync(List<string> documentFiles, string group);
    }
}
