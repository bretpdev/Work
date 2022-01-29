DECLARE @LoanNumbers TABLE
(
	LoanNumber TINYINT
)

DECLARE @BorrowerLoans TABLE
(
	BF_SSN CHAR(9),
	LoanNumber TINYINT
)

INSERT INTO 
	@LoanNumbers
VALUES
	(1),
	(2),
	(3),
	(4),
	(5),
	(6),
	(7),
	(8)

INSERT INTO
	@BorrowerLoans
SELECT
	LN10.BF_SSN,
	LN.LoanNumber
FROM 
	@LoanNumbers LN
	CROSS JOIN 
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			LN10_LON LN10
		WHERE
			LN10.BF_SSN IN ('001029588', '001044731', '001081976')
	) LN10


SELECT
	OBL.BF_SSN,
	LoanData = 	STUFF
	(
		( 
			SELECT
				''',''' + LA_CUR_PRI + ''',''' + LA_LON_AMT_GTR
			FROM    
			( 
				SELECT
					BL.BF_SSN,
					BL.LoanNumber,
					ISNULL(CAST(LN10.LA_CUR_PRI AS VARCHAR(10)), '') [LA_CUR_PRI],
					ISNULL(CAST(LN10.LA_LON_AMT_GTR AS VARCHAR(10)), '') [LA_LON_AMT_GTR]
				FROM 
					@BorrowerLoans BL
					LEFT JOIN LN10_LON LN10 ON LN10.BF_SSN = BL.BF_SSN and LN10.LN_SEQ = BL.LoanNumber
				WHERE
					OBL.BF_SSN = BL.BF_SSN
			) x
			ORDER BY
				BF_SSN
			FOR XML PATH(''), TYPE
		).value('.','VARCHAR(max)'),
		1,2,''
	  )
FROM
	( 
		SELECT DISTINCT
			OBL.BF_SSN
		FROM
			@BorrowerLoans OBL
	) OBL
ORDER BY
	OBL.BF_SSN
