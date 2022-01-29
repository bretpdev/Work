USE UDW
GO

IF OBJECT_ID(N'tempdb..#ACH_EBILL_DATA') IS NOT NULL
	BEGIN
		DROP TABLE tempdb..#ACH_EBILL_DATA
	END
GO
SELECT DISTINCT
	LN10.BF_SSN [Ssn],
	PD10.DF_SPE_ACC_ID [Account],
	PD10.DM_PRS_1 [FirstName],
	PD10.DM_PRS_LST [LastName],
	LN10.RepayStart,
	CASE
		WHEN 
			LN10.LC_LON_SND_CHC = 'Y' -- on ebill
			OR
			LN10.RepayStart >= '2009-07-01'
		THEN 1
		WHEN 
			LN10.LC_LON_SND_CHC != 'Y' -- not on ebill
			AND
			PH05.DI_CNC_EBL_OPI != 'Y' -- not on ACH
		THEN 2
		ELSE 0 -- neither population
	END	[QueryPopulation]
INTO 
	#ACH_EBILL_DATA
FROM
	PD10_PRS_NME PD10
	INNER JOIN
	( -- determine MaxRepayStart
		SELECT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LC_LON_SND_CHC,
			CASE 
				WHEN LN10.IC_LON_PGM IN ('STFFRD', 'UNSTFD') THEN DATEADD(DAY, 1, LN10.LD_END_GRC_PRD)
				WHEN LN10.IC_LON_PGM IN ('CNSLDN', 'SUBCNS', 'SUBSPC', 'UNCNS', 'UNSPC') THEN LN10.LD_LON_1_DSB
				WHEN LN10.IC_LON_PGM IN ('PLUS', 'PLUSGB', 'SLS') THEN LN15.max_LD_DSB
			END [RepayStart]
		FROM
			LN10_LON LN10
			LEFT JOIN 
			(
				SELECT
					LN15.BF_SSN,
					LN15.LN_SEQ,
					MAX(LN15.LD_DSB) [max_LD_DSB]
				FROM
					LN15_DSB LN15
				WHERE
					LN15.LD_DSB_CAN IS NULL
				GROUP BY
					LN15.BF_SSN,
					LN15.LN_SEQ
			) LN15 ON LN15.BF_SSN = LN10.BF_SSN AND LN15.LN_SEQ = LN10.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND
			LN10.LA_CUR_PRI > 0.00
	) LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN OPENQUERY
	( -- LN83 is not currently a local table
		DUSTER,
		'
			SELECT
				LN83.BF_SSN,
				LN83.LN_SEQ,
				LN83.BN_EFT_SEQ
			FROM
				OLWHRM1.LN83_EFT_TO_LON LN83
			WHERE
				LN83.LC_STA_LN83 = ''A''
		'
	) LN83 ON LN83.BF_SSN = LN10.BF_SSN AND LN83.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN BR30_BR_EFT BR30 ON BR30.BF_SSN = LN10.BF_SSN AND BR30.BN_EFT_SEQ = LN83.BN_EFT_SEQ
	LEFT JOIN PH05_CNC_EML PH05 ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
WHERE
	BR30.BC_EFT_STA = 'A'
ORDER BY
	QueryPopulation

-- query 1
SELECT
	[Ssn],
	[Account],
	[FirstName],
	[LastName],
	MAX(RepayStart) [MaxRepayStart]
FROM
	#ACH_EBILL_DATA AED
WHERE
	AED.QueryPopulation = 1
GROUP BY
	[Ssn],
	[Account],
	[FirstName],
	[LastName]

-- query 2
SELECT
	[Ssn],
	[Account],
	[FirstName],
	[LastName],
	MAX(RepayStart) [MaxRepayStart]
FROM
	#ACH_EBILL_DATA AED
WHERE
	AED.QueryPopulation = 2
GROUP BY
	[Ssn],
	[Account],
	[FirstName],
	[LastName]