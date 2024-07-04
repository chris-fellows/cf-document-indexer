using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Models;

namespace CFDocumentIndexer.UI
{
    public partial class MainForm : Form
    {        
        private readonly IDocumentFilterManager _documentFilterManager;
        private readonly IDocumentIndexManager _documentIndexManager;

        public MainForm(IDocumentFilterManager documentFilterManager, IDocumentIndexManager documentIndexManager)
        {
            InitializeComponent();

            _documentFilterManager = documentFilterManager;
            _documentIndexManager = documentIndexManager;

            RunIndexTest();

            //RunFilterTest();
            //RunFilterAllTest();
            int xx = 1000;
        }
      
        private void RunIndexTest()
        {
            var group = "Group1";
            
            // Get documents to index
            var documentFiles = Directory.GetFiles("D:\\Test\\DocumentIndex\\Files").ToList();

            // Index documents
            var task = _documentIndexManager.CreateIndexesAsync(documentFiles, group);
            task.Wait();

            int xxx = 1000;
        }

        private void RunFilterTest()
        {            
            // Set filter options
            var filterOptions = new DocumentFilterOptions()
            {
                Group = "Group1",
                TextToFind = "FolderOptions",
                MaxDocuments = 2
            };

            var task = _documentFilterManager.GetFilteredAsync(filterOptions);

            
            /*
            , (results) =>
            {
                foreach (var result in results)
                {
                    System.Diagnostics.Debug.WriteLine(result.DocumentFile);
                }
            });
            */

            task.Wait();
            int xxx = 1000;
        }

        private void RunFilterAllTest()
        {
            //IndexedDocumentService indexedDocumentService = new SQLiteIndexedDocumentService(_connectionString);

            //IDocumentFilterManager documentFilterManager = new DocumentFilterManager(indexedDocumentService);

            // Set filter options
            var filterOptions = new DocumentFilterOptions()
            {
                Group = "Group1",
                TextToFind = ""
            };

            var task = _documentFilterManager.GetFilteredAsync(filterOptions);
            
            /*, (results) =>
            {
                foreach (var result in results)
                {
                    System.Diagnostics.Debug.WriteLine(result.DocumentFile);
                }
            });
            */

            task.Wait();
            int xxx = 1000;
        }

    }
}
