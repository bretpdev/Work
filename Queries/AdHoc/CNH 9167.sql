DECLARE @Populations TABLE
(
	CALLID VARCHAR(X)
	,ACCOUNTNO VARCHAR(XX)
	,SSN VARCHAR(XX)
)

INSERT INTO
	@Populations
(
	CALLID
	,ACCOUNTNO
	,SSN
)
VALUES
	 ('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')
	,('XXXXXXX','XXXXXXXXXX','XXXXXXXXX')

SELECT * FROM @Populations P
INNER JOIN NobleCalls.dbo.NobleCallHistory NCH
ON NCH.NobleCallHistoryId = P.CALLID

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE [NobleCalls].[dbo].[NobleCallHistory]
		SET AccountIdentifier = P.SSN
		FROM @Populations P
			INNER JOIN [NobleCalls].[dbo].[NobleCallHistory] NCH
				ON NCH.NobleCallHistoryId = P.CALLID
				AND NCH.AccountIdentifier = P.ACCOUNTNO
		WHERE NCH.NobleCallHistoryId = P.CALLID
			AND NCH.AccountIdentifier = P.ACCOUNTNO

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
		
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


