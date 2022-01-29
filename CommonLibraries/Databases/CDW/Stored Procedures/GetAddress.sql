
CREATE PROCEDURE [dbo].[GetAddress]
	@AccountNumber CHAR(10)
AS

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
					TRIM(PD10.DM_PRS_1) AS DM_PRS_1,
					TRIM(PD10.DM_PRS_LST) AS DM_PRS_LST,
					TRIM(PD10.DM_PRS_1) || '''' '''' || TRIM(PD10.DM_PRS_LST) AS Name,
					TRIM(PD30.DX_STR_ADR_1) AS Address1,
					TRIM(PD30.DX_STR_ADR_2) AS Address2,
					CASE	
						WHEN LENGTH(TRIM(PD30.DF_ZIP_CDE)) = 9 THEN TRIM(PD30.DM_CT) || '''' '''' || TRIM(PD30.DC_DOM_ST) || ''''  '''' || LEFT(TRIM(PD30.DF_ZIP_CDE), 5) || ''''-'''' || RIGHT(TRIM(PD30.DF_ZIP_CDE), 4)
						ELSE TRIM(PD30.DM_CT) || '''' '''' || TRIM(PD30.DC_DOM_ST) || ''''  '''' || TRIM(PD30.DF_ZIP_CDE)
					END AS CityStateZip,
					TRIM(PD30.DM_FGN_ST) as ForeignState,
					TRIM(PD30.DM_FGN_CNY) as Country,
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


PRINT @SQLStatement
EXEC(@SQLStatement)

IF @@ROWCOUNT != 1
BEGIN
	RAISERROR('[dbo].[GetAddress] for account %s returned no records.', 16, 1, @AccountNumber)
END
