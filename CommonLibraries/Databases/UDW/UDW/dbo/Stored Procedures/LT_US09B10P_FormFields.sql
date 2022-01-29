﻿
CREATE PROCEDURE [dbo].[LT_US09B10P_FormFields]
	@BF_SSN  CHAR(9)
AS

DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = 'US09B10P')

SELECT DISTINCT
	'$' + (CONVERT(VARCHAR(15), SUM((ISNULL(LN80.LN_LTE_FEE, 0.00) + ISNULL(LN80.LA_BIL_PAS_DU, 0.00))), 1)) AS PAS_DU_AMNT
FROM
	UDW..LN10_LON LN10
	LEFT JOIN UDW..FormatTranslation FT
		ON FT.Start = LN10.IC_LON_PGM
	LEFT JOIN 
	(
		SELECT	
			LN80.BF_SSN,
			LN80.LN_SEQ,
			ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) AS LN_LTE_FEE,
			LN80.LA_BIL_PAS_DU AS LA_BIL_PAS_DU
		FROM
			UDW..LN80_LON_BIL_CRF LN80
			INNER JOIN
			(
				SELECT	
					LN80.BF_SSN,
					LN80.LN_SEQ,
					MAX(LN80.LD_BIL_CRT) AS MAX_LD_BIL_CRT,
					MAX(LN80.LN_BIL_OCC_SEQ) AS MAX_LN_BIL_OCC_SEQ
				FROM
					UDW..LN80_LON_BIL_CRF LN80
				WHERE
					LN80.LC_STA_LON80 = 'A'
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ
			)LN80_MAX
				ON LN80_MAX.BF_SSN = LN80.BF_SSN
				AND LN80_MAX.LN_SEQ = LN80.LN_SEQ
				AND LN80_MAX.MAX_LD_BIL_CRT = LN80.LD_BIL_CRT
				AND LN80_MAX.MAX_LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
	)LN80
		ON LN80.BF_SSN = LN10.BF_SSN
		AND LN80.LN_SEQ = LN10.LN_SEQ
	INNER JOIN
	(
		SELECT
			LN85.BF_SSN,
			LN85.LN_SEQ
		FROM
			UDW..LN85_LON_ATY LN85
			INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
			(
				SELECT
					AY10.BF_SSN,
					MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
					MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM 
					UDW..AY10_BR_LON_ATY AY10
				WHERE
					AY10.PF_REQ_ACT = @PF_REQ_ACT
				GROUP BY
					AY10.BF_SSN
			)AY10
				ON AY10.BF_SSN = LN85.BF_SSN
				AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
	)LN85
		ON LN85.BF_SSN = LN10.BF_SSN
		AND LN85.LN_SEQ = LN10.LN_SEQ
WHERE 
	LN10.BF_SSN = @BF_SSN

RETURN 0