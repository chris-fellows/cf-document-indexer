using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;

namespace CFDocumentIndexer.Common.DocumentIndexers
{
    /// <summary>
    /// Indexes PowerPoint (Open XML) documents
    /// </summary>
    public class PowerPointOpenXmlFileIndexer : IDocumentIndexer
    {
        public int Priority => 1;

        public IndexedDocument CreateIndex(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>(),
                Tags = new List<string>()
            };

            using (var document = PresentationDocument.Open(documentFile, false))
            {
                
            }

            // Read tags if exists
            var tagFile = $"{documentFile}.tags";
            if (File.Exists(tagFile))
            {
                indexedDocument.Tags = File.ReadAllText(tagFile).Split('\t').Distinct().ToList();
            }

            return indexedDocument;
        }

        public bool CanIndex(string documentFile)
        {
            return documentFile.ToLower().EndsWith(".pptx");
        }
    }
}
