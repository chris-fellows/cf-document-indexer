using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Indexers
{
    /// <summary>
    /// Indexes text files. If a .tags file exists then IndexedDocument.Tags will be set.
    /// </summary>
    public class TextFileIndexer : IDocumentIndexer
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
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!String.IsNullOrEmpty(line))
                    {
                        var items = GetLineItems(line);
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
            return true;   // Don't check extensions
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
