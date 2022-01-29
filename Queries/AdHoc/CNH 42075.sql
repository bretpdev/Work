USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @INSERTCOUNT INT = X
	
	-----ARC-------
	DECLARE @ArcId INT
	DECLARE @Arc VARCHAR(X) = 'LSXXX'
	IF NOT EXISTS (SELECT ArcId FROM CLS.docid.Arcs WHERE Arc = @Arc)
		BEGIN
			INSERT INTO CLS.docid.Arcs(Arc)
			VALUES(@Arc)
			SELECT @ROWCOUNT = @@ROWCOUNT, @INSERTCOUNT = @INSERTCOUNT + @@ROWCOUNT, @ERROR = @@ERROR
		END
	SET @ArcId = (SELECT ArcId FROM CLS.docid.Arcs WHERE Arc = @Arc)

	-----Document-------
	DECLARE @DocId INT
	DECLARE @Document VARCHAR(X) = 'CRDOD'
	IF NOT EXISTS (SELECT DocumentsId FROM CLS.docid.Documents WHERE Document = @Document)
		BEGIN
			INSERT INTO CLS.docid.Documents(Document)
			VALUES(@Document)
			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @INSERTCOUNT = @INSERTCOUNT + @@ROWCOUNT, @ERROR = @@ERROR
		END
	SET @DocId = (SELECT DocumentsId FROM CLS.docid.Documents WHERE Document = @Document)

	-----DocumentType-------
	DECLARE @DocTypeId INT
	DECLARE @DocumentType VARCHAR(XXX) = 'Loan Repayment Programs'
	IF NOT EXISTS (SELECT DocumentTypesId FROM CLS.docid.DocumentTypes WHERE DocumentType = @DocumentType)
		BEGIN
			INSERT INTO CLS.docid.DocumentTypes(DocumentType)
			VALUES(@DocumentType)
			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @INSERTCOUNT = @INSERTCOUNT + @@ROWCOUNT, @ERROR = @@ERROR
		END
	SET @DocTypeId = (SELECT DocumentTypesId FROM CLS.docid.DocumentTypes WHERE DocumentType = @DocumentType)

	-----Mapping Table-------
	INSERT INTO CLS.docid.DocIdMapping(DocumentTypeId, DocumentId, ArcId)
	VALUES(@DocTypeId, @DocId, @ArcId)
	
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @INSERTCOUNT = @INSERTCOUNT + @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @INSERTCOUNT AND @ERROR = X
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