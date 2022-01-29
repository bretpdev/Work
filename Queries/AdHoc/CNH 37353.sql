SELECT 
	DT.DocumentType,
	D.Document,
	A.Arc,
	ISNULL(ACXX.WF_QUE, '') AS QueueId
FROM 
	CLS.docid.Documents D 
	INNER JOIN CLS.docid.DocIdMapping DIM 
		ON DIM.DocumentId = D.DocumentsId
	INNER JOIN CLS.docid.Arcs A
		ON A.ArcId = DIM.ArcId
	INNER JOIN CLS.docid.DocumentTypes DT
		ON DT.DocumentTypesId = DIM.DocumentTypeId
	LEFT JOIN CDW..ACXX_ACT_REQ ACXX
		ON ACXX.PF_REQ_ACT = A.Arc
ORDER BY
	DT.DocumentType,
	D.Document,
	A.Arc,
	ACXX.WF_QUE


