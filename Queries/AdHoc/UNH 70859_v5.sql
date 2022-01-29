USE UDW
GO

SELECT
P.*,
LK10.PX_DSC_MDM
FROM
(
SELECT DISTINCT
	unh.*,
	BILL_BEFORE.LD_BIL_CRT as B_LD_BIL_CRT,
	BILL_BEFORE.LD_NXT_PAY_DUE_AHD AS B_LD_NXT_PAY_DUE_AHD,
	BILL_AFTER.LD_BIL_CRT as A_LD_BIL_CRT,
	BILL_AFTER.LD_NXT_PAY_DUE_AHD AS A_LD_NXT_PAY_DUE_AHD
FROM
	UDW..UNH70859 UNH
	INNER JOIN 
	(
		SELECT	
			BF_SSN,
			SUM(LA_CUR_PRI) AS LA_CUR_PRI
		FROM
			UDW..LN10_LON
		GROUP BY
			BF_SSN
		HAVING SUM(LA_CUR_PRI) > 0
	) LN10
		ON LN10.BF_SSN = UNH.SSN
	INNER JOIN
	(
		SELECT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LD_BIL_CRT,
			LN80.LD_NXT_PAY_DUE_AHD
		FROM
			UDW..LN80_LON_BIL_CRF LN80
			INNER JOIN UDW..BL10_BR_BIL BL10
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
			INNER JOIN
			(
				SELECT
					LN80.BF_SSN,
					LN80.LN_SEQ,
					MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT
				FROM
					UDW..LN80_LON_BIL_CRF LN80
					INNER JOIN UDW..UNH70859 UNH
						ON UNH.SSN = LN80.BF_SSN
						AND UNH.[LOAN SEQ] = LN80.LN_SEQ
					INNER JOIN UDW..BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				WHERE
					LN80.LD_BIL_CRT <= '02/25/2021'
					AND BL10.LC_IND_BIL_SNT = '1'
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ
			)M_BILL
				on M_BILL.BF_SSN = LN80.BF_SSN
				AND M_BILL.LN_SEQ = LN80.LN_SEQ
				AND M_BILL.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE
				BL10.LC_IND_BIL_SNT IN('1','G')
	) BILL_BEFORE
		ON BILL_BEFORE.BF_SSN = UNH.SSN
		AND BILL_BEFORE.LN_SEQ = UNH.[LOAN SEQ]
	INNER JOIN
	(
		SELECT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LD_BIL_CRT,
			LN80.LD_NXT_PAY_DUE_AHD
		FROM
			UDW..LN80_LON_BIL_CRF LN80
			INNER JOIN UDW..BL10_BR_BIL BL10
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
			INNER JOIN
			(
				SELECT
					LN80.BF_SSN,
					LN80.LN_SEQ,
					MIN(LN80.LD_BIL_CRT) AS LD_BIL_CRT
				FROM
					UDW..LN80_LON_BIL_CRF LN80
					INNER JOIN UDW..UNH70859 UNH
						ON UNH.SSN = LN80.BF_SSN
						AND UNH.[LOAN SEQ] = LN80.LN_SEQ
					INNER JOIN UDW..BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				WHERE
					LN80.LD_BIL_CRT > '02/25/2021'
					AND BL10.LC_IND_BIL_SNT IN('1','G')
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ
			)M_BILL
				on M_BILL.BF_SSN = LN80.BF_SSN
				AND M_BILL.LN_SEQ = LN80.LN_SEQ
				AND M_BILL.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE
				BL10.LC_IND_BIL_SNT IN('1','G')
	) BILL_AFTER
		ON BILL_AFTER.BF_SSN = UNH.SSN
		AND BILL_AFTER.LN_SEQ = UNH.[LOAN SEQ]
	
--where
--	unh.SSN = '001680975'
) P
INNER JOIN UDW..DW01_DW_CLC_CLU DW01
	ON DW01.BF_SSN = P.SSN
	AND DW01.LN_SEQ = P.[LOAN SEQ]
INNER JOIN UDW..LK10_LS_CDE_LKP LK10
	ON LK10.PM_ATR = 'WC-DW-LON-STA'
	AND LK10.PX_ATR_VAL = DW01.WC_DW_LON_STA
WHERE 
	P.B_LD_NXT_PAY_DUE_AHD IS NOT NULL
	AND
	(
		P.A_LD_NXT_PAY_DUE_AHD IS NULL
		OR
		P.A_LD_NXT_PAY_DUE_AHD < P.B_LD_NXT_PAY_DUE_AHD
	)
	AND LK10.PX_DSC_MDM IN ('IN REPAYMENT','PRE-CLAIM SUBMITTED')
ORDER BY
	P.SSN,
	P.[LOAN SEQ]

--2500