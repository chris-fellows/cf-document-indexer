using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CFDocumentIndexer.Indexers.MicrosoftOffice
{
    /// <summary>
    /// Indexes Excel (Open XML) documents
    /// </summary>
    public class ExcelOpenXmlFileIndexer : IDocumentIndexer
    {
        public int Priority => 1;

        public Task<IndexedDocument> CreateIndexAsync(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>(),
                Tags = new List<string>()
            };

            using (var document = SpreadsheetDocument.Open(documentFile, false))
            {

            }

            // Read tags if exists
            var tagFile = $"{documentFile}.tags";
            if (File.Exists(tagFile))
            {
                indexedDocument.Tags = File.ReadAllText(tagFile).Split('\t').Distinct().ToList();
            }

            return Task.FromResult(indexedDocument);
        }

        public bool CanIndex(string documentFile)
        {
            return documentFile.ToLower().EndsWith(".xlsx");
        }
    }
}
