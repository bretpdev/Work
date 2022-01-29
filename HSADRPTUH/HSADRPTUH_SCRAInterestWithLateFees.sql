/***************** UNH 68829 SCRA Interest With Late Fees (population requested by Compliance) *************************/
--declare @ssn varchar(9) = (select df_prs_id from udw..pd10_prs_nme where df_spe_acc_id = '') --TEST FOR SINGLE BORROWER

SELECT DISTINCT
	DC.BorrSSN,
	DC.Loan,
	DC.StatusDate,
	DC.BeginBrwr,
	DC.EndBrwr,
	LN10.LD_LON_1_DSB,
	DateCalcs.EarliestActiveDutyBeginDate,
	DC.BorrActive,
	DC.EndrActive,
	LN90.PC_FAT_TYP,
	LN90.PC_FAT_SUB_TYP,
	LN90.LC_FAT_REV_REA,
	IIF(CAST(LN90.LD_FAT_EFF AS DATE) BETWEEN DC.BeginBrwr AND DC.EndBrwr, 'Y', 'N') AS LateFeeBetweenSCRAPeriod,
	CAST(LN90.LD_FAT_EFF AS DATE) AS LD_FAT_EFF,
	LN72.LR_INT_RDC_PGM_ORG
FROM 
	ULS.hsadrptuh.DataComparison DC
	INNER JOIN UDW..LN10_LON LN10 --ULS.hsadrptuh.LN10_LON LN10 --opsdev
		ON DC.BorrSSN = LN10.BF_SSN
		AND DC.Loan = LN10.LN_SEQ		
	INNER JOIN UDW..LN90_FIN_ATY LN90 --ULS.hsadrptuh.LN90_FIN_ATY LN90 --opsdev
		ON LN10.BF_SSN = LN90.BF_SSN
		AND LN10.LN_SEQ = LN90.LN_SEQ
	INNER JOIN
	(--determine earliest begin and most recent end dates for active duty
		SELECT
			ADBegin_MIN.BorrSSN,
			CASE
				WHEN ADBegin_MIN.BeginBrwr IS NOT NULL
					AND ADBegin_MIN.BeginEndr IS NOT NULL
				THEN 
					CASE
						WHEN ADBegin_MIN.BeginBrwr < ADBegin_MIN.BeginEndr
						THEN ADBegin_MIN.BeginBrwr
						ELSE ADBegin_MIN.BeginEndr
					END
				ELSE
					ISNULL(ADBegin_MIN.BeginBrwr, ADBegin_MIN.BeginEndr)
			END AS EarliestActiveDutyBeginDate
		FROM
			(--active duty BEGIN date earliest dates
				SELECT
					BorrSSN,
					MIN(BeginBrwr) AS BeginBrwr,
					MIN(BeginEndr) AS BeginEndr
				FROM
					ULS.hsadrptuh.DataComparison
				WHERE
					BorrActive IS NOT NULL
					OR EndrActive IS NOT NULL
				GROUP BY
					BorrSSN
			) ADBegin_MIN
	) DateCalcs
		ON DC.BorrSSN = DateCalcs.BorrSSN
	LEFT JOIN UDW..LN72_INT_RTE_HST LN72
		ON LN90.BF_SSN = LN72.BF_SSN
		AND LN90.LN_SEQ = LN72.LN_SEQ
		AND LN90.LD_FAT_EFF BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
		AND LN72.LC_STA_LON72 = 'A'
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LD_LON_1_DSB < DateCalcs.EarliestActiveDutyBeginDate
	AND LN90.PC_FAT_TYP = '26'
	AND LN90.PC_FAT_SUB_TYP = '01'
	AND LN90.LC_STA_LON90 = 'A'
	AND (
			LN90.LC_FAT_REV_REA = ' '
			OR LN90.LC_FAT_REV_REA IS NULL
		)
	AND CAST(LN90.LD_FAT_EFF AS DATE) BETWEEN DC.BeginBrwr AND DC.EndBrwr
	AND (
			DC.BorrActive IS NOT NULL
			OR DC.EndrActive IS NOT NULL
		)
	--AND DC.BorrSSN = @SSN --TEST FOR SINGLE BORROWER
ORDER BY
	BorrSSN,
	DC.StatusDate,
	DC.Loan,
	LD_FAT_EFF
;