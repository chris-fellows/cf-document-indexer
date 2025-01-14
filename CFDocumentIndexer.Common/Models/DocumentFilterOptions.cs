namespace CFDocumentIndexer.Models
{
    /// <summary>
    /// Document filter options
    /// </summary>
    public class DocumentFilterOptions
    {
        /// <summary>
        /// Document group
        /// </summary>
        public string Group { get; set; } = String.Empty;

        /// <summary>
        /// Text to find
        /// </summary>
        public string TextToFind { get; set; } = String.Empty;       

        /// <summary>
        /// Max documents to return (null=Any)
        /// </summary>
        public int? MaxDocuments { get; set; } = null;
    }
}
