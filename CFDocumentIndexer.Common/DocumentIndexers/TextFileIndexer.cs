using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Models;

namespace CFDocumentIndexer.Common.DocumentIndexers
{
    /// <summary>
    /// Indexes text files
    /// </summary>
    public class TextFileIndexer : IDocumentIndexer
    {
        public IndexedDocument CreateIndex(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>()
            };

            // Process file
            // TODO: Make this more efficient
            using (var reader = new StreamReader(documentFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var elements = line.Split(' ');
                    foreach (var element in elements)
                    {
                        if (element.Trim().Length > 0 &&
                            !indexedDocument.Items.Contains(element))
                        {
                            indexedDocument.Items.Add(element);
                        }
                    }
                }
            }

            return indexedDocument;
        }

        public bool CanIndex(string documentFile)
        {
            return true;   // Don't check extensions
        }
    }
}
