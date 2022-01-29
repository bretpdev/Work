USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

INSERT INTO CLS.dbo.SystemLetterExclusionReasons(SystemLetterExclusionReason) VALUES('Not fulfilled within relevant time frame')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

UPDATE
	CDW.dbo.LTXX_LetterRequests 
SET
	InactivatedAt = GETDATE(),
	SystemLetterExclusionReasonId = X
WHERE 
	PrintedAt IS NULL 
	AND InactivatedAt IS NULL 
	AND RT_RUN_SRT_DTS_PRC < 'XXXX-XX-XX' 
	AND RM_DSC_LTR_PRC IN ('TSXXBFXXXJ','TSXXBFXXXC','TSXXBDXXX','TSXXBFXXX') 

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = XXX AND @ERROR = X
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


	select * from cls.dbo.SystemLetterExclusionReasons