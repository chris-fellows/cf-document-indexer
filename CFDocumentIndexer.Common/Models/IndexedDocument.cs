namespace CFDocumentIndexer.Common.Models
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
        /// Indexed items
        /// </summary>
        public List<string> Items { get; set; } = new List<string>();
    }
}
