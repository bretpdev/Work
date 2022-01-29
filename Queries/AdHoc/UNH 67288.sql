USE UDW;
GO

DROP TABLE IF EXISTS #MULTI;

--get borrowers with more than one open loan with an endorser
SELECT DISTINCT
	M.*
INTO 
	#MULTI
FROM 
	(--get highest count of open loans
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ,
			MAX(OpenLoans) OVER(PARTITION BY BF_SSN) AS OpenLoans
		FROM
			(--all open loans
				SELECT DISTINCT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					ROW_NUMBER () OVER(PARTITION BY LN10.BF_SSN ORDER BY LN10.BF_SSN) AS OpenLoans
				FROM
					LN10_LON LN10
				WHERE
					LN10.LA_CUR_PRI > 0.00
			)POP
		--order by bf_ssn,ln_seq
	)M
	INNER JOIN LN20_EDS LN20 --reduce pop to only those with endorsers
		ON M.BF_SSN = LN20.BF_SSN
WHERE
	M.OpenLoans > 1
;

--SELECT * FROM #MULTI

;WITH FLAGGING AS
(--get endorsers
	SELECT DISTINCT
		M.*,
		--LN20.BF_SSN,
		LN20.LF_EDS,
		LN20.LN_SEQ AS LN20_LN_SEQ,
		IIF(LN20.BF_SSN IS NULL, 1, 0) AS OldOrNoEndorser --flags old endorser loans and loans that don't otherwise have an endorser
	FROM 
		#MULTI M
		LEFT JOIN LN20_EDS LN20
			ON M.BF_SSN = LN20.BF_SSN
			AND M.LN_SEQ = LN20.LN_SEQ
	--WHERE 
	--	M.BF_SSN = '223495906'
	--ORDER BY
	--	M.BF_SSN,
	--	M.LN_SEQ
)
SELECT DISTINCT
	F.*
FROM
	FLAGGING F
	INNER JOIN
	(
		SELECT
			BF_SSN
		FROM
			FLAGGING
		WHERE
			OldOrNoEndorser = 1 --flags old endorser loans and loans that don't otherwise have an endorser
	) SSN
	ON F.BF_SSN = SSN.BF_SSN
;

--SELECT * FROM LN10_LON WHERE BF_SSN = '423336503' AND LA_CUR_PRI > 0
--SELECT * FROM LN20_EDS WHERE BF_SSN = '423336503'
