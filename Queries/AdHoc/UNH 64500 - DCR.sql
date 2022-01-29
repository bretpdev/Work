USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 4

INSERT INTO ULS.docid.DocumentTypes(DocumentType)
VALUES('Deferment - Cancer Treatment')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

INSERT INTO ULS.docid.Documents(Document)
VALUES('LSCDF')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO ULS.docid.Arcs(Arc)
VALUES('LSCDF')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

DECLARE @DocumentId INT = (SELECT DocumentsId FROM ULS.docid.Documents WHERE Document = 'LSCDF')
DECLARE @DocumentTypeId INT = (SELECT DocumentTypesId FROM ULS.docid.DocumentTypes WHERE DocumentType = 'Deferment - Cancer Treatment')
DECLARE @ArcId INT = (SELECT ArcId FROM ULS.docid.Arcs WHERE Arc = 'LSCDF')

INSERT INTO ULS.docid.DocIdMapping(DocumentId, DocumentTypeId, ArcId, CreateQueue, AddTd22, BU, PO)
VALUES(@DocumentId, @DocumentTypeId, @ArcId, 1, 1, 1, 1)
	
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END