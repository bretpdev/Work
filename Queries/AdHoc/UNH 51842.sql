USE UDW
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
/*

MAIN INPUT:
	All open, released loans with an ebill indicator of Y.
	LN10.LC_STA_LON10 = R
	LN10.LA_CUR_PRI > 0.00
	LN10. LC_LON_SND_CHC = Y 

	FORMAT (no header)
	01 	02      'UT'
	03 	09     	LN10.BF_SSN
	12 	01		SPACE
	13 	04     	LN10.LN_SEQ (with leading zeros)
	17 	06     	LN10.IC_LON_PGM
	23 	06		‘000000’
	29 	01      '.'
	30 	02      '00'
	32 	01     	'P'
	33 	60     	SPACE
*/

/*
FILE 1 TITLE:  On Ebill and Ecorr-Bill is Y
	Additional Input: 
	PH05.DI_CNC_EBL_OPI = Y
*/
SELECT
	'UT' + PD10.DF_PRS_ID + ' ' + RIGHT('0000' + CAST(LN10.LN_SEQ AS VARCHAR(4)), 4) + LN10.IC_LON_PGM + '000000.00P' + SPACE(60)
FROM
	PD10_PRS_NME PD10
	INNER JOIN LN10_LON LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN PH05_CNC_EML PH05 ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND
	LN10.LA_CUR_PRI > 0.00
	AND
	LN10.LC_LON_SND_CHC = 'Y'
	AND
	PH05.DI_CNC_EBL_OPI = 'Y'


/*
FILE 2 TITLE:  On Ebill and Ecorr-Bill is not Y
*/
SELECT DISTINCT
	LN10.BF_SSN [Ssn],
	PD10.DF_SPE_ACC_ID [Account],
	PD10.DM_PRS_1 [FirstName],
	PD10.DM_PRS_LST [LastName],
	LN10.RepayStart,
	CASE 
		WHEN LN83.LC_STA_LN83 = 'A' and BR30.BC_EFT_STA = 'A' THEN 'Y'
		ELSE 'N'
	END [ACH]
INTO
-- DROP TABLE tempdb..#ACH_EBILL_DATA
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
			AND
			LN10.LC_LON_SND_CHC = 'Y'
	) LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN OPENQUERY
	( -- LN83 is not currently a local table
		DUSTER,
		'
			SELECT
				LN83.BF_SSN,
				LN83.LN_SEQ,
				LN83.BN_EFT_SEQ,
				LN83.LC_STA_LN83
			FROM
				OLWHRM1.LN83_EFT_TO_LON LN83
			--WHERE
			--	LN83.LC_STA_LN83 = ''A''
		'
	) LN83 ON LN83.BF_SSN = LN10.BF_SSN AND LN83.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN BR30_BR_EFT BR30 ON BR30.BF_SSN = LN10.BF_SSN AND BR30.BN_EFT_SEQ = LN83.BN_EFT_SEQ
	LEFT JOIN PH05_CNC_EML PH05 ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
WHERE
	PH05.DI_CNC_EBL_OPI != 'Y'

SELECT
	[Ssn],
	[Account],
	[FirstName],
	[LastName],
	MAX(RepayStart) [MaxRepayStart],
	ACH
FROM
	#ACH_EBILL_DATA AED
GROUP BY
	[Ssn],
	[Account],
	[FirstName],
	[LastName],
	[ACH]

/*
Issue:
Query Requirements: 

	MAIN INPUT:  
	All open, released loans with an Ebill Indicator of Y.

	FILE 1 ADDITIONAL INPUT:	
	And has an Ecorr-Bill Indicator of Y

	FILE 2 ADDITIONAL INPUT:	
	And has an Ecorr-Bill Indicator that is not Y

	OUTPUT: 	
	File format needed for Updating E-bill Requests script


File 1 will be fed into the Updating E-bill Requests script to remove their ebill indicator, thus allowing the billing sas to begin picking up borrowers and send their bills via ecorr.  

File 2 will need to be processed manually to remove the ebill indicator and update the ecorr-bill indicator to Y. 
*/