namespace CFDocumentIndexer.Models
{
    /// <summary>
    /// Indexed document
    /// </summary>
    public class IndexedDocument
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Document group
        /// </summary>
        public string Group { get; set; } = String.Empty;

        /// <summary>
        /// Original document
        /// </summary>
        public string DocumentFile { get; set; } = String.Empty;

        /// <summary>
        /// Indexed items from document
        /// </summary>
        public List<string> Items { get; set; } = new List<string>();

        /// <summary>
        /// Tags
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();
    }
}
