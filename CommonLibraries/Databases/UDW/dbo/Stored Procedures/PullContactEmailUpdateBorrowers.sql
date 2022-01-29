﻿CREATE PROCEDURE [dbo].[PullContactEmailUpdateBorrowers]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	SELECT
		AccountNumber, 
		BF_SSN,
		EmailAddress,
		SourceCode,
		CONVERT(VARCHAR,ValidityDate,101) AS ValidityDate
	FROM
		OPENQUERY
		(
			QADBD004,
			'
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID AS AccountNumber,
					LN10.BF_SSN AS BF_SSN,
					COALESCE(PD32H.DX_ADR_EML,PD32A.DX_ADR_EML,PD32W.DX_ADR_EML) AS EmailAddress,
					COALESCE(PD32H.DC_SRC_ADR,PD32A.DC_SRC_ADR,PD32W.DC_SRC_ADR) AS SourceCode,
					COALESCE(PD32H.DD_VER_ADR_EML,PD32A.DD_VER_ADR_EML,PD32W.DD_VER_ADR_EML) AS ValidityDate,
					PH05.DI_VLD_CNC_EML_ADR AS EmailValidityFlag
				FROM
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32H
						ON PD32H.DF_PRS_ID = PD10.DF_PRS_ID
						AND PD32H.DC_ADR_EML = ''H''
						AND PD32H.DI_VLD_ADR_EML = ''Y''
						AND PD32H.DC_STA_PD32 = ''A''
					LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32A
						ON PD32A.DF_PRS_ID = PD10.DF_PRS_ID
						AND PD32A.DC_ADR_EML = ''A''
						AND PD32A.DI_VLD_ADR_EML = ''Y''
						AND PD32A.DC_STA_PD32 = ''A''
					LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32W
						ON PD32W.DF_PRS_ID = PD10.DF_PRS_ID
						AND PD32W.DC_ADR_EML = ''W''
						AND PD32W.DI_VLD_ADR_EML = ''Y''
						AND PD32W.DC_STA_PD32 = ''A''
					INNER JOIN OLWHRM1.PH05_CNC_EML PH05
						ON RIGHT(''0000000000'' + CAST(PH05.DF_SPE_ID AS VARCHAR(10)), 10) = PD10.DF_SPE_ACC_ID
				WHERE
					LN10.LA_CUR_PRI > 0
					AND LN10.LC_STA_LON10 = ''R''
					AND PH05.DI_VLD_CNC_EML_ADR != ''Y''
					AND /*Has at least 1 valid email*/
					(
						PD32H.DF_PRS_ID IS NOT NULL 
						OR PD32A.DF_PRS_ID IS NOT NULL 
						OR PD32W.DF_PRS_ID IS NOT NULL 
					)
			'
		)
END