CREATE PROCEDURE [achrirdf].[GetDefForPeriods]
	@POP [Population] READONLY,
	@LASTRUN VARCHAR(30)
AS

--Empty the DF_PERIOD processing table before starting
DELETE FROM [achrirdf].[DF_PERIOD]

--Empty the LN50 processing table before starting
DELETE FROM [achrirdf].[LN50_Population]

--Empty the LN60 processing table before starting
DELETE FROM [achrirdf].[LN60_Population]

--If no date is provided set the value to a minimum date
IF @LASTRUN IS NULL
	SET @LASTRUN = '1900-01-01'

--PRINT 'LAST REFRESH DATE:  ' + @LASTRUN

--INSERT INTO @SPECIAL_PAY_POP(BF_SSN)
--SELECT DISTINCT
--	LN10.BF_SSN
--FROM
--	UDW..LN10_LON LN10
--	INNER JOIN UDW..LN72_INT_RTE_HST LN72
--		ON LN10.BF_SSN = LN72.BF_SSN
--		AND LN10.LN_SEQ = LN72.LN_SEQ
--	INNER JOIN UDW..LN83_EFT_TO_LON LN83
--		ON LN10.BF_SSN = LN83.BF_SSN
--		AND LN10.LN_SEQ = LN83.LN_SEQ
--WHERE 
--	LN72.LC_STA_LON72 = 'A'
--	AND LN72.LC_INT_RDC_PGM = 'S'
--	AND LN10.LC_STA_LON10 ='R'
--	AND LN10.LA_CUR_PRI > 0.00
--	AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
--	AND LN83.LC_STA_LN83 = 'A'
--	--AND LN72.LF_LST_DTS_LN72 > @LASTRUN

INSERT INTO [achrirdf].[LN50_Population]
SELECT
	LN50.BF_SSN,
	LN50.LN_SEQ,
	LN50.LD_DFR_BEG,
	LN50.LD_DFR_END,
	LN50.LC_STA_LON50,
	LN50.LC_DFR_RSP
FROM
	@POP POP
	INNER JOIN UDW..LN50_BR_DFR_APV LN50
		ON POP.BF_SSN = LN50.BF_SSN
		AND LN50.LF_LST_DTS_LN50 > @LASTRUN

INSERT INTO [achrirdf].[LN60_Population]
SELECT
	LN60.BF_SSN,
	LN60.LN_SEQ,
	LN60.LD_FOR_BEG,
	LN60.LD_FOR_END,
	LN60.LC_STA_LON60,
	LN60.LC_FOR_RSP
FROM
	@POP POP
	INNER JOIN UDW..LN60_BR_FOR_APV LN60
		ON POP.BF_SSN = LN60.BF_SSN
		AND LN60.LF_LST_DTS_LN60 > @LASTRUN

DECLARE @While_Cond BIGINT = 1 --Force do until no rows return
DECLARE @Iter BIGINT = 1

--Begin Get D/F Periods
;WITH First_DF_FOR(BEGIN_DATE, END_DATE, BF_SSN, LN_SEQ, MIN_Begin, MAX_End) --First Contiguous DF Period
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
				LN50.LD_DFR_BEG AS BEGIN_DATE,
				LN50.LD_DFR_END AS END_DATE,
				LN50.BF_SSN AS BF_SSN,
				LN50.LN_SEQ AS LN_SEQ,
				MIN_Begin = LN50.LD_DFR_BEG,
				MAX_End = LN50.LD_DFR_END
			FROM
				[achrirdf].[LN50_Population] LN50
			WHERE
				LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003'

			UNION ALL

			SELECT
				LN60.LD_FOR_BEG AS BEGIN_DATE,
				LN60.LD_FOR_END AS END_DATE,
				LN60.BF_SSN AS BF_SSN,
				LN60.LN_SEQ AS LN_SEQ,
				MIN_Begin = LN60.LD_FOR_BEG,
				MAX_End = LN60.LD_FOR_END
			FROM
				[achrirdf].[LN60_Population] LN60
			WHERE
				LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003'
		) MIN_Date
		GROUP BY
			MIN_Date.BF_SSN,
			MIN_Date.LN_SEQ
	) AS DF_FR
	WHERE
		DF_FR.BEGIN_DATE = DF_FR.MIN_Begin --Starter row
	
	UNION ALL

	SELECT
		DF.BEGIN_DATE,
		LN50.LD_DFR_END AS END_DATE,
		DF.BF_SSN AS BF_SSN,
		DF.LN_SEQ AS LN_SEQ,
		DF.MIN_Begin,
		LN50.LD_DFR_END AS MAX_End
	FROM
		First_DF_FOR DF
		INNER JOIN [achrirdf].[LN50_Population] LN50
			ON DF.BF_SSN = LN50.BF_SSN
			AND DF.LN_SEQ = LN50.LN_SEQ
			AND LN50.LC_STA_LON50 = 'A'
			AND LN50.LC_DFR_RSP != '003'
			AND CONVERT(DATE,DATEADD(DAY, 1,DF.END_DATE)) = CONVERT(DATE,LN50.LD_DFR_BEG)

	UNION ALL

	SELECT
		DF.BEGIN_DATE,
		LN60.LD_FOR_END AS END_DATE,
		DF.BF_SSN AS BF_SSN,
		DF.LN_SEQ AS LN_SEQ,
		DF.MIN_Begin,
		LN60.LD_FOR_END AS MAX_End
	FROM
		First_DF_FOR DF
		INNER JOIN [achrirdf].[LN60_Population] LN60
			ON DF.BF_SSN = LN60.BF_SSN
			AND DF.LN_SEQ = LN60.LN_SEQ
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND CONVERT(DATE,DATEADD(DAY, 1,DF.END_DATE)) = CONVERT(DATE,LN60.LD_FOR_BEG)
)

INSERT INTO [achrirdf].[DF_PERIOD](BF_SSN, LN_SEQ, Begin_Date, End_Date)
SELECT
	BF_SSN,
	LN_SEQ,
	MIN(MIN_Begin) AS Begin_Date,
	MAX(MAX_End) AS End_Date
FROM
	First_DF_FOR
GROUP BY
	BF_SSN,
	LN_SEQ

WHILE @While_Cond != 0 AND @Iter < 32
BEGIN

	;WITH DF_FOR_FINAL(BEGIN_DATE, END_DATE, BF_SSN, LN_SEQ, MIN_Begin, MAX_End)
	AS
	(
		SELECT
			DF_FR.*
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
					LN50.LD_DFR_BEG AS BEGIN_DATE,
					LN50.LD_DFR_END AS END_DATE,
					LN50.BF_SSN AS BF_SSN,
					LN50.LN_SEQ AS LN_SEQ,
					MIN_Begin = LN50.LD_DFR_BEG,
					MAX_End = LN50.LD_DFR_END
				FROM
					[achrirdf].[LN50_Population] LN50
					INNER JOIN 
					(
						SELECT
							BF_SSN,
							LN_SEQ,
							MAX(DFP.End_Date) End_Date
						FROM 
							[achrirdf].[DF_PERIOD] DFP
						GROUP BY
							DFP.BF_SSN,
							DFP.LN_SEQ
					) DF
						ON LN50.BF_SSN = DF.BF_SSN
						AND LN50.LN_SEQ = DF.LN_SEQ
				WHERE
					LN50.LC_STA_LON50 = 'A'
					AND LN50.LC_DFR_RSP != '003'
					AND CONVERT(DATE,DATEADD(DAY, 1, DF.End_Date)) < CONVERT(DATE,LN50.LD_DFR_BEG)

				UNION ALL

				SELECT
					LN60.LD_FOR_BEG AS BEGIN_DATE,
					LN60.LD_FOR_END AS END_DATE,
					LN60.BF_SSN AS BF_SSN,
					LN60.LN_SEQ AS LN_SEQ,
					MIN_Begin = LN60.LD_FOR_BEG,
					MAX_End = LN60.LD_FOR_END
				FROM
					[achrirdf].[LN60_Population] LN60
					INNER JOIN 
					(
						SELECT
							BF_SSN,
							LN_SEQ,
							MAX(DFP.End_Date) End_Date
						FROM 
							[achrirdf].[DF_PERIOD] DFP
						GROUP BY
							DFP.BF_SSN,
							DFP.LN_SEQ
					) DF
						ON LN60.BF_SSN = DF.BF_SSN
						AND LN60.LN_SEQ = DF.LN_SEQ
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND LN60.LC_FOR_RSP != '003'
					AND CONVERT(DATE,DATEADD(DAY, 1,DF.End_Date)) < CONVERT(DATE,LN60.LD_FOR_BEG)
			) MIN_Date
			GROUP BY
				MIN_Date.BF_SSN,
				MIN_Date.LN_SEQ
		) AS DF_FR

		UNION ALL

		SELECT
			DF.BEGIN_DATE,
			LN50.LD_DFR_END AS END_DATE,
			DF.BF_SSN AS BF_SSN,
			DF.LN_SEQ AS LN_SEQ,
			DF.MIN_Begin,
			LN50.LD_DFR_END AS MAX_End
		FROM
			DF_FOR_FINAL DF
			INNER JOIN [achrirdf].[LN50_Population] LN50
				ON DF.BF_SSN = LN50.BF_SSN
				AND DF.LN_SEQ = LN50.LN_SEQ
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003'
				AND CONVERT(DATE,DATEADD(DAY, 1,DF.MAX_End)) = CONVERT(DATE,LN50.LD_DFR_BEG)

		UNION ALL

		SELECT
			DF.BEGIN_DATE,
			LN60.LD_FOR_END AS END_DATE,
			DF.BF_SSN AS BF_SSN,
			DF.LN_SEQ AS LN_SEQ,
			DF.MIN_Begin,
			LN60.LD_FOR_END AS MAX_End
		FROM
			DF_FOR_FINAL DF
			INNER JOIN [achrirdf].[LN60_Population] LN60
				ON DF.BF_SSN = LN60.BF_SSN
				AND DF.LN_SEQ = LN60.LN_SEQ
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003'
				AND CONVERT(DATE,DATEADD(DAY, 1,DF.MAX_End)) = CONVERT(DATE,LN60.LD_FOR_BEG)
	)

	INSERT INTO [achrirdf].[DF_PERIOD](BF_SSN, LN_SEQ, Begin_Date, End_Date)
	SELECT
		DFF.*
	FROM
	(
		SELECT
			DFF.BF_SSN,
			DFF.LN_SEQ,
			MIN(DFF.MIN_Begin) AS Begin_Date,
			MAX(DFF.MAX_End) AS End_Date
		FROM
			DF_FOR_FINAL DFF
		GROUP BY
			DFF.BF_SSN,
			DFF.LN_SEQ
	) DFF
	LEFT JOIN [achrirdf].[DF_PERIOD] DFP
		ON DFF.BF_SSN = DFP.BF_SSN
		AND DFF.LN_SEQ = DFP.LN_SEQ
		AND DFF.Begin_Date = DFP.Begin_Date
		AND DFF.End_Date = DFP.End_Date
	WHERE
		DFP.BF_SSN IS NULL

	SET @While_Cond =  (SELECT @@ROWCOUNT)
	SET @Iter = (SELECT @Iter + 1)
	PRINT(@While_Cond)

END

--Return all values accumulated in [achrirdf].[DF_PERIOD]
SELECT
	BF_SSN,
	LN_SEQ,
	Begin_Date,
	End_Date
FROM
	[achrirdf].[DF_PERIOD]

GO


