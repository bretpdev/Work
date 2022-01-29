﻿CREATE PROCEDURE [smbalwo].[GetLoansFromWarehouse]

AS
	MERGE
	CLS.[smbalwo].LoanWriteOffs LWO
USING
(
SELECT * FROM OPENQUERY(LEGEND,
'
	SELECT
		PD10.DF_SPE_ACC_ID,
		LN10.LN_SEQ,
		(LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) AS LOAN_BALANCE
	FROM
		PKUB.LN10_LON LN10
		INNER JOIN PKUB.PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL.BAL,
				ABS(MAX(CASE
						WHEN LN90.LC_CSH_ADV = ''C'' THEN  DAYS(LN90.LD_FAT_APL) - DAYS(CURRENT_DATE) 
						ELSE NULL
					END 
					)) AS  LD_FAT_APL_DAYS
			FROM
				PKUB.LN10_LON LN10
				INNER JOIN
				(
					SELECT
						LN10.BF_SSN,
						SUM((LN10.LA_CUR_PRI + LN10.LA_NSI_OTS)) AS BAL
					FROM
						PKUB.LN10_LON LN10
					GROUP BY
						LN10.BF_SSN
				) TOTAL_BAL
					ON LN10.BF_SSN = TOTAL_BAL.BF_SSN
				INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
					AND DW01.WC_DW_LON_STA != ''23''
				INNER JOIN
				(
					SELECT
						LN15.BF_SSN,
						LN15.LN_SEQ,
						SUM((LN15.LA_DSB - LN15.LA_DSB_CAN)) DSB_AMT
					FROM
						PKUB.LN15_DSB LN15
					GROUP BY
						LN15.BF_SSN,
						LN15.LN_SEQ
				)LN15
					ON LN15.BF_SSN = LN10.BF_SSN
					AND LN15.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN PKUB.LN90_FIN_ATY LN90
					ON LN90.BF_SSN = LN10.BF_SSN
					AND LN90.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN PKUR.MR5A_LON_MTH_SSH_12 MR5A
					ON MR5A.BF_SSN = LN10.BF_SSN
					AND MR5A.LN_SEQ = LN10.LN_SEQ
			WHERE
				(LN10.LC_STA_LON10 = ''R'' OR (LN10.LC_STA_LON10 = ''D'' AND COALESCE(MR5A.WN_DAY_DLQ_ISL, 0) <= 365))
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) BETWEEN 0.01 AND 25
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) != LN15.DSB_AMT
				AND TOTAL_BAL.BAL < 25
			GROUP BY
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL.BAL
		)POP
			ON POP.BF_SSN = LN10.BF_SSN
			AND POP.LN_SEQ = LN10.LN_SEQ
	WHERE
		POP.LD_FAT_APL_DAYS > 54 AND POP.BAL < 25
') 
)L 
ON L.DF_SPE_ACC_ID = LWO.DF_SPE_ACC_ID
AND L.LN_SEQ = LWO.LN_SEQ	
WHEN NOT MATCHED THEN
	INSERT 
	(
		DF_SPE_ACC_ID,
		LN_SEQ,
		LoanBalance
	)
	VALUES
	(
		L.DF_SPE_ACC_ID,
		L.LN_SEQ,
		L.LOAN_BALANCE
	)
;

RETURN 0