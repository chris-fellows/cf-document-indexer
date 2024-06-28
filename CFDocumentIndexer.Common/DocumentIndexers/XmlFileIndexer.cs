using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Models;

namespace CFDocumentIndexer.Common.DocumentIndexers
{
    /// <summary>
    /// Indexes XML files. Allow search of only the content
    /// </summary>
    public class XmlFileIndexer : IDocumentIndexer
    {
        public IndexedDocument CreateIndex(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>()
            };
            return indexedDocument;
        }

        public bool CanIndex(string documentFile)
        {
            return false;   // documentFile.ToLower().EndsWith(".xml");
        }
    }
}
