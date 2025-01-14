using CFDocumentIndexer.Interfaces;
using CFDocumentIndexer.Models;
using CFDocumentIndexer.Microsoft.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace CFDocumentIndexer.Microsoft.Indexers.Images
{
    /// <summary>
    /// Indexes image file using Vision API
    /// </summary>
    public class VisionImageFileIndexer : IDocumentIndexer
    {
        private readonly IVisionConfig _visionConfig;

        public int Priority => 1;   // Only use if no higher priority for document

        public VisionImageFileIndexer(IVisionConfig visionConfig)
        {
            _visionConfig = visionConfig;
        }

        public async Task<IndexedDocument> CreateIndexAsync(string documentFile)
        {
            var indexedDocument = new IndexedDocument()
            {
                DocumentFile = documentFile,
                Items = new List<string>(),
                Tags = new List<string>()
            };
            
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_visionConfig.APIKey))
            {
                Endpoint = _visionConfig.Endpoint
            };

            // Set features
            var features = new List<VisualFeatureTypes?>
            {
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Tags
            };

            using (var stream = File.OpenRead(documentFile))
            {
                // Analyse image
                var analysis = await client.AnalyzeImageInStreamAsync(stream, features);

                // Add analysed data
                indexedDocument.Items.AddRange(analysis.Description.Captions.Select(c => c.Text));
                indexedDocument.Items.AddRange(analysis.Description.Tags);
                indexedDocument.Items.AddRange(analysis.Categories.Select(c => c.Name));
                indexedDocument.Items.AddRange(analysis.Tags.Select(t => t.Name));          
            }

            indexedDocument.Items = indexedDocument.Items.Distinct().ToList();  // Sanity check            

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
            return true;
        }
    }
}
