﻿CREATE PROCEDURE [cntrprnt].[GetBorrowerEcorrInfo]
	@AccountNumber char(10)
AS

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AccountNumber,
	PD10.DF_PRS_ID Ssn,
	CAST(CASE WHEN PH05.DI_CNC_ELT_OPI = 'Y' AND LN10.BF_SSN IS NOT NULL THEN 1 ELSE 0 END AS BIT) OptedIntoEcorrLetters,
	PH05.DX_CNC_EML_ADR EmailAddress, 
	CAST(CASE WHEN PH05.DI_VLD_CNC_EML_ADR = 'Y' THEN 1 ELSE 0 END AS BIT) EmailAddressIsValid
FROM
	PD10_PRS_NME PD10
	LEFT JOIN PH05_CNC_EML PH05 ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
	LEFT JOIN LN10_LON LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID 
		AND LN10.LA_CUR_PRI > 0 --principal balance greater than zero
		AND LN10.LC_STA_LON10 = 'R'
WHERE
	PD10.DF_SPE_ACC_ID = @AccountNumber

RETURN 0