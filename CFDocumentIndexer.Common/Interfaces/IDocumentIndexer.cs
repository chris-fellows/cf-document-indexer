using CFDocumentIndexer.Common.Models;

namespace CFDocumentIndexer.Common.Interfaces
{
    /// <summary>
    /// Interface for indexing a document
    /// </summary>
    public interface IDocumentIndexer
    {
        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="documentFile"></param>
        /// <returns></returns>
        IndexedDocument CreateIndex(string documentFile);

        /// <summary>
        /// Whether instance can index this document
        /// </summary>
        /// <param name="documentFile"></param>
        /// <returns></returns>
        bool CanIndex(string documentFile);
    }
}
