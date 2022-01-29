USE UDW
GO

DECLARE @BanaLoan VARCHAR(10) = '829769', --loans from BANA conversion
		@BanaBlanketCode VARCHAR(10) = '99999999', --BANA blanket school code for missing Original School Code
		@DummyCode VARCHAR(10) = '77777700' --pre-existing Dummy School Code for consolidated loans

DECLARE @ConsoLoan TABLE(LoanType VARCHAR(10))
		INSERT INTO @ConsoLoan VALUES ('CNSLDN'),('SUBCNS'),('SUBSPC'),('UNCNS'),('UNSPC') 

SELECT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN10.LF_DOE_SCL_ORG 'LF_DOE_SCL_ORG (current)', --original school date field
	CASE
		WHEN LN10.IC_LON_PGM IN 
			(
			SELECT LoanType 
			FROM @ConsoLoan
			) 
		THEN @DummyCode 
		ELSE @BanaBlanketCode
	END AS 'LF_DOE_SCL_ORG (new)'
FROM
	[dbo].[LN10_LON] LN10
WHERE
	LN10.LF_LON_CUR_OWN = @BanaLoan
	AND 
		(
		ISNULL(LN10.LF_DOE_SCL_ORG, '') = '' 
		OR LN10.LF_DOE_SCL_ORG = 00 --same as null?
		) --no original school date
ORDER BY 
	LN10.LF_DOE_SCL_ORG


BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE 
		[dbo].[LN10_LON]
	SET 
		LF_DOE_SCL_ORG = 
		(
		CASE
			WHEN IC_LON_PGM IN 
				(
				SELECT LoanType 
				FROM @ConsoLoan
				) 
			THEN @DummyCode 
			ELSE @BanaBlanketCode
		END
		)
	WHERE
		LF_LON_CUR_OWN = @BanaLoan
		AND 
			(
			ISNULL(LF_DOE_SCL_ORG, '') = '' 
			OR LF_DOE_SCL_ORG = 00 --same as null?
			) --no original school date

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 0 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		--COMMIT TRANSACTION
		ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
