using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Indexers
{
    /// <summary>
    /// Indexes XML files. Allow search of only the content
    /// </summary>
    public class XmlFileIndexer : IDocumentIndexer
    {
        public int Priority => Int32.MaxValue;

        public Task<IndexedDocument> CreateIndexAsync(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>()
            };

            return Task.FromResult(indexedDocument);
        }

        public bool CanIndex(string documentFile)
        {
            return documentFile.ToLower().EndsWith(".xml");
        }
    }
}
