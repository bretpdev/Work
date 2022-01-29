USE UDW
GO

SELECT DISTINCT
	LN10.BF_SSN AS SSN,
	LN10.LN_SEQ AS LOAN_SEQ,
	LN10.LD_LON_IBR_ENT AS IBR_SUBSIDY,
	LN10.IF_DOE_LDR AS PREV_SERVICER,
	ISNULL(CONVERT(VARCHAR,LN90_MIN.LD_FAT_EFF, 101),'N/A') AS FIRST3901,
	ISNULL(CONVERT(VARCHAR,LN90_MAX.LD_FAT_EFF, 101),'N/A') AS LAST3901,
	LN10.LD_LON_EFF_ADD AS EFFECTIVE_LOAN_ADD
FROM
	UDW..LN10_LON LN10
	INNER JOIN
	(
		SELECT
			RS10.BF_SSN,
			LN65.LN_SEQ,
			MIN(RS10.LD_RPS_1_PAY_DU) AS LD_RPS_1_PAY_DU --GETS THE FIRST PAYMENT FOR THE BORROWERS IBR
		FROM 
			UDW..RS10_BR_RPD RS10
			INNER JOIN UDW..LN65_LON_RPS LN65
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
		WHERE
			LN65.LC_TYP_SCH_DIS = 'IB'
		GROUP BY
			RS10.BF_SSN,
			LN65.LN_SEQ
	) RS
		ON RS.BF_SSN = LN10.BF_SSN
		AND RS.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..LN09_RPD_PIO_CVN LN09
		ON LN09.BF_SSN = LN10.BF_SSN
		AND LN09.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..LNPC_LFG_PAY_DTL LNPC
		ON LNPC.BF_SSN = LN10.BF_SSN
		AND LNPC.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN 
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MAX(LD_FAT_EFF) AS LD_FAT_EFF
		FROM
			UDW..LN90_FIN_ATY
		WHERE
			LC_STA_LON90 = 'A'
			AND ISNULL(LC_FAT_REV_REA,'') = ''
			AND PC_FAT_TYP = '39'
			AND PC_FAT_SUB_TYP = '01'
		GROUP BY
			BF_SSN,
			LN_SEQ
	) LN90_MAX
		ON LN90_MAX.BF_SSN = LN10.BF_SSN
		AND LN90_MAX.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN 
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MIN(LD_FAT_EFF) AS LD_FAT_EFF
		FROM
			UDW..LN90_FIN_ATY
		WHERE
			LC_STA_LON90 = 'A'
			AND ISNULL(LC_FAT_REV_REA,'') = ''
			AND PC_FAT_TYP = '39'
			AND PC_FAT_SUB_TYP = '01'
		GROUP BY
			BF_SSN,
			LN_SEQ
	) LN90_MIN
		ON LN90_MIN.BF_SSN = LN10.BF_SSN
		AND LN90_MIN.LN_SEQ = LN10.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.IC_LON_PGM IN ('STFFRD','SUBCNS','SUBSPC')
	AND LN10.LF_LON_CUR_OWN NOT IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906','82976907','82976908')
	AND LN10.LD_LON_IBR_ENT <= LN10.LD_LON_EFF_ADD
