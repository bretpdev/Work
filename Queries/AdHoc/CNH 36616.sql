USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DECLARE @DocId int
	DECLARE @DocTypeId int = (SELECT DocumentTypesId FROM docid.DocumentTypes WHERE DocumentType = 'Borrower Corr')
	DECLARE @ArcId int

	INSERT INTO docid.Documents(Document)
	VALUES('CCRVP')
	SET @DocId = @@IDENTITY

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO docid.Arcs(Arc)
	VALUES('CCRVP')
	SET @ArcId = @@IDENTITY

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId)
	VALUES(@DocId, @DocTypeId, @ArcId)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END