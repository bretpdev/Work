USE NobleCalls
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	INSERT INTO CallCampaigns(CallCampaign, RegionId)
	VALUES('ARMY',X),
			('BDDX',X),
			('BOA',X),
			('CORX',X),
			('CORM',X),
			('CPLT',X),
			('CSPN',X),
			('CWEL',X),
			('DIR',X),
			('DNOW',X),
			('INBD',X),
			('INBN',X),
			('INCH',X),
			('INCS',X),
			('INSC',X),
			('SCRA',X),
			('UCSK',X),
			('UNOW',X),
			('USPN',X),
			('USTN',X),
			('WYOM',X),
			('XCMP',X)

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
