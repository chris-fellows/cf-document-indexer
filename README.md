# cf-document-indexer

Indexes documents to enable fast searching. Documents are indexed using SQLite. The mechanism can search
the document content or tags that are stored in an optional (.tags) file.

Document Indexers
-----------------
A document indexer can be defined for individual document types (E.g. .docx) but it one is not defined then
the default document indexer (E.g. Treat as text file) will be used.

Tags
----
If a file [document].tags exists then it should contain a tab delimited list of tags that can be searched.