USE ULS
GO
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 8

--1
INSERT INTO ULS..ArcAddProcessing(ArcTypeId, ArcResponseCodeId, AccountNumber, RecipientId, ARC, ActivityType, ActivityContact, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeededBy, RegardsTo, RegardsCode, LN_ATY_SEQ, ProcessingAttempts, CreatedAt, CreatedBy)
VALUES(0,NULL,'1964209292','','BILLS',NULL,NULL,'BILLING',GETDATE(),'Installment Bill Sent to Borrower',0,0,NULL,NULL,NULL,NULL,NULL,0,0,GETDATE(),'UNH 59755')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

DECLARE @ArcAddId NUMERIC(18,0) = (SELECT SCOPE_IDENTITY())
--6
INSERT INTO ULS..ArcLoanSequenceSelection(ArcAddProcessingId, LoanSequence)
VALUES
	(@ArcAddId, 1),
	(@ArcAddId, 2),
	(@ArcAddId, 4),
	(@ArcAddId, 6),
	(@ArcAddId, 7),
	(@ArcAddId, 9)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--1
UPDATE 
	ULS.[print].PrintProcessing
SET
	ArcAddProcessingId = @ArcAddId
WHERE
	PrintProcessingId = '3201795'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END