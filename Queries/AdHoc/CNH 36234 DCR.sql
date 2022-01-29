USE CLS
GO
BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXXXX

--XXXXX
UPDATE
	PP
SET
	PP.PrintedAt = NULL,
	PP.DeletedAt = NULL,
	PP.DeletedBy = NULL
FROM
	CLS.[print].Letters L
	INNER JOIN CLS.[print].ScriptData SD
		ON SD.LetterId = L.LetterId
	INNER JOIN CLS.[print].PrintProcessing PP
		ON PP.ScriptDataId = SD.ScriptDataId
WHERE
	L.Letter IN ('GRADLETFED','ENRDEFNFED','WTHDRWFED')
	AND OnEcorr = X


SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END