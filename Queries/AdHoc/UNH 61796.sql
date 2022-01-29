USE ULS
GO

IF NOT EXISTS (SELECT * FROM ULS.docid.Documents WHERE Document = 'PCDDB')
	BEGIN
		INSERT INTO ULS.docid.Documents(Document)
		VALUES('PCDDB')
	END

IF NOT EXISTS (SELECT * FROM ULS.docid.DocumentTypes WHERE DocumentType = 'Death Certificate')
	BEGIN
		INSERT INTO uls.docid.DocumentTypes(DocumentType)
		VALUES('Death Certificate')
	END

DECLARE @DocumentId INT = (SELECT DocumentsId FROM ULS.docid.Documents WHERE Document = 'PCDDB')
DECLARE @DocumentTypeId INT = (SELECT DocumentTypesId FROM ULS.docid.DocumentTypes WHERE DocumentType = 'Death Certificate')
IF NOT EXISTS (SELECT * FROM ULS.docid.DocIdMapping WHERE DocumentId = @DocumentId)
	BEGIN
		INSERT INTO ULS.docid.DocIdMapping(DocumentId, DocumentTypeId, CreateQueue, AddTd22, BU, PO)
		VALUES(@DocumentId,@DocumentTypeId,0,0,1,1)
	END