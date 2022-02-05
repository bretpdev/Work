﻿CREATE PROCEDURE [smbalwo].[GetLoansFromWarehouse]
AS

DECLARE @Today DATE = CAST(GETDATE() AS DATE)

MERGE
	ULS.[smbalwo].LoanWriteOffs LWO
USING
(
	SELECT
		PD10.DF_SPE_ACC_ID,
		LN10.LN_SEQ,
		(LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS, 0)) AS LOAN_BALANCE
	FROM
		UDW..LN10_LON LN10
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL_NONTILP.BAL,
				MAX(CASE
						WHEN (LN90.LC_STA_LON90 = 'A' AND ISNULL(LN90.LC_FAT_REV_REA, '') = '' AND LN90.PC_FAT_TYP = '10' AND LN90.PC_FAT_SUB_TYP IN ('70','80')) THEN 'CONSOLIDATED'
						ELSE 'NOT CONSOLIDATED'
					END
					) AS [STATUS],
				ABS(MIN(
						CASE
							WHEN ADJUSTMENT.LD_FAT_APL_DAYS IS NOT NULL THEN ADJUSTMENT.LD_FAT_APL_DAYS
							ELSE NULL
						END
						)
					) AS LD_FAT_APL_DAYS
			FROM
				UDW..LN10_LON LN10
				INNER JOIN
				(
					SELECT
						LN10.BF_SSN,
						SUM(LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) AS BAL
					FROM
						UDW..LN10_LON LN10
					WHERE
						LN10.IC_LON_PGM != 'TILP'
					GROUP BY
						LN10.BF_SSN
				) TOTAL_BAL_NONTILP
					ON LN10.BF_SSN = TOTAL_BAL_NONTILP.BF_SSN
				LEFT JOIN
				(
					SELECT
						LN10.BF_SSN,
						SUM(LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) AS BAL
					FROM
						UDW..LN10_LON LN10
					WHERE
						LN10.IC_LON_PGM = 'TILP'
					GROUP BY
						LN10.BF_SSN
				) TOTAL_BAL_TILP
					ON LN10.BF_SSN = TOTAL_BAL_TILP.BF_SSN
				INNER JOIN UDW..DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
					AND DW01.WC_DW_LON_STA != '23'
				INNER JOIN
				(
					SELECT
						LN15.BF_SSN,
						LN15.LN_SEQ,
						SUM(LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0)) DSB_AMT
					FROM
						UDW..LN15_DSB LN15
					WHERE
						LN15.LC_STA_LON15 = 1 
						AND LN15.LC_DSB_TYP = 2
					GROUP BY
						LN15.BF_SSN,
						LN15.LN_SEQ
				)LN15
					ON LN15.BF_SSN = LN10.BF_SSN
					AND LN15.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN UDW..LN90_FIN_ATY LN90
					ON LN90.BF_SSN = LN10.BF_SSN
					AND LN90.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN
				(
					SELECT DISTINCT
						LN90.BF_SSN,
						LN90.LN_SEQ,
						CASE
							WHEN LN90.LC_STA_LON90 = 'A' AND ISNULL(LN90.LC_FAT_REV_REA, '') = '' AND LN90.PC_FAT_TYP = '10' THEN DATEDIFF(DAY, LN90.LD_FAT_APL, GETDATE())
							ELSE NULL
						END AS LD_FAT_APL_DAYS
					FROM
						UDW..LN90_FIN_ATY LN90
					WHERE
						LN90.BF_SSN NOT IN
							(
								SELECT
									DISTINCT LN90.BF_SSN
								FROM
									UDW..LN90_FIN_ATY LN90
									INNER JOIN UDW..LN10_LON LN10
										ON LN90.BF_SSN = LN10.BF_SSN
								WHERE
									LN90.LC_STA_LON90 = 'A' AND ISNULL(LN90.LC_FAT_REV_REA, '') = '' AND LN90.PC_FAT_TYP = '10'
									AND
									LN10.IC_LON_PGM != 'TILP'
								GROUP BY
									LN90.BF_SSN,
									LN90.LN_SEQ,
									LN90.LD_FAT_APL
								HAVING
									DATEDIFF(DAY, LN90.LD_FAT_APL, GETDATE()) < 21 AND SUM(LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) < 25
							)
				) ADJUSTMENT
					ON ADJUSTMENT.BF_SSN = LN10.BF_SSN
					AND ADJUSTMENT.LN_SEQ = LN10.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = 'R'
				AND (LN10.LA_CUR_PRI + LN10.LA_NSI_OTS) BETWEEN .01 AND 24.99
				AND LN10.LA_CUR_PRI != LN15.DSB_AMT
				AND TOTAL_BAL_NONTILP.BAL < 25
				AND (ISNULL(TOTAL_BAL_TILP.BAL, 0.00) + TOTAL_BAL_NONTILP.BAL) < 74.99 --Check if the TILP balance is null and set to 0 then add it to the non tilp balance to make sure they are under $75
				AND LN10.IC_LON_PGM != 'TILP'
			GROUP BY
				LN10.BF_SSN,
				LN10.LN_SEQ,
				TOTAL_BAL_NONTILP.BAL
		) POP
			ON POP.BF_SSN = LN10.BF_SSN
			AND POP.LN_SEQ = LN10.LN_SEQ
	WHERE
		(POP.[STATUS] = 'CONSOLIDATED' AND POP.BAL < 25)
		OR
		(
			POP.[STATUS] = 'NOT CONSOLIDATED'
			AND POP.LD_FAT_APL_DAYS > 20 AND POP.BAL < 25
		)
)L 
	ON L.DF_SPE_ACC_ID = LWO.DF_SPE_ACC_ID
	AND L.LN_SEQ = LWO.LN_SEQ
	AND L.LOAN_BALANCE = LWO.LoanBalance
	AND	(CAST(LWO.AddedAT AS DATE) = @Today OR LWO.ProcessedAt IS NULL)
	AND LWO.DeletedAt IS NULL
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