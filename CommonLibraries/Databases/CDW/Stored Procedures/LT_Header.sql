CREATE PROCEDURE [dbo].[LT_Header]
(
	@AccountNumber		CHAR(10)
)
AS
BEGIN

	
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	DECLARE @RowCount int = 0

	SELECT
		--borrower info
		BORR.DM_PRS_1 + ' ' + BORR.DM_PRS_LST AS Name,
		--address info
		ADDR.DX_STR_ADR_1 AS Address1,
		ADDR.DX_STR_ADR_2 AS Address2,
		CASE	
			WHEN LEN(ADDR.DF_ZIP_CDE) = 9 THEN ADDR.DM_CT + ' ' + ADDR.DC_DOM_ST + '  ' + LEFT(ADDR.DF_ZIP_CDE, 5) + '-' + RIGHT(ADDR.DF_ZIP_CDE, 4)
			ELSE ADDR.DM_CT + ' ' + ADDR.DC_DOM_ST + '  ' + ADDR.DF_ZIP_CDE
		END AS CityStateZip,
		ADDR.DM_FGN_ST AS ForeignState,
		ADDR.DM_FGN_CNY AS Country,
		BORR.DF_SPE_ACC_ID AS AccountNumber,
		ADDR.DM_CT AS City,
		ADDR.DC_DOM_ST AS [State],
		CASE	
			WHEN LEN(ADDR.DF_ZIP_CDE) = 9 THEN LEFT(ADDR.DF_ZIP_CDE, 5) + '-' + RIGHT(ADDR.DF_ZIP_CDE, 4)
			ELSE ADDR.DF_ZIP_CDE
		END AS Zip,
		CASE
			WHEN DI_VLD_ADR = 'Y' THEN 1
			ELSE 0
		END AS HasValidAddress
	FROM
		dbo.PD10_Borrower BORR 
		INNER JOIN dbo.PD30_Address ADDR 
			ON BORR.DF_SPE_ACC_ID = ADDR.DF_SPE_ACC_ID
	WHERE 
		BORR.DF_SPE_ACC_ID = @AccountNumber

	SET @RowCount = @@ROWCOUNT
		
IF @RowCount != 1
BEGIN
	DECLARE @SQLStatement VARCHAR(MAX) = 
	'
		SELECT
			*
		FROM
			OPENQUERY
			(
				LEGEND,
				''
					SELECT
						TRIM(PD10.DM_PRS_1) || '''' '''' || TRIM(PD10.DM_PRS_LST) AS Name,
						TRIM(PD30.DX_STR_ADR_1) AS Address1,
						TRIM(PD30.DX_STR_ADR_2) AS Address2,
						CASE	
							WHEN LENGTH(TRIM(PD30.DF_ZIP_CDE)) = 9 THEN TRIM(PD30.DM_CT) || '''' '''' || TRIM(PD30.DC_DOM_ST) || ''''  '''' || LEFT(TRIM(PD30.DF_ZIP_CDE), 5) || ''''-'''' || RIGHT(TRIM(PD30.DF_ZIP_CDE), 4)
							ELSE TRIM(PD30.DM_CT) || '''' '''' || TRIM(PD30.DC_DOM_ST) || ''''  '''' || TRIM(PD30.DF_ZIP_CDE)
						END AS CityStateZip,
						TRIM(PD30.DM_FGN_ST) as ForeignState,
						TRIM(PD30.DM_FGN_CNY) as Country,
						PD10.DF_SPE_ACC_ID AS AccountNumber,
						TRIM(PD30.DM_CT) AS City,
						TRIM(PD30.DC_DOM_ST) as State,
						CASE	
							WHEN LENGTH(TRIM(PD30.DF_ZIP_CDE)) = 9 THEN LEFT(TRIM(PD30.DF_ZIP_CDE), 5) || ''''-'''' || RIGHT(TRIM(PD30.DF_ZIP_CDE), 4)
							ELSE TRIM(PD30.DF_ZIP_CDE)
						END AS Zip,
						CASE PD30.DI_VLD_ADR
							WHEN ''''Y'''' THEN 1
							ELSE 0
						END AS HasValidAddress
					FROM
						PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.PD30_PRS_ADR PD30 ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
					WHERE
						PD10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
				''
			)
	'

	--PRINT @SQLStatement
	EXEC(@SQLStatement)
	SET @RowCount = @@ROWCOUNT
END

IF @RowCount != 1
BEGIN
	RAISERROR('[dbo].[LT_Header] returned %i record(s) for account %s', 16, 1, @Rowcount, @AccountNumber)
END

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LT_Header] TO [db_executor]
    AS [dbo];
