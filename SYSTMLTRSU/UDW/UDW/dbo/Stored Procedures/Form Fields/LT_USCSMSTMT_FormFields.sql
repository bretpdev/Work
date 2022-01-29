﻿CREATE PROCEDURE [dbo].[LT_USCSMSTMT_FormFields]
	@BF_SSN  VARCHAR(9),
	@IsCoborrower BIT = 0,
	@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'USCSMSTMT')
DECLARE @TaxYear VARCHAR(4) = '2020'

IF @IsCoborrower = 0 --borrower
	BEGIN
		SELECT 
			@TaxYear AS TaxYear,
			REPLACE(FORMAT(SUM(ISNULL(MR65.WA_PRI_INC_CRD,0)), 'C'), ',', '') AS CancelledPrincipalBalance,
			REPLACE(FORMAT(SUM(ISNULL(MR65.WA_INT_INC_CRD,0)), 'C'), ',', '') AS InterestIncludedInCancellation
		FROM
			LN10_LON LN10 --Start with the borrowers loans
			INNER JOIN PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN
			(
				SELECT
					LN85.BF_SSN,
					LN85.LN_SEQ
				FROM
					LN85_LON_ATY LN85
					INNER JOIN AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN85.BF_SSN
						AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				WHERE   
					AY10.PF_REQ_ACT = @PF_REQ_ACT
					AND AY10.BF_SSN = @BF_SSN
					AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
					--Active flag ignored, as LT20 provides the exact record that is tied to this request
			)LN85
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ 
			INNER JOIN 
			(
				SELECT
					MR65.BF_SSN,
					MR65.LN_SEQ,
					MR65.LF_TAX_YR,
					MR65.WA_PRI_INC_CRD,
					MR65.WA_INT_INC_CRD
				FROM
					MR65_MSC_TAX_RPT MR65
					INNER JOIN
					(
						SELECT
							MR65.BF_SSN,
							MR65.LN_SEQ,
							MAX(WF_CRT_DTS_MR65) AS WF_CRT_DTS_MR65
						FROM			
							MR65_MSC_TAX_RPT MR65
						WHERE
							MR65.LF_TAX_YR = @TaxYear
							AND MR65.BF_SSN = @BF_SSN
							AND MR65.WC_STA_MR65 = 'A'
						GROUP BY
							MR65.BF_SSN,
							MR65.LN_SEQ
					) MR65_AGG
						ON MR65_AGG.BF_SSN = MR65.BF_SSN
						AND MR65_AGG.LN_SEQ = MR65.LN_SEQ
						AND MR65_AGG.WF_CRT_DTS_MR65 = MR65.WF_CRT_DTS_MR65
				WHERE
					MR65.WC_STA_MR65 = 'A'
			) MR65
				ON MR65.BF_SSN = LN10.BF_SSN
				AND MR65.LN_SEQ = LN10.LN_SEQ
				AND MR65.LF_TAX_YR = @TaxYear
		WHERE
			PD10.DF_PRS_ID = @BF_SSN
	END
ELSE --Coborrower
	BEGIN 
		SELECT 
		@TaxYear AS TaxYear,
		REPLACE(FORMAT(SUM(ISNULL(MR65.WA_PRI_INC_CRD,0)), 'C'), ',', '') AS CancelledPrincipalBalance,
		REPLACE(FORMAT(SUM(ISNULL(MR65.WA_INT_INC_CRD,0)), 'C'), ',', '') AS InterestIncludedInCancellation
	FROM
		LN10_LON LN10 --Start with the borrowers loans
		INNER JOIN LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		INNER JOIN
		(
			SELECT
				LN85.BF_SSN,
				LN85.LN_SEQ
			FROM
				LN85_LON_ATY LN85
				INNER JOIN AY10_BR_LON_ATY AY10
					ON AY10.BF_SSN = LN85.BF_SSN
					AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				INNER JOIN LN20_EDS LN20
					ON LN20.BF_SSN = LN85.BF_SSN
					AND LN20.LN_SEQ = LN85.LN_SEQ
					AND LN20.LC_STA_LON20 = 'A'
					AND LN20.LC_EDS_TYP = 'M'
			WHERE   
				AY10.PF_REQ_ACT = @PF_REQ_ACT
				AND LN20.LF_EDS = @BF_SSN
				AND AY10.LN_ATY_SEQ = @RN_ATY_SEQ_PRC
				--Active flag ignored, as LT20 provides the exact record that is tied to this request
		)LN85
			ON LN85.BF_SSN = LN10.BF_SSN
			AND LN85.LN_SEQ = LN10.LN_SEQ 
		INNER JOIN 
		(
			SELECT
				MR65.BF_SSN,
				MR65.LN_SEQ,
				MR65.LF_TAX_YR,
				MR65.WA_PRI_INC_CRD,
				MR65.WA_INT_INC_CRD
			FROM
				MR65_MSC_TAX_RPT MR65
				INNER JOIN
				(
					SELECT
						MR65.BF_SSN,
						MR65.LN_SEQ,
						MAX(WF_CRT_DTS_MR65) AS WF_CRT_DTS_MR65
					FROM			
						MR65_MSC_TAX_RPT MR65
						INNER JOIN LN20_EDS LN20
							ON LN20.BF_SSN = MR65.BF_SSN
							AND LN20.LN_SEQ = MR65.LN_SEQ
							AND LN20.LC_EDS_TYP = 'M'
							AND LN20.LC_STA_LON20 = 'A'
					WHERE
						MR65.LF_TAX_YR = @TaxYear
						AND LN20.LF_EDS = @BF_SSN
						AND MR65.WC_STA_MR65 = 'A'
					GROUP BY
						MR65.BF_SSN,
						MR65.LN_SEQ
				) MR65_AGG
					ON MR65_AGG.BF_SSN = MR65.BF_SSN
					AND MR65_AGG.LN_SEQ = MR65.LN_SEQ
					AND MR65_AGG.WF_CRT_DTS_MR65 = MR65.WF_CRT_DTS_MR65
			WHERE
				MR65.WC_STA_MR65 = 'A'
		) MR65
			ON MR65.BF_SSN = LN10.BF_SSN
			AND MR65.LN_SEQ = LN10.LN_SEQ
			AND MR65.LF_TAX_YR = @TaxYear
	WHERE
		LN20.LF_EDS = @BF_SSN
	END
END
GO

