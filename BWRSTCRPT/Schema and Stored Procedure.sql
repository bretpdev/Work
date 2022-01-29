GO
USE CentralData;
GO

CREATE SCHEMA [bwrstcrpt] AUTHORIZATION [dbo];
GO

GRANT EXECUTE ON SCHEMA ::[bwrstcrpt] TO [db_executor]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [bwrstcrpt].[GetBorrowerStateCount]
	@Date DATE
AS

--DECLARE @DATE DATE = CAST(GETDATE() AS DATE)
SELECT
	LTRIM(RTRIM(POP.State)) AS [State],
	COUNT(DISTINCT POP.AccountNumber) AS BorrowerCount,
	COUNT(POP.LoanSequence) AS LoanCount
FROM
	(
		SELECT DISTINCT
			COALESCE(PD30_ADR.DC_DOM_ST, PD31_ADR.DC_DOM_ST_HST, '') AS [State],
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			LN10.LN_SEQ AS LoanSequence
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
				AND LC_STA_LON10 = 'R'
				AND LA_CUR_PRI > 0.00
				AND IC_LON_PGM != 'TILP'
			LEFT JOIN 
			(
				SELECT
					PD30.DF_PRS_ID,
					PD30.DC_DOM_ST
				FROM
					UDW..PD30_PRS_ADR PD30
				WHERE
					PD30.DC_ADR = 'L'
					--AND PD30.DI_VLD_ADR = 'Y' --Purposefully ignoring address validity
					AND ISNULL(LTRIM(RTRIM(PD30.DC_DOM_ST)), '') != ''
					AND @Date >= PD30.DD_STA_PDEM30
		
			) PD30_ADR
				ON PD30_ADR.DF_PRS_ID = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT	
					PD31.DF_PRS_ID,
					PD31.DC_DOM_ST_HST
				FROM
					(
						SELECT
							MAX(PD31.DN_ADR_SEQ) AS TGT_SEQ,
							PD31.DF_PRS_ID
						FROM
							UDW..PD31_PRS_INA PD31
						WHERE
							PD31.DC_ADR_HST = 'L'
							--AND PD31.DI_VLD_ADR_HST = 'Y' --Purposefully ignoring address validity
							AND ISNULL(LTRIM(RTRIM(PD31.DC_DOM_ST_HST)), '') != ''
							AND @Date >= CAST(PD31.DF_LST_DTS_PD31 AS DATE)
						GROUP BY
							PD31.DF_PRS_ID
					) PD31_MAX
					INNER JOIN UDW..PD31_PRS_INA PD31
						ON PD31.DF_PRS_ID = PD31_MAX.DF_PRS_ID
						AND PD31.DN_ADR_SEQ = PD31_MAX.TGT_SEQ
			) PD31_ADR
				ON PD31_ADR.DF_PRS_ID = PD10.DF_PRS_ID
	) POP
WHERE
	POP.State IS NOT NULL
	AND POP.State != ''
	AND POP.State NOT IN ('AE', 'AP')
GROUP BY
	POP.State
ORDER BY
	POP.State