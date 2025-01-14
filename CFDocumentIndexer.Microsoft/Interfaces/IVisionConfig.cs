namespace CFDocumentIndexer.Microsoft.Interfaces
{
    /// <summary>
    /// Vision config
    /// </summary>
    public interface IVisionConfig
    {
        string Endpoint { get; }

        string APIKey { get; }
    }
}
