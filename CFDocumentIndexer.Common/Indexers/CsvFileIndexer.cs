using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Indexers
{
    /// <summary>
    /// Indexes CSV files. Allow search of only the content
    /// </summary>
    public class CsvFileIndexer : IDocumentIndexer
    {
        public int Priority => 10000;   // Only use if no higher priority for document

        public Task<IndexedDocument> CreateIndexAsync(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>(),
                Tags = new List<string>()
            };

            // Process file
            // TODO: Make this more efficient
            using (var reader = new StreamReader(documentFile))
            {
                const char delimiter = ',';
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!String.IsNullOrEmpty(line))
                    {
                        var items = GetLineItems(line, delimiter);
                        foreach (var item in items)
                        {
                            if (!indexedDocument.Items.Contains(item)) indexedDocument.Items.Add(item);
                        }
                    }
                }
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
            return documentFile.ToLower().EndsWith(".csv");
        }

        private static List<string> GetLineItems(string line, Char delimiter)
        {
            var items = new List<string>();
            var elements = line.Split(delimiter);
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
