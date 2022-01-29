﻿CREATE PROCEDURE [dbo].[LT_US09B10CP_FormFields]
		@BF_SSN  VARCHAR(9),
		@IsCoborrower BIT = 0,
		@RN_ATY_SEQ_PRC INT
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US09B10CP')

--Special case letter to coborrowers only
	SELECT 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS BorrowersName,
		CASE WHEN SUM((ISNULL(LN80.LN_LTE_FEE, 0.00) + ISNULL(LN80.LA_BIL_PAS_DU, 0.00))) != 0 
			THEN '$' + (CONVERT(VARCHAR(15), SUM((ISNULL(LN80.LN_LTE_FEE, 0.00) + ISNULL(LN80.LA_BIL_PAS_DU, 0.00))), 1))
			ELSE '$' + (CONVERT(VARCHAR(15), SUM((ISNULL(LN80.LA_BIL_CUR_DU, 0.00))), 1))
		END AS TotalAmtDue
	FROM
		LN10_LON LN10
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
		LEFT JOIN 
		(
			SELECT	
				LN80.BF_SSN,
				LN80.LN_SEQ,
				ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) AS LN_LTE_FEE,
				ISNULL(LN80.LA_BIL_PAS_DU,0.00) AS LA_BIL_PAS_DU,
				ISNULL(LN80.LA_BIL_CUR_DU,0.00) AS LA_BIL_CUR_DU
			FROM
				LN80_LON_BIL_CRF LN80
				INNER JOIN
				(
					SELECT	
						LN80.BF_SSN,
						LN80.LN_SEQ,
						MAX(LN80.LD_BIL_CRT) AS MAX_LD_BIL_CRT
					FROM
						LN80_LON_BIL_CRF LN80
					WHERE
						LN80.LC_STA_LON80 = 'A'
					GROUP BY
						LN80.BF_SSN,
						LN80.LN_SEQ
				)LN80_MAX
					ON LN80_MAX.BF_SSN = LN80.BF_SSN
					AND LN80_MAX.LN_SEQ = LN80.LN_SEQ
					AND LN80_MAX.MAX_LD_BIL_CRT = LN80.LD_BIL_CRT
		)LN80
			ON LN80.BF_SSN = LN10.BF_SSN
			AND LN80.LN_SEQ = LN10.LN_SEQ
	WHERE
		LN10.BF_SSN = @BF_SSN
	GROUP BY
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST)
END