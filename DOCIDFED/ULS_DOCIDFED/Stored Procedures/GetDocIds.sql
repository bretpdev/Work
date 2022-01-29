CREATE PROCEDURE [docid].[GetDocIds]
AS
	SELECT
		MAP.DocIdMappingId [MappingId],
		D.Document [DocId],
		REPLACE(DT.DocumentType, '/', '-') [DocType],
		A.Arc,
		MAP.CreateQueue,
		MAP.AddTd22,
		MAP.BU,
		MAP.PO
	FROM
		docid.DocIdMapping MAP
		LEFT JOIN docid.Documents D
			ON MAP.DocumentId = D.DocumentsId
		LEFT JOIN docid.DocumentTypes DT
			ON MAP.DocumentTypeId = DT.DocumentTypesId
		LEFT JOIN docid.Arcs A
			ON  MAP.ArcId = A.ArcId

RETURN 0