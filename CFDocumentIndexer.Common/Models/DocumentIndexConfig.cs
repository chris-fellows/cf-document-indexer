using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFDocumentIndexer.Models
{
    public class DocumentIndexConfig
    {
        public string Group { get; set; } = String.Empty;

        public int MaxConcurrentTasks { get; set; } = 5;
    }
}
