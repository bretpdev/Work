--run on UHEAASQLDB
USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX

DECLARE @Date DATETIME = [CentralData].dbo.AddBusinessDays(GETDATE(), -X)

	--XX rows
	UPDATE
		DD
	SET
		DD.Active = X
	FROM
		ECorrFed..[DocumentDetails] DD
	WHERE
		DD.Active = X
		AND
		(
			( -- document has not been printed (really means processed) within X business days of DocDate
				DD.DocDate < @Date
				AND
				DD.Printed IS NULL
			)
			OR
			( -- document was created (really means processed) but has not been sent to AES
				DD.Printed BETWEEN 'XXXX-X-X' /*exclude records before tracking enhancement*/ AND DATEADD(DAY, -X, GETDATE())
				AND
				DD.ZipFileName IS NULL
			)
		)
		AND
		DD.AddedAt < [CentralData].dbo.AddBusinessDays(GETDATE(), -X)


	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--XX rows
	UPDATE
		PP
	SET
		PP.EcorrDocumentCreatedAt = NULL
	FROM
		ECorrFed..[DocumentDetails] DD
		INNER JOIN CLS.billing.PrintProcessing PP
			ON PP.AccountNumber = DD.ADDR_ACCT_NUM
			AND CAST(PP.AddedAt AS DATE) = CAST(DD.CreateDate AS DATE)
	WHERE
		DD.Active = X
		AND
		(
			( -- document has not been printed (really means processed) within X business days of DocDate
				DD.DocDate < @Date
				AND
				DD.Printed IS NULL
			)
			OR
			( -- document was created (really means processed) but has not been sent to AES
				DD.Printed BETWEEN 'XXXX-X-X' /*exclude records before tracking enhancement*/ AND DATEADD(DAY, -X, GETDATE())
				AND
				DD.ZipFileName IS NULL
			)
		)
		AND
		DD.AddedAt < [CentralData].dbo.AddBusinessDays(GETDATE(), -X)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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
