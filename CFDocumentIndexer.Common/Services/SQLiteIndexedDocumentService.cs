using CFDocumentIndexer.Common.Interfaces;
using CFDocumentIndexer.Common.Models;
using System.Data.SQLite;

namespace CFDocumentIndexer.Common.Services
{
    /// <summary>
    /// Indexed document service using SQLite.
    /// </summary>
    public class SQLiteIndexedDocumentService : IIndexedDocumentService, IDisposable
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public SQLiteIndexedDocumentService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        /// <summary>
        /// Initialise database to store indexed documents
        /// </summary>
        /// <param name="connection"></param>
        private void InitialiseDatabase(SQLiteConnection connection)
        {            
            var sql = "CREATE TABLE IF NOT EXISTS IndexedDocuments (" +
                "ID TEXT PRIMARY KEY NOT NULL, " +
                "DocumentGroup TEXT NOT NULL, " +   // TODO: Consider Groups table
                "DocumentFile TEXT NOT NULL, " +
                "Data TEXT" +
                ")";
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        private SQLiteConnection GetConnection()
        {
            if (_connection == null)   // Open connection
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();

                InitialiseDatabase(_connection);
            }
            return _connection;
        }

        public void Add(IndexedDocument indexedDocument)
        {
            var connection = GetConnection();

            var sql = "DELETE FROM IndexedDocuments WHERE DocumentGroup = @p1 AND DocumentFile = @p2 ";
            var command1 = new SQLiteCommand(sql, connection);            
            command1.Parameters.Add((new SQLiteParameter("@p1", indexedDocument.Group)));
            command1.Parameters.Add((new SQLiteParameter("@p2", indexedDocument.DocumentFile)));            
            command1.ExecuteNonQuery();

            sql = "INSERT INTO IndexedDocuments (ID, DocumentGroup, DocumentFile, Data) VALUES(@p1, @p2, @p3, @p4)";
            var command = new SQLiteCommand(sql, connection);
            command.Parameters.Add((new SQLiteParameter("@p1", Guid.NewGuid().ToString())));
            command.Parameters.Add((new SQLiteParameter("@p2", indexedDocument.Group)));
            command.Parameters.Add((new SQLiteParameter("@p3", indexedDocument.DocumentFile)));
            command.Parameters.Add((new SQLiteParameter("@p4", System.String.Join("\t", indexedDocument.Items))));
            command.ExecuteNonQuery();
        }

        public void Delete(List<string> documentFiles, string group)
        {
            if (String.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(group);
            }

            var connection = GetConnection();            

            if (documentFiles != null && documentFiles.Any())   // Delete specific documents for group
            {
                foreach (var documentFile in documentFiles)
                {
                    var sql = "DELETE FROM IndexedDocuments WHERE DocumentGroup = @p1 AND DocumentFile = @p2";
                    var command = new SQLiteCommand(sql, connection);
                    command.Parameters.Add((new SQLiteParameter("@p1", group)));
                    command.Parameters.Add((new SQLiteParameter("@p2", documentFile)));
                    command.ExecuteNonQuery();
                }
            }
            else    // Delete all documents for group
            {
                var sql = "DELETE FROM IndexedDocuments WHERE DocumentGroup = @p1";
                var command = new SQLiteCommand(sql, connection);
                command.Parameters.Add((new SQLiteParameter("@p1", group)));                
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<IndexedDocument> GetAll(string group)
        {
            if (String.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(group);
            }

            var connection = GetConnection();
            var documents = new List<IndexedDocument>();

            var sql = "SELECT ID, DocumentGroup, DocumentFile, Data FROM IndexedDocuments WHERE DocumentGroup = @p1 ORDER BY DocumentFile";

            var command = new SQLiteCommand(sql, connection);
            command.Parameters.Add((new SQLiteParameter("@p1", group)));

            using (var reader = command.ExecuteReader(System.Data.CommandBehavior.Default))
            {
                do
                {
                    while (reader.Read())
                    {
                        var indexedDocument = new IndexedDocument()
                        {
                            Id = reader.GetString(reader.GetOrdinal("ID")),
                            Group = reader.GetString(reader.GetOrdinal("DocumentGroup")),
                            DocumentFile = reader.GetString(reader.GetOrdinal("DocumentFile")),
                            Items = reader.GetString(reader.GetOrdinal("Data")).Split('\t').Select(i => i).ToList()
                        };
                        documents.Add(indexedDocument);
                    }
                } while (reader.NextResult());
            }

            return documents;
        }

        public IEnumerable<IndexedDocument> GetByFilter(string group, long? maxDocuments, string textToFind, bool returnItems)
        {
            if (String.IsNullOrEmpty(group))
            {
                throw new ArgumentNullException(group);
            }

            var connection = GetConnection();
            var documents = new List<IndexedDocument>();

            var sql = returnItems ?
                        "SELECT ID, DocumentGroup, DocumentFile, Data FROM IndexedDocuments WHERE DocumentGroup = @p1 AND Data LIKE @p2 ORDER BY DocumentFile " :
                        "SELECT ID, DocumentGroup, DocumentFile FROM IndexedDocuments WHERE DocumentGroup = @p1 AND Data LIKE @p2 ORDER BY DocumentFile ";
            if (maxDocuments != null) sql += $"LIMIT {maxDocuments.Value}";

            var command = new SQLiteCommand(sql, connection);
            command.Parameters.Add((new SQLiteParameter("@p1", group)));
            command.Parameters.Add((new SQLiteParameter("@p2", $"%{textToFind}%")));
            
            using (var reader = command.ExecuteReader(System.Data.CommandBehavior.Default))
            {
                do
                {
                    while (reader.Read())
                    {
                        var indexedDocument = new IndexedDocument()
                        {
                            Id = reader.GetString(reader.GetOrdinal("ID")),
                            Group = reader.GetString(reader.GetOrdinal("DocumentGroup")),
                            DocumentFile = reader.GetString(reader.GetOrdinal("DocumentFile")),
                            Items = returnItems ?
                                    reader.GetString(reader.GetOrdinal("Data")).Split('\t').Select(i => i).ToList() :
                                    new List<string>()
                        };
                        documents.Add(indexedDocument);
                    }
                } while (reader.NextResult());
            }

            return documents;
        }       
    }
}
