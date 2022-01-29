﻿
CREATE PROCEDURE [smbalwo].[GetLoansFromWarehouse]
AS
EXEC('[smbalwo].[GetDeliquentAccounts]')
DECLARE @SQL VARCHAR(MAX) = '
MERGE
	ULS.[smbalwo].LoanWriteOffs LWO
USING
(
SELECT * FROM OPENQUERY(QADBD004,
''
	SELECT
		PD10.DF_SPE_ACC_ID,
		LN10.LN_SEQ,
		(LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS,0)) AS LOAN_BALANCE
	FROM
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL.BAL,
				MAX(CASE
						WHEN (LN90.LC_STA_LON90 = ''''A'''' AND LN90.PC_FAT_TYP = ''''10'''' AND LN90.PC_FAT_SUB_TYP IN (''''70'''',''''80'''')) THEN ''''CONSOLIDATED''''
						ELSE ''''NOT CONSOLIDATED''''
					END
					) AS STATUS,
				ABS(MAX(CASE
						WHEN LN90.LC_CSH_ADV = ''''C'''' THEN  DAYS(LN90.LD_FAT_APL) - DAYS(CURRENT_DATE) 
						ELSE NULL
					END 
					)) AS  LD_FAT_APL_DAYS
			FROM
				OLWHRM1.LN10_LON LN10
				INNER JOIN
				(
					SELECT
						LN10.BF_SSN,
						SUM((LN10.LA_CUR_PRI + LN10.LA_NSI_OTS)) AS BAL
					FROM
						OLWHRM1.LN10_LON LN10
					WHERE
						LN10.IC_LON_PGM != ''''TILP''''
					GROUP BY
						LN10.BF_SSN
				) TOTAL_BAL
					ON LN10.BF_SSN = TOTAL_BAL.BF_SSN
				INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
					AND DW01.WC_DW_LON_STA != ''''23''''
				INNER JOIN
				(
					SELECT
						LN15.BF_SSN,
						LN15.LN_SEQ,
						SUM((LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0))) DSB_AMT
					FROM
						OLWHRM1.LN15_DSB LN15
					GROUP BY
						LN15.BF_SSN,
						LN15.LN_SEQ
				)LN15
					ON LN15.BF_SSN = LN10.BF_SSN
					AND LN15.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN OLWHRM1.LN90_FIN_ATY LN90
					ON LN90.BF_SSN = LN10.BF_SSN
					AND LN90.LN_SEQ = LN10.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = ''''R''''
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) BETWEEN 0.01 AND 25
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) != LN15.DSB_AMT
				AND TOTAL_BAL.BAL < 25
				AND LN10.IC_LON_PGM != ''''TILP''''
			GROUP BY
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL.BAL
		)POP
			ON POP.BF_SSN = LN10.BF_SSN
			AND POP.LN_SEQ = LN10.LN_SEQ
	WHERE
		(POP.STATUS = ''''CONSOLIDATED'''' AND POP.BAL < 25)
		OR
		(
			(POP.STATUS = ''''NOT CONSOLIDATED'''' AND POP.LD_FAT_APL_DAYS > 54 AND POP.BAL BETWEEN 10 AND 25)
			OR
			(POP.STATUS = ''''NOT CONSOLIDATED'''' AND POP.LD_FAT_APL_DAYS > 20 AND POP.BAL <= 10)
		)

'') 
)L 
ON L.DF_SPE_ACC_ID = LWO.DF_SPE_ACC_ID
AND L.LN_SEQ = LWO.LN_SEQ	
WHEN NOT MATCHED THEN
	INSERT 
	(
		DF_SPE_ACC_ID,
		LN_SEQ,
		LoanBalance,
		IsDelinquent
	)
	VALUES
	(
		L.DF_SPE_ACC_ID,
		L.LN_SEQ,
		L.LOAN_BALANCE,
		0
	)
;
'
--SELECT @SQL
EXEC(@SQL)	

RETURN 0
