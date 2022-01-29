﻿
CREATE PROCEDURE [docid].[CaseNumberSearch]
	@CaseNumber varchar(12)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @DVAL DATE = DATEADD(YEAR, -10, GETDATE());

--OneLink bankruptcy
SELECT DISTINCT 
	PD01.DF_PRS_ID [AccountIdentifier]
	,RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) [Name],
	1 IsOneLink
FROM
	ODW..DC01_LON_CLM_INF DC01
	INNER JOIN ODW..DC18_BKR DC18
		ON DC01.AF_APL_ID = DC18.AF_APL_ID
		AND DC01.AF_APL_ID_SFX = DC18.AF_APL_ID_SFX
		AND DC01.LF_CRT_DTS_DC10 = DC18.LF_CRT_DTS_DC10
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
WHERE
	DC18.LD_BKR_FIL > @DVAL
	AND
	REPLACE(RTRIM(DC18.LF_BKR_DKT), '-', '') = REPLACE(RTRIM(@CaseNumber), '-', '')

RETURN 0;