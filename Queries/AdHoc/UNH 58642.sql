USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Document char(5) = 'LSSSL'
	DECLARE @DocumentId INT
	DECLARE @DocumentType varchar(100) = 'School Skip Letter'
	DECLARE @DocumentTypeId INT
	DECLARE @Arc varchar(5) = 'DISSL'
	DECLARE @ArcId INT
	DECLARE @DocIdMappingId INT

	IF NOT EXISTS (SELECT * FROM docid.Documents WHERE Document = @Document)
	BEGIN
		INSERT INTO docid.Documents(Document)
		VALUES('LSSSL')
	END
	SELECT @DocumentId = (SELECT DocumentsId FROM docid.Documents WHERE Document = @Document)
	PRINT CONCAT('Document ID: ', @DocumentId)

	IF NOT EXISTS (SELECT * FROM docid.DocumentTypes WHERE DocumentType = @DocumentType)
	BEGIN
		INSERT INTO docid.DocumentTypes(DocumentType)
		VALUES('School Skip Letter')
	END
	SELECT @DocumentTypeId = (SELECT DocumentTypesId FROM docid.DocumentTypes WHERE DocumentType = @DocumentType)
	PRINT CONCAT('Document Type ID: ', @DocumentTypeId)

	IF NOT EXISTS (SELECT * FROM docid.Arcs WHERE Arc = @Arc)
	BEGIN
		INSERT INTO docid.Arcs(Arc)
		VALUES('DISSL')
	END
	SELECT @ArcId = (SELECT ArcId FROM docid.Arcs WHERE Arc = @Arc)
	PRINT CONCAT('Arc ID: ', @ArcId)


	IF NOT EXISTS (SELECT * FROM docid.DocIdMapping WHERE DocumentId = @DocumentId AND DocumentTypeId = @DocumentTypeId AND ArcId = @ArcId)
	BEGIN
		INSERT INTO docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId, CreateQueue, AddTd22, BU, PO)
		VALUES(@DocumentId, @DocumentTypeId, @ArcId, 0, 1, 1, 1)
	END
	SELECT @DocIdMappingId = (SELECT DocIdMappingId FROM docid.DocIdMapping WHERE DocumentId = @DocumentId AND DocumentTypeId = @DocumentTypeId AND ArcId = @ArcId)
	PRINT CONCAT('DocID Mapping ID: ', @DocIdMappingId)

COMMIT TRANSACTION
--ROLLBACK TRANSACTION