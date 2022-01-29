--run on UHEAASQLDB
USE UDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 130

	DECLARE @Date DATETIME = [CentralData].dbo.AddBusinessDays(GETDATE(), -2)

	--89 rows
	UPDATE
		DD
	SET
		DD.Active = 0
	FROM
		EcorrUheaa..[DocumentDetails] DD
	WHERE
		DD.Active = 1
		AND
		(
			( -- document has not been printed (really means processed) within 2 business days of DocDate
				DD.DocDate < @Date
				AND
				DD.Printed IS NULL
			)
			OR
			( -- document was created (really means processed) but has not been sent to AES
				DD.Printed BETWEEN '2016-1-1' /*exclude records before tracking enhancement*/ AND DATEADD(DAY, -3, GETDATE())
				AND
				DD.ZipFileName IS NULL
			)
		)
		AND
		DD.AddedAt < [CentralData].dbo.AddBusinessDays(GETDATE(), -2)

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	--65
	UPDATE
		PP
	SET
		PP.EcorrDocumentCreatedAt = NULL
	FROM
		EcorrUheaa..[DocumentDetails] DD
		INNER JOIN ULS.[print].PrintProcessing PP
			ON PP.AccountNumber = DD.ADDR_ACCT_NUM
			AND CAST(PP.AddedAt AS DATE) = CAST(DD.CreateDate AS DATE)
	WHERE
		DD.Active = 1
		AND
		(
			( -- document has not been printed (really means processed) within 2 business days of DocDate
				DD.DocDate < @Date
				AND
				DD.Printed IS NULL
			)
			OR
			( -- document was created (really means processed) but has not been sent to AES
				DD.Printed BETWEEN '2016-1-1' /*exclude records before tracking enhancement*/ AND DATEADD(DAY, -3, GETDATE())
				AND
				DD.ZipFileName IS NULL
			)
		)
		AND
		DD.AddedAt < [CentralData].dbo.AddBusinessDays(GETDATE(), -2)

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
