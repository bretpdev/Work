USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		PP
	SET
		PP.PrintedAt = NULL
	FROM
		CLS.[print].PrintProcessing PP
		LEFT JOIN CLS.[print].ScriptData SD
			ON PP.ScriptDataId = SD.ScriptDataId
		LEFT JOIN CLS.[print].Letters L
			ON SD.LetterId = L.LetterId
	WHERE
		L.Letter IN ('APREQFED','FINBLNOPFD')
		AND
		CAST(PP.AddedAt AS DATE) = 'X/X/XX'
		AND
		PP.OnEcorr = X
		AND
		PP.PrintedAt IS NOT NULL

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XX AND @ERROR = X
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