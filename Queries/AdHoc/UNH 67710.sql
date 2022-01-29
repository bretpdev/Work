USE ULS
GO

DECLARE @Document VARCHAR(5) = 'LSDFD'
DECLARE @DocumentId INT
DECLARE @DocumentType VARCHAR(100) = 'UHEAA Commercial Library'
DECLARE @DocumentTypeId INT

IF NOT EXISTS (SELECT * FROM docid.Documents WHERE Document = @Document)
	BEGIN
		INSERT INTO docid.Documents(Document)
		VALUES(@Document)
	END
SET @DocumentId = (SELECT DocumentsId FROM docid.Documents WHERE Document = @Document)

IF NOT EXISTS (SELECT * FROM docid.DocumentTypes WHERE DocumentType = @DocumentType)
	BEGIN
		INSERT INTO docid.DocumentTypes(DocumentType)
		VALUES(@DocumentType)
	END
SET @DocumentTypeId = (SELECT DocumentTypesId FROM docid.DocumentTypes WHERE DocumentType = @DocumentType)

INSERT INTO docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId, CreateQueue, AddTd22, BU, PO)
VALUES(@DocumentId, @DocumentTypeId, NULL, 0, 0, 1, 1)