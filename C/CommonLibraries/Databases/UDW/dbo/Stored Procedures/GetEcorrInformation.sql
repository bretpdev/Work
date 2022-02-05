
CREATE PROCEDURE [dbo].[GetEcorrInformation]
    @AccountNumber varchar(10)
AS

DECLARE @RowCount int = 0

SELECT 
	@AccountNumber [AccountNumber],
	COALESCE(PH05.DX_CNC_EML_ADR, 'Ecorr@UHEAA.org') [EmailAddress],
	CASE PH05.DI_VLD_CNC_EML_ADR WHEN 'Y' THEN 1 ELSE 0 END AS ValidEmail,
	CASE PH05.DI_CNC_ELT_OPI WHEN 'Y' THEN 1 ELSE 0 END AS LetterIndicator,
	CASE PH05.DI_CNC_EBL_OPI WHEN 'Y' THEN 1 ELSE 0 END AS EbillIndicator,
	CASE PH05.DI_CNC_TAX_OPI WHEN 'Y' THEN 1 ELSE 0 END AS TaxIndicator
FROM
	[dbo].PD10_PRS_NME PD10
	LEFT JOIN PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
WHERE
	PD10.DF_SPE_ACC_ID = @AccountNumber

SET @RowCount = @@ROWCOUNT

IF @RowCount != 1
BEGIN
	RAISERROR('[dbo].[GetEcorrInformation] returned %i record(s) for account %s', 16, 1, @Rowcount, @AccountNumber)
END