using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;

namespace CFDocumentIndexer.Indexers.Images
{
    /// <summary>
    /// Indexes .jpg/jpeg files. If a .tags file exists then IndexedDocument.Tags will be set.
    /// </summary>
    public class JpgFileIndexer : IDocumentIndexer
    {
        public int Priority => 2;   // Use if no AI indexer

        public Task<IndexedDocument> CreateIndexAsync(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>(),
                Tags = new List<string>()
            };

            //// Process file
            //// TODO: Make this more efficient
            //using (var reader = new StreamReader(documentFile))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        var line = reader.ReadLine();
            //        if (!String.IsNullOrEmpty(line))
            //        {
            //            var items = GetLineItems(line);
            //            foreach (var item in items)
            //            {
            //                if (!indexedDocument.Items.Contains(item)) indexedDocument.Items.Add(item);
            //            }
            //        }
            //    }
            //}

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
            return documentFile.ToLower().EndsWith(".jpg") ||
                    documentFile.ToLower().EndsWith(".jpeg");
        }
    }
}
