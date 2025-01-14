using CFDocumentIndexer.Microsoft.Interfaces;

namespace CFDocumentIndexer.Microsoft.Models
{
    /// <summary>
    /// Vision config
    /// </summary>
    public class VisionConfig : IVisionConfig
    {
        public string Endpoint { get; set; } = String.Empty;

        public string APIKey { get; set; } = String.Empty;
    }
}
