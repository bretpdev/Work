CREATE PROCEDURE [achrirdf].[AddNewWorkToQueue]
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


--Create temp table that will be loaded into the processing table
IF OBJECT_ID('tempdb..#DusterData') IS NOT NULL
    DROP TABLE #DusterData

CREATE TABLE #DusterData
(
	Report VARCHAR(3),
	Ssn CHAR(9),
	AccountNumber CHAR(10),
	LoanSequence VARCHAR(3),
	OwnerCode VARCHAR(8),
	DefermentOrForbearanceOriginalBeginDate DATE,
	DefermentOrForbearanceOriginalEndDate DATE,
	DefermentOrForbearanceBeginDate DATE,
	DefermentOrForbearanceEndDate DATE,
	UpdatedAt DATETIME,
	VariableRate BIT,
	HasPartialReducedRate BIT NULL
)

--Create the table that will house the contiguous deferment/forbearance periods
DECLARE @DF_PERIOD TABLE
(
	BF_SSN VARCHAR(9),
	LN_SEQ SMALLINT,
	Begin_Date DATE,
	End_Date DATE
)

DECLARE @D_PERIOD TABLE
(
	BF_SSN VARCHAR(9),
	LN_SEQ SMALLINT,
	Begin_Date DATE,
	End_Date DATE
)

DECLARE @F_PERIOD TABLE
(
	BF_SSN VARCHAR(9),
	LN_SEQ SMALLINT,
	Begin_Date DATE,
	End_Date DATE
)


--Create the table that will house the contiguous ach periods
DECLARE @ACH_PERIOD TABLE
(
	BF_SSN VARCHAR(9),
	LN_SEQ SMALLINT,
	Begin_Date DATE,
	End_Date DATE
)

--Create the special interest rate period table
DECLARE @LN72_PERIOD TABLE
(
	BF_SSN VARCHAR(9),
	LN_SEQ SMALLINT,
	Begin_Date DATE,
	End_Date DATE
)

--Population to limit the deferment/forbearance periods and ach periods by
DECLARE @SPECIAL_PAY_POP [achrirdf].[Population]

--Date to filter LN50/LN60 records by for deferment/forbearance periods
DECLARE @LASTRUN VARCHAR(30) = 
--Test filter
NULL
--Production filter
--(
--	SELECT 
--		ISNULL(CONVERT(DATE,MAX(UpdatedAt),101), '2017-02-04') --USE FOR LIVE
--		--ISNULL(CONVERT(DATE,MAX(UpdatedAt),101), '2016-9-28')  --USE FOR TEST
--	FROM 
--		ULS.[achrirdf].[ProcessQueue]
--)

INSERT INTO @SPECIAL_PAY_POP(BF_SSN)
--Special Payment Population to look at by query requirements
SELECT DISTINCT
	LN10.BF_SSN
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LN72_INT_RTE_HST LN72
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
	INNER JOIN UDW..LN83_EFT_TO_LON LN83
		ON LN10.BF_SSN = LN83.BF_SSN
		AND LN10.LN_SEQ = LN83.LN_SEQ
WHERE 
	LN72.LC_STA_LON72 = 'A'
	--AND LN72.LC_INT_RDC_PGM = 'S'
	AND LN10.LC_STA_LON10 ='R'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
	AND LN83.LC_STA_LN83 = 'A'
	AND LN72.LF_LST_DTS_LN72 > ISNULL(@LASTRUN, '1900-1-1')

--Uncomment to manually add accounts to test them
--INSERT INTO @SPECIAL_PAY_POP(BF_SSN)
--VALUES ('')

--Get Deferment/Forbearance Periods
INSERT INTO
	@DF_PERIOD
EXEC
	[achrirdf].GetDefForPeriods @SPECIAL_PAY_POP, @LASTRUN;

INSERT INTO
	@D_PERIOD
EXEC
	[achrirdf].GetDefPeriods @SPECIAL_PAY_POP, @LASTRUN;

INSERT INTO
	@F_PERIOD
EXEC
	[achrirdf].GetForPeriods @SPECIAL_PAY_POP, @LASTRUN;

INSERT INTO
	@ACH_PERIOD
EXEC
	[achrirdf].GetACHPeriods @SPECIAL_PAY_POP, @LASTRUN; --Lastrun is not currently used

INSERT INTO
	@LN72_PERIOD
EXEC
	[achrirdf].GetLN72Periods @SPECIAL_PAY_POP, @LASTRUN;
--SELECT
--	*
--FROM
--	@DF_PERIOD

--SELECT
--	*
--FROM
--	@ACH_PERIOD

--R2 Manual M Rate Review
INSERT INTO
	#DusterData
SELECT DISTINCT 
	R2.Report,
	R2.Ssn,
	R2.AccountNumber,
	R2.LoanSequence,
	R2.OwnerCode,
	R2.DefermentOrForbearanceOriginalBeginDate,
	R2.DefermentOrForbearanceOriginalEndDate,
	R2.DefermentOrForbearanceBeginDate,
	R2.DefermentOrForbearanceEndDate,
	R2.UpdatedAt,
	R2.VariableRate,
	R2.HasPartialReducedRate
FROM 	
	(/* R2D – Entering Deferment */
		SELECT DISTINCT
			'R2'				    AS Report
			,LN10.BF_SSN			AS Ssn
			,PD10.DF_SPE_ACC_ID		AS AccountNumber
			,LN10.LN_SEQ			AS LoanSequence
			,LN10.LF_LON_CUR_OWN	AS OwnerCode
			,NULL					AS DefermentOrForbearanceOriginalBeginDate
			,NULL					AS DefermentOrForbearanceOriginalEndDate
			,CASE WHEN DF.Begin_Date >= ACH.Begin_Date THEN DF.Begin_Date
			WHEN DF.Begin_Date < ACH.Begin_Date THEN ACH.Begin_Date			
			END AS DefermentOrForbearanceBeginDate
			,CASE WHEN DF.End_Date >= ACH.End_Date THEN ACH.End_Date
			WHEN DF.End_Date < ACH.End_Date THEN DF.End_Date
		    END AS DefermentOrForbearanceEndDate
			,NULL					AS UpdatedAt
			,CASE WHEN LN72_VAR.BF_SSN IS NULL THEN 0 ELSE 1  END AS VariableRate
			,CASE WHEN CVRD.LC_INT_RDC_PGM IN ('M') THEN 1 ELSE 0 END AS HasPartialReducedRate
			,CVRD.LN72_Begin AS LN72_Begin
			,CVRD.LN72_End AS LN72_End
		FROM
		UDW..LN10_LON LN10
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN @ACH_PERIOD ACH
			ON LN10.BF_SSN = ACH.BF_SSN
			AND LN10.LN_SEQ = ACH.LN_SEQ
		INNER JOIN @DF_PERIOD DF
			ON ACH.BF_SSN = DF.BF_SSN
			AND ACH.LN_SEQ = DF.LN_SEQ
		--Needed to determine fixed vs variable interest rate loans, should only ever return 1 result
		LEFT JOIN
		(
			SELECT DISTINCT
				BF_SSN,
				LN_SEQ
			FROM
				UDW..LN72_INT_RTE_HST LN72
			WHERE
				LN72.LC_ITR_TYP != ''
				AND LN72.LC_ITR_TYP IS NOT NULL
				AND LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
		) LN72_VAR
			ON LN10.BF_SSN = LN72_VAR.BF_SSN
			AND LN10.LN_SEQ = LN72_VAR.LN_SEQ
		--Used here to determine partially covered M rate DF/ACH periods for manual review
		LEFT JOIN
		(
			SELECT
				LN10.*,
				DF.Begin_Date AS  DF_Begin,
				DF.End_Date AS DF_End,
				ACH.Begin_Date AS ACH_Begin,
				ACH.End_Date AS ACH_End,
				LN72.Begin_Date AS LN72_Begin,
				LN72.End_Date AS LN72_End,
				PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
				LN72_M.LC_INT_RDC_PGM
			FROM
				UDW..LN10_LON LN10
				INNER JOIN @DF_PERIOD DF
					ON LN10.BF_SSN = DF.BF_SSN
					AND LN10.LN_SEQ = DF.LN_SEQ
				INNER JOIN @ACH_PERIOD ACH
					ON LN10.BF_SSN = ACH.BF_SSN
					AND LN10.LN_SEQ = ACH.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN @LN72_PERIOD LN72
					ON LN10.BF_SSN = LN72.BF_SSN
					AND LN10.LN_SEQ = LN72.LN_SEQ
				LEFT JOIN UDW..LN72_INT_RTE_HST LN72_M
					ON LN72.BF_SSN = LN72_M.BF_SSN
					AND LN72.LN_SEQ = LN72_M.LN_SEQ
					AND LN72_M.LD_ITR_EFF_BEG BETWEEN LN72.Begin_Date AND LN72.End_Date
					AND LN72_M.LD_ITR_EFF_END BETWEEN LN72.Begin_Date AND LN72.End_Date
					AND LN72_M.LC_INT_RDC_PGM = 'M'
					AND LN72_M.LC_STA_LON72 = 'A'
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN72_M.BF_SSN IS NOT NULL
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND 
				(
					(
						LN72.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
						OR LN72.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					)
					AND
					(
						LN72.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date 
						OR LN72.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
					)
				)
				AND
				(
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
			) CVRD	
				ON DF.Begin_Date = CVRD.DF_Begin
				AND DF.End_Date = CVRD.DF_End
				AND ACH.Begin_Date = CVRD.ACH_Begin
				AND ACH.End_Date = CVRD.ACH_End
				AND DF.BF_SSN = CVRD.BF_SSN
				AND DF.LN_SEQ = CVRD.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN10.*,
				DF.Begin_Date AS  DF_Begin,
				DF.End_Date AS DF_End,
				ACH.Begin_Date AS ACH_Begin,
				ACH.End_Date AS ACH_End,
				LN72.Begin_Date AS LN72_Begin,
				LN72.End_Date AS LN72_End,
				PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
				LN72_M.LC_INT_RDC_PGM
			FROM
				UDW..LN10_LON LN10
				INNER JOIN @DF_PERIOD DF
					ON LN10.BF_SSN = DF.BF_SSN
					AND LN10.LN_SEQ = DF.LN_SEQ
				INNER JOIN @ACH_PERIOD ACH
					ON LN10.BF_SSN = ACH.BF_SSN
					AND LN10.LN_SEQ = ACH.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN @LN72_PERIOD LN72
					ON LN10.BF_SSN = LN72.BF_SSN
					AND LN10.LN_SEQ = LN72.LN_SEQ
				LEFT JOIN UDW..LN72_INT_RTE_HST LN72_M
					ON LN72.BF_SSN = LN72_M.BF_SSN
					AND LN72.LN_SEQ = LN72_M.LN_SEQ
					AND LN72_M.LD_ITR_EFF_BEG BETWEEN LN72.Begin_Date AND LN72.End_Date
					AND LN72_M.LD_ITR_EFF_END BETWEEN LN72.Begin_Date AND LN72.End_Date
					AND LN72_M.LC_INT_RDC_PGM = 'M'
					AND LN72_M.LC_STA_LON72 = 'A'
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN72_M.BF_SSN IS NOT NULL
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND
				(
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
				AND
				(
					(
						(
							DF.Begin_Date BETWEEN LN72.Begin_Date AND LN72.End_Date
							OR ACH.Begin_Date BETWEEN LN72.Begin_Date AND LN72.End_Date
						)
						AND 
						(
							ACH.End_Date BETWEEN LN72.Begin_Date AND LN72.End_Date
							OR DF.End_Date BETWEEN LN72.Begin_Date AND LN72.End_Date
						)
					)
					--Special Case where the M rate is on a variable interest rate loan that ends at the end of the current fiscal year
					OR 
					(
						LN72_M.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						AND
						(
							DF.Begin_Date BETWEEN LN72.Begin_Date AND LN72.End_Date
							OR ACH.Begin_Date BETWEEN LN72.Begin_Date AND LN72.End_Date
						)
						AND
						(
							DATEPART(MONTH, LN72.End_Date) = 6
							AND DATEPART(DAY, LN72.End_Date) = 30
							AND
							(
								(
									CAST(GETDATE() AS DATE) > CAST(DATEADD(DAY, 29, DATEADD(MONTH, 5, DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()), 0))) AS DATE) --June 30th this year
									AND LN72.End_Date = CAST(DATEADD(DAY, 29, DATEADD(MONTH, 5, DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()) + 1, 0))) AS DATE) --June 30th next year
								)
								OR
								(
									CAST(GETDATE() AS DATE) <= CAST(DATEADD(DAY, 29, DATEADD(MONTH, 5, DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()), 0))) AS DATE) --June 30th this year
									AND LN72.End_Date = CAST(DATEADD(DAY, 29, DATEADD(MONTH, 5, DATEADD(YEAR, DATEDIFF(YEAR, 0, GETDATE()), 0))) AS DATE) --June 30th next year
								)
							)
						)
					)
				)
			) SPANNED
				ON DF.Begin_Date = SPANNED.DF_Begin
				AND DF.End_Date = SPANNED.DF_End
				AND ACH.Begin_Date = SPANNED.ACH_Begin
				AND ACH.End_Date = SPANNED.ACH_End
				AND DF.BF_SSN = SPANNED.BF_SSN
				AND DF.LN_SEQ = SPANNED.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND CVRD.BF_SSN IS NOT NULL
				AND SPANNED.BF_SSN IS NULL
				AND
				( 
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
	) AS R2
WHERE 
	HasPartialReducedRate = 1
	AND 
	(
		LN72_Begin != DefermentOrForbearanceBeginDate
		OR LN72_End != DefermentOrForbearanceEndDate
	)

--R2 NON VARIABLE RATE
INSERT INTO
	#DusterData
SELECT DISTINCT 
	R2.Report,
	R2.Ssn,
	R2.AccountNumber,
	R2.LoanSequence,
	R2.OwnerCode,
	R2.DefermentOrForbearanceOriginalBeginDate,
	R2.DefermentOrForbearanceOriginalEndDate,
	R2.DefermentOrForbearanceBeginDate,
	R2.DefermentOrForbearanceEndDate,
	R2.UpdatedAt,
	R2.VariableRate,
	R2.HasPartialReducedRate
FROM 	
	(/* R2D – Entering Deferment */
		SELECT DISTINCT
			'R2'				    AS Report
			,LN10.BF_SSN			AS Ssn
			,PD10.DF_SPE_ACC_ID		AS AccountNumber
			,LN10.LN_SEQ			AS LoanSequence
			,LN10.LF_LON_CUR_OWN	AS OwnerCode
			,NULL					AS DefermentOrForbearanceOriginalBeginDate
			,NULL					AS DefermentOrForbearanceOriginalEndDate
			,CASE WHEN DF.Begin_Date >= ACH.Begin_Date THEN DF.Begin_Date
			WHEN DF.Begin_Date < ACH.Begin_Date THEN ACH.Begin_Date			
			END AS DefermentOrForbearanceBeginDate
			,CASE WHEN DF.End_Date >= ACH.End_Date THEN ACH.End_Date
			WHEN DF.End_Date < ACH.End_Date THEN DF.End_Date
		    END AS DefermentOrForbearanceEndDate
			,NULL					AS UpdatedAt
			,CASE WHEN LN72_VAR.BF_SSN IS NULL THEN 0 ELSE 1  END AS VariableRate
			,CASE WHEN CVRD.LC_INT_RDC_PGM IN ('R','P') THEN 1 ELSE 0 END AS HasPartialReducedRate
			,CVRD.LN72_Begin AS LN72_Begin
			,CVRD.LN72_End AS LN72_End
		FROM
		UDW..LN10_LON LN10
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN @ACH_PERIOD ACH
			ON LN10.BF_SSN = ACH.BF_SSN
			AND LN10.LN_SEQ = ACH.LN_SEQ
		INNER JOIN @DF_PERIOD DF
			ON ACH.BF_SSN = DF.BF_SSN
			AND ACH.LN_SEQ = DF.LN_SEQ
		--Needed to determine fixed vs variable interest rate loans, should only ever return 1 result
		LEFT JOIN
		(
			SELECT DISTINCT
				BF_SSN,
				LN_SEQ
			FROM
				UDW..LN72_INT_RTE_HST LN72
			WHERE
				LN72.LC_ITR_TYP != ''
				AND LN72.LC_ITR_TYP IS NOT NULL
				AND LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
		) LN72_VAR
			ON LN10.BF_SSN = LN72_VAR.BF_SSN
			AND LN10.LN_SEQ = LN72_VAR.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN10.*,
				DF.Begin_Date AS  DF_Begin,
				DF.End_Date AS DF_End,
				ACH.Begin_Date AS ACH_Begin,
				ACH.End_Date AS ACH_End,
				LN72.LD_ITR_EFF_BEG AS LN72_Begin,
				LN72.LD_ITR_EFF_END AS LN72_End,
				PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
				LN72.LC_INT_RDC_PGM
			FROM
				UDW..LN10_LON LN10
				INNER JOIN @DF_PERIOD DF
					ON LN10.BF_SSN = DF.BF_SSN
					AND LN10.LN_SEQ = DF.LN_SEQ
				INNER JOIN @ACH_PERIOD ACH
					ON LN10.BF_SSN = ACH.BF_SSN
					AND LN10.LN_SEQ = ACH.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..LN72_INT_RTE_HST LN72
					ON LN72.BF_SSN = DF.BF_SSN
					AND LN72.LN_SEQ = DF.LN_SEQ
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND LN72.LC_INT_RDC_PGM IN ('S', 'R', 'M', 'P')
				AND LN72.LC_STA_LON72 = 'A'
				AND 
				(
					(
						LN72.LD_ITR_EFF_BEG BETWEEN ACH.Begin_Date AND ACH.End_Date
						OR LN72.LD_ITR_EFF_END BETWEEN ACH.Begin_Date AND ACH.End_Date
					)
					AND
					(
						LN72.LD_ITR_EFF_BEG BETWEEN DF.Begin_Date AND DF.End_Date 
						OR LN72.LD_ITR_EFF_END BETWEEN DF.Begin_Date AND DF.End_Date
					)
				)
				AND
				(
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
				--Only include fixed rate loans
				AND LN72.LC_ITR_TYP NOT IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
			) CVRD	
				ON DF.Begin_Date = CVRD.DF_Begin
				AND DF.End_Date = CVRD.DF_End
				AND ACH.Begin_Date = CVRD.ACH_Begin
				AND ACH.End_Date = CVRD.ACH_End
				AND DF.BF_SSN = CVRD.BF_SSN
				AND DF.LN_SEQ = CVRD.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = 'R'
				--AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND 
				(
					CVRD.BF_SSN IS NULL
					OR CVRD.LC_INT_RDC_PGM IN ('R','P')
				)
				AND
				( 
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
	) AS R2
	LEFT JOIN #DusterData DD
		ON DD.AccountNumber = R2.AccountNumber
		AND DD.LoanSequence = R2.LoanSequence
		AND DD.DefermentOrForbearanceBeginDate = R2.DefermentOrForbearanceBeginDate
		AND DD.DefermentOrForbearanceEndDate = R2.DefermentOrForbearanceEndDate
		AND DD.VariableRate = R2.VariableRate
WHERE 
	R2.VariableRate = 0
	AND
	(
		R2.HasPartialReducedRate = 0
		OR
		(
			R2.HasPartialReducedRate = 1
			AND 
			(
				LN72_Begin != R2.DefermentOrForbearanceBeginDate
				OR LN72_End != R2.DefermentOrForbearanceEndDate
			)
		)
	)
	AND DD.AccountNumber IS NULL

--R2 PART 2 VARIABLE RATE DEFERMENTS
INSERT INTO
	#DusterData
SELECT DISTINCT 
	R2.Report,
	R2.Ssn,
	R2.AccountNumber,
	R2.LoanSequence,
	R2.OwnerCode,
	R2.DefermentOrForbearanceOriginalBeginDate,
	R2.DefermentOrForbearanceOriginalEndDate,
	R2.DefermentOrForbearanceBeginDate,
	R2.DefermentOrForbearanceEndDate,
	R2.UpdatedAt,
	R2.VariableRate,
	R2.HasPartialReducedRate
FROM 	
	(/* R2D – Entering Deferment */
		SELECT DISTINCT
			'R2'				    AS Report
			,LN10.BF_SSN			AS Ssn
			,PD10.DF_SPE_ACC_ID		AS AccountNumber
			,LN10.LN_SEQ			AS LoanSequence
			,LN10.LF_LON_CUR_OWN	AS OwnerCode
			,NULL					AS DefermentOrForbearanceOriginalBeginDate
			,NULL					AS DefermentOrForbearanceOriginalEndDate
			,CASE WHEN DF.Begin_Date >= ACH.Begin_Date THEN DF.Begin_Date
			WHEN DF.Begin_Date < ACH.Begin_Date THEN ACH.Begin_Date			
			END AS DefermentOrForbearanceBeginDate
			,CASE WHEN DF.End_Date >= ACH.End_Date THEN ACH.End_Date
			WHEN DF.End_Date < ACH.End_Date THEN DF.End_Date
		    END AS DefermentOrForbearanceEndDate
			,NULL					AS UpdatedAt
			,CASE WHEN LN72_VAR.BF_SSN IS NULL THEN 0 ELSE 1  END AS VariableRate
			,CASE WHEN CVRD_D.LC_INT_RDC_PGM IN ('R','P') THEN 1 ELSE 0 END AS HasPartialReducedRate
			,CVRD_D.LN72_Begin AS LN72_Begin
			,CVRD_D.LN72_End AS LN72_End
		FROM
		UDW..LN10_LON LN10
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN @ACH_PERIOD ACH
			ON LN10.BF_SSN = ACH.BF_SSN
			AND LN10.LN_SEQ = ACH.LN_SEQ
		INNER JOIN @D_PERIOD DF
			ON ACH.BF_SSN = DF.BF_SSN
			AND ACH.LN_SEQ = DF.LN_SEQ
		--Needed to determine fixed vs variable interest rate loans, should only ever return 1 result
		LEFT JOIN
		(
			SELECT DISTINCT
				BF_SSN,
				LN_SEQ
			FROM
				UDW..LN72_INT_RTE_HST LN72
			WHERE
				LN72.LC_ITR_TYP != ''
				AND LN72.LC_ITR_TYP IS NOT NULL
				AND LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
		) LN72_VAR
			ON LN10.BF_SSN = LN72_VAR.BF_SSN
			AND LN10.LN_SEQ = LN72_VAR.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN10.*,
				DF.Begin_Date AS  DF_Begin,
				DF.End_Date AS DF_End,
				ACH.Begin_Date AS ACH_Begin,
				ACH.End_Date AS ACH_End,
				LN72.LD_ITR_EFF_BEG AS LN72_Begin,
				LN72.LD_ITR_EFF_END AS LN72_End,
				PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
				LN72.LC_INT_RDC_PGM
			FROM
				UDW..LN10_LON LN10
				INNER JOIN @D_PERIOD DF
					ON LN10.BF_SSN = DF.BF_SSN
					AND LN10.LN_SEQ = DF.LN_SEQ
				INNER JOIN @ACH_PERIOD ACH
					ON LN10.BF_SSN = ACH.BF_SSN
					AND LN10.LN_SEQ = ACH.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..LN72_INT_RTE_HST LN72
					ON LN72.BF_SSN = DF.BF_SSN
					AND LN72.LN_SEQ = DF.LN_SEQ
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND LN72.LC_INT_RDC_PGM IN ('S','R','M','P')
				AND LN72.LC_STA_LON72 = 'A'
				AND 
				(
					(
						LN72.LD_ITR_EFF_BEG BETWEEN ACH.Begin_Date AND ACH.End_Date
						OR LN72.LD_ITR_EFF_END BETWEEN ACH.Begin_Date AND ACH.End_Date
					)
					AND
					(
						LN72.LD_ITR_EFF_BEG BETWEEN DF.Begin_Date AND DF.End_Date 
						OR LN72.LD_ITR_EFF_END BETWEEN DF.Begin_Date AND DF.End_Date
					)
				)
				AND
				(
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
				--Condition for variable rate loan exclusion
				AND
				(
					(
						LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						--Can End on the end of the deferment/forbearance, ACH, or june 30th
						AND 
						(
							(
								DATEPART(MONTH, LN72.LD_ITR_EFF_END) = 6
								AND DATEPART(DAY, LN72.LD_ITR_EFF_END) = 30
								AND GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							)
							OR
							(
								CAST(LN72.LD_ITR_EFF_END AS DATE) = CAST(ACH.End_Date AS DATE) 
							)
							OR
							(
								CAST(LN72.LD_ITR_EFF_END AS DATE) = CAST(DF.End_Date AS DATE) 
							)
						)
						AND
						(
							(
								LN72.LD_ITR_EFF_END <= ACH.End_Date
								AND ACH.End_Date <= DF.End_Date
							)
							OR
							(
								LN72.LD_ITR_EFF_END <= DF.End_Date
								AND DF.End_Date <= ACH.End_Date
							)
							OR
							(
								GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							)
						)
					)
				)
			) CVRD_D	
				ON DF.Begin_Date = CVRD_D.DF_Begin
				AND DF.End_Date = CVRD_D.DF_End
				AND ACH.Begin_Date = CVRD_D.ACH_Begin
				AND ACH.End_Date = CVRD_D.ACH_End
				AND DF.BF_SSN = CVRD_D.BF_SSN
				AND DF.LN_SEQ = CVRD_D.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND 
				(
					CVRD_D.BF_SSN IS NULL
					OR CVRD_D.LC_INT_RDC_PGM IN ('R','P')
				)
				AND
				( 
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
	) AS R2
	LEFT JOIN #DusterData DD
		ON DD.AccountNumber = R2.AccountNumber
		AND DD.LoanSequence = R2.LoanSequence
		AND DD.DefermentOrForbearanceBeginDate = R2.DefermentOrForbearanceBeginDate
		AND DD.DefermentOrForbearanceEndDate = R2.DefermentOrForbearanceEndDate
		AND DD.VariableRate = R2.VariableRate
WHERE 
	R2.VariableRate = 1
	AND
	(
		R2.HasPartialReducedRate = 0
		OR
		(
			R2.HasPartialReducedRate = 1
			AND 
			(
				LN72_Begin != R2.DefermentOrForbearanceBeginDate
				OR LN72_End != R2.DefermentOrForbearanceEndDate
			)
		)
	)
	AND DD.AccountNumber IS NULL

--R2 PART 3 VARIABLE RATE FORBEARANCE
INSERT INTO
	#DusterData
SELECT DISTINCT 
	R2.Report,
	R2.Ssn,
	R2.AccountNumber,
	R2.LoanSequence,
	R2.OwnerCode,
	R2.DefermentOrForbearanceOriginalBeginDate,
	R2.DefermentOrForbearanceOriginalEndDate,
	R2.DefermentOrForbearanceBeginDate,
	R2.DefermentOrForbearanceEndDate,
	R2.UpdatedAt,
	R2.VariableRate,
	R2.HasPartialReducedRate
FROM 	
	(/* R2D – Entering Deferment */
		SELECT DISTINCT
			'R2'				    AS Report
			,LN10.BF_SSN			AS Ssn
			,PD10.DF_SPE_ACC_ID		AS AccountNumber
			,LN10.LN_SEQ			AS LoanSequence
			,LN10.LF_LON_CUR_OWN	AS OwnerCode
			,NULL					AS DefermentOrForbearanceOriginalBeginDate
			,NULL					AS DefermentOrForbearanceOriginalEndDate
			,CASE WHEN DF.Begin_Date >= ACH.Begin_Date THEN DF.Begin_Date
			WHEN DF.Begin_Date < ACH.Begin_Date THEN ACH.Begin_Date			
			END AS DefermentOrForbearanceBeginDate
			,CASE WHEN DF.End_Date >= ACH.End_Date THEN ACH.End_Date
			WHEN DF.End_Date < ACH.End_Date THEN DF.End_Date
		    END AS DefermentOrForbearanceEndDate
			,NULL					AS UpdatedAt
			,CASE WHEN LN72_VAR.BF_SSN IS NULL THEN 0 ELSE 1  END AS VariableRate
			,CASE WHEN CVRD_D.LC_INT_RDC_PGM IN ('R','P') THEN 1 ELSE 0 END AS HasPartialReducedRate
			,CVRD_D.LN72_Begin AS LN72_Begin
			,CVRD_D.LN72_End AS LN72_End
		FROM
		UDW..LN10_LON LN10
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN @ACH_PERIOD ACH
			ON LN10.BF_SSN = ACH.BF_SSN
			AND LN10.LN_SEQ = ACH.LN_SEQ
		INNER JOIN @F_PERIOD DF
			ON ACH.BF_SSN = DF.BF_SSN
			AND ACH.LN_SEQ = DF.LN_SEQ
		--Needed to determine fixed vs variable interest rate loans, should only ever return 1 result
		LEFT JOIN
		(
			SELECT DISTINCT
				BF_SSN,
				LN_SEQ
			FROM
				UDW..LN72_INT_RTE_HST LN72
			WHERE
				LN72.LC_ITR_TYP != ''
				AND LN72.LC_ITR_TYP IS NOT NULL
				AND LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
		) LN72_VAR
			ON LN10.BF_SSN = LN72_VAR.BF_SSN
			AND LN10.LN_SEQ = LN72_VAR.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN10.*,
				DF.Begin_Date AS  DF_Begin,
				DF.End_Date AS DF_End,
				ACH.Begin_Date AS ACH_Begin,
				ACH.End_Date AS ACH_End,
				LN72.LD_ITR_EFF_BEG AS LN72_Begin,
				LN72.LD_ITR_EFF_END AS LN72_End,
				PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
				LN72.LC_INT_RDC_PGM
			FROM
				UDW..LN10_LON LN10
				INNER JOIN @F_PERIOD DF
					ON LN10.BF_SSN = DF.BF_SSN
					AND LN10.LN_SEQ = DF.LN_SEQ
				INNER JOIN @ACH_PERIOD ACH
					ON LN10.BF_SSN = ACH.BF_SSN
					AND LN10.LN_SEQ = ACH.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..LN72_INT_RTE_HST LN72
					ON LN72.BF_SSN = DF.BF_SSN
					AND LN72.LN_SEQ = DF.LN_SEQ
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND LN72.LC_INT_RDC_PGM IN ('S','R','M','P')
				AND LN72.LC_STA_LON72 = 'A'
				AND 
				(
					(
						LN72.LD_ITR_EFF_BEG BETWEEN ACH.Begin_Date AND ACH.End_Date
						OR LN72.LD_ITR_EFF_END BETWEEN ACH.Begin_Date AND ACH.End_Date
					)
					AND
					(
						LN72.LD_ITR_EFF_BEG BETWEEN DF.Begin_Date AND DF.End_Date 
						OR LN72.LD_ITR_EFF_END BETWEEN DF.Begin_Date AND DF.End_Date
					)
				)
				AND
				(
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
				--Condition for variable rate loan exclusion
				AND
				(
					(
						LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						--Can End on the end of the deferment/forbearance, ACH, or june 30th
						AND 
						(
							(
								DATEPART(MONTH, LN72.LD_ITR_EFF_END) = 6
								AND DATEPART(DAY, LN72.LD_ITR_EFF_END) = 30
								AND GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							)
							OR
							(
								CAST(LN72.LD_ITR_EFF_END AS DATE) = CAST(ACH.End_Date AS DATE) 
							)
							OR
							(
								CAST(LN72.LD_ITR_EFF_END AS DATE) = CAST(DF.End_Date AS DATE) 
							)
						)
						AND
						(
							(
								LN72.LD_ITR_EFF_END <= ACH.End_Date
								AND ACH.End_Date <= DF.End_Date
							)
							OR
							(
								LN72.LD_ITR_EFF_END <= DF.End_Date
								AND DF.End_Date <= ACH.End_Date
							)
							OR
							(
								GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
							)
						)
					)
				)
			) CVRD_D	
				ON DF.Begin_Date = CVRD_D.DF_Begin
				AND DF.End_Date = CVRD_D.DF_End
				AND ACH.Begin_Date = CVRD_D.ACH_Begin
				AND ACH.End_Date = CVRD_D.ACH_End
				AND DF.BF_SSN = CVRD_D.BF_SSN
				AND DF.LN_SEQ = CVRD_D.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
				AND 
				(
					CVRD_D.BF_SSN IS NULL
					OR CVRD_D.LC_INT_RDC_PGM IN ('R','P')
				)
				AND
				( 
					DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
					OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
					OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
				)
	) AS R2
	LEFT JOIN #DusterData DD
		ON DD.AccountNumber = R2.AccountNumber
		AND DD.LoanSequence = R2.LoanSequence
		AND DD.DefermentOrForbearanceBeginDate = R2.DefermentOrForbearanceBeginDate
		AND DD.DefermentOrForbearanceEndDate = R2.DefermentOrForbearanceEndDate
		AND DD.VariableRate = R2.VariableRate
WHERE 
	R2.VariableRate = 1
	AND
	(
		R2.HasPartialReducedRate = 0
		OR
		(
			R2.HasPartialReducedRate = 1
			AND 
			(
				LN72_Begin != R2.DefermentOrForbearanceBeginDate
				OR LN72_End != R2.DefermentOrForbearanceEndDate
			)
		)
	)
	AND DD.AccountNumber IS NULL

--R4 FXIED RATE
INSERT INTO
	#DusterData
SELECT DISTINCT
	*,
	0 --partial reduced rate false
FROM 
	(/* R4 File - Ineligible RIR */
		SELECT DISTINCT
			'R4'					AS Report
			,LN10.BF_SSN			AS Ssn
			,PD10.DF_SPE_ACC_ID		AS AccountNumber
			,LN10.LN_SEQ			AS LoanSequence
			,LN10.LF_LON_CUR_OWN	AS OwnerCode
			,LN72.LD_ITR_EFF_BEG	AS DefermentOrForbearanceOriginalBeginDate
			,LN72.LD_ITR_EFF_END	AS DefermentOrForbearanceOriginalEndDate
			,NULL					AS DefermentOrForbearanceBeginDate
			,NULL					AS DefermentOrForbearanceEndDate
			,LN72.LF_LST_DTS_LN72	AS UpdatedAt
			,CASE WHEN LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2') THEN 1 ELSE 0 END AS VariableRate
		FROM
			UDW..LN72_INT_RTE_HST LN72 
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON LN72.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW..LN10_LON LN10
				ON LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
			INNER JOIN @SPECIAL_PAY_POP POP
				ON LN72.BF_SSN = POP.BF_SSN
			LEFT JOIN
			(
				SELECT
					DF.BF_SSN AS BF_SSN,
					DF.LN_SEQ AS LN_SEQ,
					DF.Begin_Date AS DF_Begin,
					DF.End_Date AS DF_End,
					ACH.Begin_Date AS ACH_Begin,
					ACH.End_Date AS ACH_End
				FROM
					@DF_PERIOD DF
					INNER JOIN @ACH_PERIOD ACH
						ON DF.BF_SSN = ACH.BF_SSN
						AND DF.LN_SEQ = ACH.LN_SEQ
			) ACH_DF
				ON LN72.BF_SSN = ACH_DF.BF_SSN
				AND LN72.LN_SEQ = ACH_DF.LN_SEQ
				AND LN72.LD_ITR_EFF_BEG BETWEEN ACH_DF.DF_Begin AND ACH_DF.DF_End --Between Some Deferment And ACH
				AND LN72.LD_ITR_EFF_END BETWEEN ACH_DF.DF_Begin AND ACH_DF.DF_End
				AND LN72.LD_ITR_EFF_BEG BETWEEN ACH_DF.ACH_Begin AND ACH_DF.ACH_End 
				AND LN72.LD_ITR_EFF_END BETWEEN ACH_DF.ACH_Begin AND ACH_DF.ACH_End
				AND
				(
					--ACH Starts After And Ends Before D/F
					(
						ACH_DF.ACH_Begin >= ACH_DF.DF_Begin
						AND ACH_DF.ACH_End <= ACH_DF.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_DF.ACH_Begin
						AND LN72.LD_ITR_EFF_END = ACH_DF.ACH_End
					)
					OR--ACH Starts Before And Ends After D/F
					(
						ACH_DF.ACH_Begin <= ACH_DF.DF_Begin
						AND ACH_DF.ACH_End >= ACH_DF.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_DF.DF_Begin
						AND LN72.LD_ITR_EFF_END = ACH_DF.DF_End
					)
					OR--ACH Starts Before D/F And Ends Before D/F
					(
						ACH_DF.ACH_Begin <= ACH_DF.DF_Begin
						AND ACH_DF.ACH_End <= ACH_DF.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_DF.DF_Begin
						AND LN72.LD_ITR_EFF_END = ACH_DF.ACH_End
					)
					OR--ACH Starts Before And Ends After D/F
					(
						ACH_DF.ACH_Begin >= ACH_DF.DF_Begin
						AND ACH_DF.ACH_End >= ACH_DF.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_DF.ACH_Begin
						AND LN72.LD_ITR_EFF_END = ACH_DF.DF_End
					)
				)
			    --Used here to determine partially covered M rate DF/ACH periods for manual review
				LEFT JOIN
				(
					SELECT
						LN10.*,
						DF.Begin_Date AS  DF_Begin,
						DF.End_Date AS DF_End,
						ACH.Begin_Date AS ACH_Begin,
						ACH.End_Date AS ACH_End,
						LN72.Begin_Date AS LN72_Begin,
						LN72.End_Date AS LN72_End,
						PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
						LN72_M.LC_INT_RDC_PGM
					FROM
						UDW..LN10_LON LN10
						INNER JOIN @DF_PERIOD DF
							ON LN10.BF_SSN = DF.BF_SSN
							AND LN10.LN_SEQ = DF.LN_SEQ
						INNER JOIN @ACH_PERIOD ACH
							ON LN10.BF_SSN = ACH.BF_SSN
							AND LN10.LN_SEQ = ACH.LN_SEQ
						INNER JOIN UDW..PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN @LN72_PERIOD LN72
							ON LN10.BF_SSN = LN72.BF_SSN
							AND LN10.LN_SEQ = LN72.LN_SEQ
						LEFT JOIN UDW..LN72_INT_RTE_HST LN72_M
							ON LN72.BF_SSN = LN72_M.BF_SSN
							AND LN72.LN_SEQ = LN72_M.LN_SEQ
							AND LN72_M.LD_ITR_EFF_BEG BETWEEN LN72.Begin_Date AND LN72.End_Date
							AND LN72_M.LD_ITR_EFF_END BETWEEN LN72.Begin_Date AND LN72.End_Date
							AND LN72_M.LC_INT_RDC_PGM = 'M'
					WHERE 
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
						AND LN72_M.BF_SSN IS NOT NULL
						AND
						(
							DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
							OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
							OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
							OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
						)
					) CVRD	
						ON LN72.LD_ITR_EFF_BEG BETWEEN CVRD.LN72_Begin AND CVRD.LN72_End
						AND LN72.LD_ITR_EFF_END BETWEEN CVRD.LN72_Begin AND CVRD.LN72_End
						AND LN72.BF_SSN = CVRD.BF_SSN
						AND LN72.LN_SEQ = CVRD.LN_SEQ
		WHERE
			LN72.LC_INT_RDC_PGM IN ('S','R')
			AND CVRD.BF_SSN IS NULL
			AND LN72.LC_STA_LON72 = 'A'
			AND LN10.LC_STA_LON10 ='R'
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
			AND ACH_DF.BF_SSN IS NULL
			AND NOT
			(
				CONVERT(DATE,LN10.LD_LON_EFF_ADD) = CONVERT(DATE,LN72.LD_ITR_EFF_BEG)
				AND LN72.LC_INT_RDC_PGM != ''
			)
			AND LN72.LC_ITR_TYP NOT IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
	) AS R4
WHERE
	VariableRate = 0

--R4 PART 2 VARIABLE RATE DEFERMENTS
INSERT INTO
	#DusterData
SELECT DISTINCT
	*,
	0 --partial reduced rate false
FROM 
	(/* R4 File - Ineligible RIR */
		SELECT DISTINCT
			'R4'					AS Report
			,LN10.BF_SSN			AS Ssn
			,PD10.DF_SPE_ACC_ID		AS AccountNumber
			,LN10.LN_SEQ			AS LoanSequence
			,LN10.LF_LON_CUR_OWN	AS OwnerCode
			,LN72.LD_ITR_EFF_BEG	AS DefermentOrForbearanceOriginalBeginDate
			,LN72.LD_ITR_EFF_END	AS DefermentOrForbearanceOriginalEndDate
			,NULL					AS DefermentOrForbearanceBeginDate
			,NULL					AS DefermentOrForbearanceEndDate
			,LN72.LF_LST_DTS_LN72	AS UpdatedAt
			,CASE WHEN LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2') THEN 1 ELSE 0 END AS VariableRate
		FROM
			UDW..LN72_INT_RTE_HST LN72 
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON LN72.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW..LN10_LON LN10
				ON LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
			INNER JOIN @SPECIAL_PAY_POP POP
				ON LN72.BF_SSN = POP.BF_SSN
			LEFT JOIN
			(
				SELECT
					DF.BF_SSN AS BF_SSN,
					DF.LN_SEQ AS LN_SEQ,
					DF.Begin_Date AS DF_Begin,
					DF.End_Date AS DF_End,
					ACH.Begin_Date AS ACH_Begin,
					ACH.End_Date AS ACH_End
				FROM
					@D_PERIOD DF
					INNER JOIN @ACH_PERIOD ACH
						ON DF.BF_SSN = ACH.BF_SSN
						AND DF.LN_SEQ = ACH.LN_SEQ
			) ACH_D
				ON LN72.BF_SSN = ACH_D.BF_SSN
				AND LN72.LN_SEQ = ACH_D.LN_SEQ
				AND LN72.LD_ITR_EFF_BEG BETWEEN ACH_D.DF_Begin AND ACH_D.DF_End --Between Some Deferment And ACH
				AND LN72.LD_ITR_EFF_END BETWEEN ACH_D.DF_Begin AND ACH_D.DF_End
				AND LN72.LD_ITR_EFF_BEG BETWEEN ACH_D.ACH_Begin AND ACH_D.ACH_End 
				AND LN72.LD_ITR_EFF_END BETWEEN ACH_D.ACH_Begin AND ACH_D.ACH_End
				AND
				(
					--ACH Starts After And Ends Before D/F
					(
						ACH_D.ACH_Begin >= ACH_D.DF_Begin
						AND ACH_D.ACH_End <= ACH_D.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_D.ACH_Begin
						AND LN72.LD_ITR_EFF_END = ACH_D.ACH_End
					)
					OR--ACH Starts Before And Ends After D/F
					(
						ACH_D.ACH_Begin <= ACH_D.DF_Begin
						AND ACH_D.ACH_End >= ACH_D.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_D.DF_Begin
						AND LN72.LD_ITR_EFF_END = ACH_D.DF_End
					)
					OR--ACH Starts Before D/F And Ends Before D/F
					(
						ACH_D.ACH_Begin <= ACH_D.DF_Begin
						AND ACH_D.ACH_End <= ACH_D.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_D.DF_Begin
						AND LN72.LD_ITR_EFF_END = ACH_D.ACH_End
					)
					OR--ACH Starts Before And Ends After D/F
					(
						ACH_D.ACH_Begin >= ACH_D.DF_Begin
						AND ACH_D.ACH_End >= ACH_D.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_D.ACH_Begin
						AND LN72.LD_ITR_EFF_END = ACH_D.DF_End
					)
					--Variable rate record ends on period boundary
					OR
					(
						LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						AND LN72.LD_ITR_EFF_END IN (ACH_D.DF_END, ACH_D.ACH_End)
					)
					OR
					(
						LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						AND DATEPART(MONTH, LN72.LD_ITR_EFF_END) = 6
						AND DATEPART(DAY, LN72.LD_ITR_EFF_END) = 30
					)
						
				)
				LEFT JOIN
				(
					SELECT
						DF.BF_SSN AS BF_SSN,
						DF.LN_SEQ AS LN_SEQ,
						DF.Begin_Date AS DF_Begin,
						DF.End_Date AS DF_End,
						ACH.Begin_Date AS ACH_Begin,
						ACH.End_Date AS ACH_End
					FROM
						@F_PERIOD DF
						INNER JOIN @ACH_PERIOD ACH
							ON DF.BF_SSN = ACH.BF_SSN
							AND DF.LN_SEQ = ACH.LN_SEQ
				) ACH_F
				ON LN72.BF_SSN = ACH_F.BF_SSN
				AND LN72.LN_SEQ = ACH_F.LN_SEQ
				AND LN72.LD_ITR_EFF_BEG BETWEEN ACH_F.DF_Begin AND ACH_F.DF_End --Between Some Deferment And ACH
				AND LN72.LD_ITR_EFF_END BETWEEN ACH_F.DF_Begin AND ACH_F.DF_End
				AND LN72.LD_ITR_EFF_BEG BETWEEN ACH_F.ACH_Begin AND ACH_F.ACH_End 
				AND LN72.LD_ITR_EFF_END BETWEEN ACH_F.ACH_Begin AND ACH_F.ACH_End
				AND
				(
					--ACH Starts After And Ends Before D/F
					(
						ACH_F.ACH_Begin >= ACH_F.DF_Begin
						AND ACH_F.ACH_End <= ACH_F.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_F.ACH_Begin
						AND LN72.LD_ITR_EFF_END = ACH_F.ACH_End
					)
					OR--ACH Starts Before And Ends After D/F
					(
						ACH_F.ACH_Begin <= ACH_F.DF_Begin
						AND ACH_F.ACH_End >= ACH_F.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_F.DF_Begin
						AND LN72.LD_ITR_EFF_END = ACH_F.DF_End
					)
					OR--ACH Starts Before D/F And Ends Before D/F
					(
						ACH_F.ACH_Begin <= ACH_F.DF_Begin
						AND ACH_F.ACH_End <= ACH_F.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_F.DF_Begin
						AND LN72.LD_ITR_EFF_END = ACH_F.ACH_End
					)
					OR--ACH Starts Before And Ends After D/F
					(
						ACH_F.ACH_Begin >= ACH_F.DF_Begin
						AND ACH_F.ACH_End >= ACH_F.DF_End
						AND LN72.LD_ITR_EFF_BEG = ACH_F.ACH_Begin
						AND LN72.LD_ITR_EFF_END = ACH_F.DF_End
					)
					--Variable rate record ends on period boundary
					OR
					(
						LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						AND LN72.LD_ITR_EFF_END IN (ACH_F.DF_END, ACH_F.ACH_End)
					)
					OR
					(
						LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
						AND DATEPART(MONTH, LN72.LD_ITR_EFF_END) = 6
						AND DATEPART(DAY, LN72.LD_ITR_EFF_END) = 30
					)
				)
				--Used here to determine partially covered M rate DF/ACH periods for manual review
				LEFT JOIN
				(
					SELECT
						LN10.*,
						DF.Begin_Date AS  DF_Begin,
						DF.End_Date AS DF_End,
						ACH.Begin_Date AS ACH_Begin,
						ACH.End_Date AS ACH_End,
						LN72.Begin_Date AS LN72_Begin,
						LN72.End_Date AS LN72_End,
						PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
						LN72_M.LC_INT_RDC_PGM
					FROM
						UDW..LN10_LON LN10
						INNER JOIN @D_PERIOD DF
							ON LN10.BF_SSN = DF.BF_SSN
							AND LN10.LN_SEQ = DF.LN_SEQ
						INNER JOIN @ACH_PERIOD ACH
							ON LN10.BF_SSN = ACH.BF_SSN
							AND LN10.LN_SEQ = ACH.LN_SEQ
						INNER JOIN UDW..PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN @LN72_PERIOD LN72
							ON LN10.BF_SSN = LN72.BF_SSN
							AND LN10.LN_SEQ = LN72.LN_SEQ
						LEFT JOIN UDW..LN72_INT_RTE_HST LN72_M
							ON LN72.BF_SSN = LN72_M.BF_SSN
							AND LN72.LN_SEQ = LN72_M.LN_SEQ
							AND LN72_M.LD_ITR_EFF_BEG BETWEEN LN72.Begin_Date AND LN72.End_Date
							AND LN72_M.LD_ITR_EFF_END BETWEEN LN72.Begin_Date AND LN72.End_Date
							AND LN72_M.LC_INT_RDC_PGM = 'M'
					WHERE 
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
						AND LN72_M.BF_SSN IS NOT NULL
						AND
						(
							DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
							OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
							OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
							OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
						)
					) CVRD_D	
						ON LN72.LD_ITR_EFF_BEG BETWEEN CVRD_D.LN72_Begin AND CVRD_D.LN72_End
						AND LN72.LD_ITR_EFF_END BETWEEN CVRD_D.LN72_Begin AND CVRD_D.LN72_End
						AND LN72.BF_SSN = CVRD_D.BF_SSN
						AND LN72.LN_SEQ = CVRD_D.LN_SEQ
				--Used here to determine partially covered M rate DF/ACH periods for manual review
				LEFT JOIN
				(
					SELECT
						LN10.*,
						DF.Begin_Date AS  DF_Begin,
						DF.End_Date AS DF_End,
						ACH.Begin_Date AS ACH_Begin,
						ACH.End_Date AS ACH_End,
						LN72.Begin_Date AS LN72_Begin,
						LN72.End_Date AS LN72_End,
						PD10.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
						LN72_M.LC_INT_RDC_PGM
					FROM
						UDW..LN10_LON LN10
						INNER JOIN @F_PERIOD DF
							ON LN10.BF_SSN = DF.BF_SSN
							AND LN10.LN_SEQ = DF.LN_SEQ
						INNER JOIN @ACH_PERIOD ACH
							ON LN10.BF_SSN = ACH.BF_SSN
							AND LN10.LN_SEQ = ACH.LN_SEQ
						INNER JOIN UDW..PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN @LN72_PERIOD LN72
							ON LN10.BF_SSN = LN72.BF_SSN
							AND LN10.LN_SEQ = LN72.LN_SEQ
						LEFT JOIN UDW..LN72_INT_RTE_HST LN72_M
							ON LN72.BF_SSN = LN72_M.BF_SSN
							AND LN72.LN_SEQ = LN72_M.LN_SEQ
							AND LN72_M.LD_ITR_EFF_BEG BETWEEN LN72.Begin_Date AND LN72.End_Date
							AND LN72_M.LD_ITR_EFF_END BETWEEN LN72.Begin_Date AND LN72.End_Date
							AND LN72_M.LC_INT_RDC_PGM = 'M'
					WHERE 
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
						AND LN72_M.BF_SSN IS NOT NULL
						AND
						(
							DF.Begin_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
							OR DF.End_Date BETWEEN ACH.Begin_Date AND ACH.End_Date
							OR ACH.Begin_Date BETWEEN DF.Begin_Date AND DF.End_Date
							OR ACH.End_Date BETWEEN DF.Begin_Date AND DF.End_Date
						)
					) CVRD_F	
						ON LN72.LD_ITR_EFF_BEG BETWEEN CVRD_F.LN72_Begin AND CVRD_F.LN72_End
						AND LN72.LD_ITR_EFF_END BETWEEN CVRD_F.LN72_Begin AND CVRD_F.LN72_End
						AND LN72.BF_SSN = CVRD_F.BF_SSN
						AND LN72.LN_SEQ = CVRD_F.LN_SEQ
		WHERE
			LN72.LC_INT_RDC_PGM IN ('S','R')
			AND LN72.LC_STA_LON72 = 'A'
			AND LN10.LC_STA_LON10 ='R'
			AND LN10.LA_CUR_PRI > 0.00
			AND CVRD_D.BF_SSN IS NULL
			AND CVRD_F.BF_SSN IS NULL
			AND LN10.LF_LON_CUR_OWN LIKE '8297690%'
			AND 
			(
				ACH_D.BF_SSN IS NULL
				AND ACH_F.BF_SSN IS NULL
			)
			AND NOT
			(
				CONVERT(DATE,LN10.LD_LON_EFF_ADD) = CONVERT(DATE,LN72.LD_ITR_EFF_BEG)
				AND LN72.LC_INT_RDC_PGM != ''
			)
			--AND NOT
			--(
			--    DATEPART(MONTH, LN72.LD_ITR_EFF_END) = 6
			--	AND DATEPART(DAY, LN72.LD_ITR_EFF_END) = 30
			--)
			AND LN72.LC_ITR_TYP IN ('SV', 'V1', 'V2', 'C1', 'C2', 'F2')
	) AS R4
WHERE
	VariableRate = 1

--GET RESULT
--Insert new records, filtering out old ones
INSERT INTO [achrirdf].[ProcessQueue]
	        ([Report]
	        ,[Ssn]
	        ,[AccountNumber]
	        ,[LoanSequence]
	        ,[OwnerCode]
	        ,[DefermentOrForbearanceOriginalBeginDate]
	        ,[DefermentOrForbearanceOriginalEndDate]
	        ,[DefermentOrForbearanceBeginDate]
	        ,[DefermentOrForbearanceEndDate]
	        ,[UpdatedAt]
			,[VariableRate]
			,[HasPartialReducedRate]
	        ,[ProcessedAt]
	        ,[ProcessedBy]
	        ,[CreatedAt]
	        ,[CreatedBy]
			)
SELECT
	DD.Report
	,DD.Ssn
	,DD.AccountNumber
	,DD.LoanSequence
	,DD.OwnerCode
	,DD.DefermentOrForbearanceOriginalBeginDate
	,DD.DefermentOrForbearanceOriginalEndDate
	,DD.DefermentOrForbearanceBeginDate
	,DD.DefermentOrForbearanceEndDate
	,ISNULL(DD.UpdatedAt, GETDATE())
	,DD.VariableRate
	,DD.HasPartialReducedRate
	,NULL
	,NULL
	,GETDATE()
	,SYSTEM_USER
FROM
	#DusterData DD
	LEFT JOIN ULS.achrirdf.ProcessQueue PQ
		ON DD.Report = PQ.Report
		AND DD.AccountNumber = PQ.AccountNumber
		AND DD.LoanSequence = PQ.LoanSequence
		AND DD.OwnerCode = PQ.OwnerCode
		AND 
		(
			DD.DefermentOrForbearanceBeginDate = PQ.DefermentOrForbearanceBeginDate
			OR 
			(
				DD.DefermentOrForbearanceBeginDate IS NULL 
				AND PQ.DefermentOrForbearanceBeginDate IS NULL
			)
		)
		AND 
		(
			DD.DefermentOrForbearanceEndDate = PQ.DefermentOrForbearanceEndDate
			OR 
			(
				DD.DefermentOrForbearanceEndDate IS NULL 
				AND PQ.DefermentOrForbearanceEndDate IS NULL
			)
		)
		AND 
		(
			DD.DefermentOrForbearanceOriginalBeginDate = PQ.DefermentOrForbearanceOriginalBeginDate
			OR 
			(
				DD.DefermentOrForbearanceOriginalBeginDate IS NULL 
				AND PQ.DefermentOrForbearanceOriginalBeginDate IS NULL
			)
		)
		AND 
		(
			DD.DefermentOrForbearanceOriginalEndDate = PQ.DefermentOrForbearanceOriginalEndDate
			OR 
			(
				DD.DefermentOrForbearanceOriginalEndDate IS NULL 
				AND PQ.DefermentOrForbearanceOriginalEndDate IS NULL
			)
		)
		AND DD.VariableRate = PQ.VariableRate
		AND DD.HasPartialReducedRate = PQ.HasPartialReducedRate
		AND PQ.ProcessedAt IS NULL
WHERE
	PQ.AccountNumber IS NULL
ORDER BY 
	DD.Ssn

IF OBJECT_ID('tempdb..#DusterData') IS NOT NULL
	DROP TABLE #DusterData

END