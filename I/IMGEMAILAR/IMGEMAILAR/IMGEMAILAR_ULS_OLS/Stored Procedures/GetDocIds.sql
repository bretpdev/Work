CREATE PROCEDURE [imgemailar].[GetDocIds]
AS
	
	SELECT
		D.Document [DocId],
		MAX(REPLACE(DT.DocumentType, '/', '-')) [DocType] --in case a document has multiple descriptions
	FROM
		docid.DocIdMapping MAP
		INNER JOIN docid.Documents D
			ON MAP.DocumentId = D.DocumentsId
		INNER JOIN docid.DocumentTypes DT
			ON MAP.DocumentTypeId = DT.DocumentTypesId
	GROUP BY D.Document 

RETURN 0
