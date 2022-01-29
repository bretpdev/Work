CREATE PROCEDURE [achrirdf].[GetLN72Periods]
	@POP [Population] READONLY,
	@LASTRUN VARCHAR(30)
AS

--Empty the LN72_PERIOD processing table before starting
DELETE FROM [achrirdf].[LN72_PERIOD]

--Empty the LN72 processing table before starting
DELETE FROM [achrirdf].[LN72_Population]

--GET A COPY OF LN72 Where the account meets the special rate criteria
INSERT INTO	
	[achrirdf].[LN72_Population]
SELECT
	LN72.BF_SSN,
	LN72.LN_SEQ,
	LN72.LD_ITR_EFF_BEG,
	LN72.LD_ITR_EFF_END
FROM
	UDW..LN72_INT_RTE_HST LN72
	INNER JOIN @POP POP
		ON LN72.BF_SSN = POP.BF_SSN
WHERE
	LN72.LC_STA_LON72 = 'A'
	AND LN72.LC_INT_RDC_PGM IN ('S', 'R', 'M', 'P')

--SELECT
--	BF_SSN
--FROM [achrirdf].[LN83_Population]
--GROUP BY
--	LN_SEQ,
--	BF_SSN
--HAVING
--	COUNT(*) > 4


DECLARE @While_Cond BIGINT = 1
DECLARE @Iter BIGINT = 1
;WITH First_LN72(BEGIN_DATE, END_DATE, BF_SSN, LN_SEQ, MIN_Begin, MAX_End) --First Contiguous ACH Period
AS
(
	SELECT
		*
	FROM
	(
		SELECT
			MIN(MIN_Date.BEGIN_DATE) AS BEGIN_DATE,
			MIN(MIN_Date.END_DATE) AS END_DATE,
			MIN_Date.BF_SSN,
			MIN_Date.LN_SEQ,
			MIN(MIN_Date.MIN_Begin) AS MIN_Begin,
			MIN(MIN_Date.MAX_End) AS MAX_End
		FROM
		(
			SELECT
				LN72.LD_ITR_EFF_BEG AS BEGIN_DATE,
				LN72.LD_ITR_EFF_END AS END_DATE, --COALESCING HANDLED AT A HIGHER LEVEL SEE [achrirdf].[LN83_Population]
				LN72.BF_SSN AS BF_SSN,
				LN72.LN_SEQ AS LN_SEQ,
				MIN_Begin = LN72.LD_ITR_EFF_BEG,
				MAX_End = LN72.LD_ITR_EFF_END
			FROM
				[ULS].[achrirdf].LN72_Population LN72
		) MIN_Date
		GROUP BY
			MIN_Date.BF_SSN,
			MIN_Date.LN_SEQ
	) AS SPEC
	WHERE
		SPEC.BEGIN_DATE = SPEC.MIN_Begin --Starter row
	
	UNION ALL

	SELECT
		SPEC.BEGIN_DATE, --TODO Figure out ACH Begin Date
		LN72.LD_ITR_EFF_END AS END_DATE,
		SPEC.BF_SSN AS BF_SSN,
		SPEC.LN_SEQ AS LN_SEQ,
		SPEC.MIN_Begin,
		LN72.LD_ITR_EFF_END AS MAX_End
	FROM
		First_LN72 SPEC
		INNER JOIN [achrirdf].[LN72_Population] LN72
			ON SPEC.BF_SSN = LN72.BF_SSN
			AND SPEC.LN_SEQ = LN72.LN_SEQ
			AND 
			(
				CONVERT(DATE,DATEADD(DAY, 1,SPEC.END_DATE)) = CONVERT(DATE,LN72.LD_ITR_EFF_BEG) --The previous end date is one day before the next begin date
				--OR CONVERT(DATE,ACH.END_DATE) = CONVERT(DATE,LN83.LD_EFT_EFF_BEG)
			)
)

INSERT INTO [achrirdf].[LN72_PERIOD](BF_SSN, LN_SEQ, Begin_Date, End_Date)
SELECT
	BF_SSN,
	LN_SEQ,
	MIN(MIN_Begin) AS Begin_Date,
	MAX(MAX_End) AS End_Date
FROM
	First_LN72
GROUP BY
	BF_SSN,
	LN_SEQ

--SELECT 
--	*
--FROM
--	[achrirdf].[ACH_PERIOD]

WHILE @While_Cond != 0 AND @Iter < 32
BEGIN

	;WITH LN72_FINAL(BEGIN_DATE, END_DATE, BF_SSN, LN_SEQ, MIN_Begin, MAX_End)
	AS
	(
		SELECT
			LN72.*
		FROM
		(
			SELECT
				MIN(MIN_Date.BEGIN_DATE) AS BEGIN_DATE,
				MIN(MIN_Date.END_DATE) AS END_DATE,
				MIN_Date.BF_SSN,
				MIN_Date.LN_SEQ,
				MIN(MIN_Date.MIN_Begin) AS MIN_Begin,
				MIN(MIN_Date.MAX_End) AS MAX_End
			FROM
			(
				SELECT
					LN72.LD_ITR_EFF_BEG AS BEGIN_DATE,
					LN72.LD_ITR_EFF_END AS END_DATE,
					LN72.BF_SSN AS BF_SSN,
					LN72.LN_SEQ AS LN_SEQ,
					MIN_Begin = LN72.LD_ITR_EFF_BEG,
					MAX_End = LN72.LD_ITR_EFF_END
				FROM
					[achrirdf].[LN72_Population] LN72
					INNER JOIN 
					(
						SELECT
							BF_SSN,
							LN_SEQ,
							MAX(LN72P.End_Date) End_Date
						FROM 
							[achrirdf].[LN72_PERIOD] LN72P
						GROUP BY
							LN72P.BF_SSN,
							LN72P.LN_SEQ
					) SPEC
						ON LN72.BF_SSN = SPEC.BF_SSN
						AND LN72.LN_SEQ = SPEC.LN_SEQ
				WHERE
					CONVERT(DATE,DATEADD(DAY, 1, SPEC.End_Date)) < CONVERT(DATE,LN72.LD_ITR_EFF_BEG)
			) MIN_Date
			GROUP BY
				MIN_Date.BF_SSN,
				MIN_Date.LN_SEQ
		) AS LN72

		UNION ALL

		SELECT
			LN72F.BEGIN_DATE,
			LN72.LD_ITR_EFF_END AS END_DATE,
			LN72F.BF_SSN AS BF_SSN,
			LN72F.LN_SEQ AS LN_SEQ,
			LN72F.MIN_Begin,
			LN72.LD_ITR_EFF_END AS MAX_End
		FROM
			LN72_FINAL LN72F
			INNER JOIN [achrirdf].[LN72_Population] LN72
				ON LN72F.BF_SSN = LN72.BF_SSN
				AND LN72F.LN_SEQ = LN72.LN_SEQ
				AND 
				(
					CONVERT(DATE,DATEADD(DAY, 1,LN72F.MAX_End)) = CONVERT(DATE,LN72.LD_ITR_EFF_BEG)
					--OR CONVERT(DATE, ACHF.MAX_End) = CONVERT(DATE,LN83.LD_EFT_EFF_BEG)
				)
	)

	INSERT INTO [achrirdf].[LN72_PERIOD](BF_SSN, LN_SEQ, Begin_Date, End_Date)
	SELECT
		LN72F.*
	FROM
	(
		SELECT
			LN72F.BF_SSN,
			LN72F.LN_SEQ,
			MIN(LN72F.MIN_Begin) AS Begin_Date,
			MAX(LN72F.MAX_End) AS End_Date
		FROM
			LN72_FINAL LN72F
		GROUP BY
			LN72F.BF_SSN,
			LN72F.LN_SEQ
	) LN72F
	LEFT JOIN [achrirdf].[LN72_PERIOD] LN72P
		ON LN72F.BF_SSN = LN72P.BF_SSN
		AND LN72F.LN_SEQ = LN72P.LN_SEQ
		AND LN72F.Begin_Date = LN72P.Begin_Date
		AND LN72F.End_Date = LN72P.End_Date
	WHERE
		LN72P.BF_SSN IS NULL

	SET @While_Cond =  (SELECT @@ROWCOUNT)
	SET @Iter = (SELECT @Iter + 1)
	PRINT(@While_Cond)

END

--Return all values accumulated in @DF_PERIOD
SELECT DISTINCT
	BF_SSN,
	LN_SEQ,
	Begin_Date,
	End_Date
FROM
	[achrirdf].[LN72_PERIOD]

