using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CFDocumentIndexer.Indexers.MicrosoftOffice
{
    /// <summary>
    /// Indexes Word (Open XML) documents
    /// </summary>
    public class WordOpenXmlFileIndexer : IDocumentIndexer
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

            using (var document = WordprocessingDocument.Open(documentFile, false))
            {
                var innerText = document.MainDocumentPart.Document.Body.InnerText;
                //var innerXml = document.MainDocumentPart.Document.Body.InnerXml;

                indexedDocument.Items = GetLineItems(innerText).Distinct().ToList();

                /*
                foreach(var part in document.MainDocumentPart.Parts)
                {                    
                    int xxx = 1000;   
                }
                */
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
            return documentFile.ToLower().EndsWith(".docx");
        }

        private static List<string> GetLineItems(string line)
        {
            var items = new List<string>();
            var elements = line.Split(' ');
            foreach (var element in elements)
            {
                if (element.Trim().Length > 0 &&
                    !items.Contains(element))
                {
                    items.Add(element);
                }
            }
            return items;
        }
    }
}
