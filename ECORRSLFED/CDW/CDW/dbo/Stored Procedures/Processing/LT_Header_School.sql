﻿CREATE PROCEDURE [dbo].[LT_Header_School]
	@AccountNumber VARCHAR(10),
	@IsCoborrower BIT = 0
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT
		--borrower info
			REPLACE(LTRIM(RTRIM(SC10.IM_SCL_FUL)),',','') AS Name,
			--address info
			REPLACE(LTRIM(RTRIM(SC25.IX_SCL_STR_ADR_1)),',','')  AS Address1,
			REPLACE(LTRIM(RTRIM(SC25.IX_SCL_STR_ADR_2)),',','')  AS Address2,
			CASE WHEN LEN(LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE))) = 9 
				THEN REPLACE(LTRIM(RTRIM(SC25.IM_SCL_CT)),',','')  + ' ' + REPLACE(LTRIM(RTRIM(SC25.IC_SCL_DOM_ST)),',','')  + '  ' + LEFT(LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE)), 4)
				ELSE REPLACE(LTRIM(RTRIM(SC25.IM_SCL_CT)),',','') + ' ' + REPLACE(LTRIM(RTRIM(SC25.IC_SCL_DOM_ST)),',','') + '  ' + REPLACE(LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE)),',','')
			END AS CityStateZip,
			REPLACE(LTRIM(RTRIM(SC25.IM_SCL_FGN_ST)),',','')  AS ForeignState,
			REPLACE(LTRIM(RTRIM(SC25.IM_SCL_FGN_CNY)),',','') AS Country,
			SC10.IF_DOE_SCL AS AccountNumber,
			REPLACE(LTRIM(RTRIM(SC25.IM_SCL_CT)),',','')  AS City,
			REPLACE(LTRIM(RTRIM(SC25.IC_SCL_DOM_ST)),',','')  AS [State],
			CASE WHEN LEN(SC25.IF_SCL_ZIP_CDE) = 9 
				THEN LEFT(LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE)), 5) + '-' + RIGHT(LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE)), 4)
				ELSE LTRIM(RTRIM(SC25.IF_SCL_ZIP_CDE))
			END AS Zip,
			CASE WHEN SC25.II_SCL_VLD_ADR = 'Y' 
				THEN 1
				ELSE 0
			END AS HasValidAddress
	FROM
		LT20_LTR_REQ_PRC LT20
		INNER JOIN AY10_BR_LON_ATY AY10
			ON LT20.RF_SBJ_PRC = AY10.BF_SSN
			AND LT20.RN_ATY_SEQ_PRC = AY10.LN_ATY_SEQ
		INNER JOIN
		( 
			SELECT
				Final.*
			FROM
				SC25_SCH_DPT Final
				INNER JOIN
				(
					SELECT
						IF_DOE_SCL,
						IC_SCL_DPT,
						RANK() OVER(PARTITION BY IF_DOE_SCL, IC_SCL_DPT ORDER BY IC_SCL_DPT) AS Ranking
					FROM	
						SC25_SCH_DPT
					WHERE
						IC_SCL_DPT IN ('000','001','004')
						AND II_SCL_VLD_ADR = 'Y'
				) RankedAddress
					ON RankedAddress.IF_DOE_SCL = Final.IF_DOE_SCL
					AND RankedAddress.IC_SCL_DPT = Final.IC_SCL_DPT
					AND RankedAddress.Ranking = 1
		) SC25
			ON SC25.IF_DOE_SCL = AY10.LF_ATY_RCP
		INNER JOIN SC10_SCH_DMO SC10
			ON SC10.IF_DOE_SCL = SC25.IF_DOE_SCL
	WHERE
		SC10.IF_DOE_SCL = @AccountNumber
		AND
		(
			(
				LT20.PrintedAt IS NULL 
				AND LT20.OnEcorr = 0
			) 
			OR LT20.EcorrDocumentCreatedAt IS NULL
		)
		AND LT20.InactivatedAt IS NULL
		AND LT20.RI_LTR_REQ_DEL_PRC = 'N'