



CREATE PROCEDURE [dbo].[LT_TS06BAPIDR_Loans]
	@AccountNumber		char(10)
AS
BEGIN
DECLARE @ROWS INT = 0

-- drop temp table to if it exist
IF OBJECT_ID('tempdb..#RD') IS NOT NULL DROP TABLE #RD

-- create temp table to hold LEGEND data
CREATE TABLE #RD
(
		SEQ INT,
		LN_SEQ INT,
		LN_GRD_RPS_SEQ INT,
		Label VARCHAR(150), 
		LD_LON_1_DSB DATE,
		LA_LON_AMT_GTR DECIMAL(14,2),
		LA_CUR_PRI DECIMAL(14,2),
		LR_ITR DECIMAL(5,3),
		LC_TYP_SCH_DIS VARCHAR(3),
		LA_TOT_RPD_DIS DECIMAL(14,2),
		LA_ANT_CAP DECIMAL(14,2),
		DueDate DATE,
		LN_RPS_TRM INT,
		LA_RPS_ISL DECIMAL(14,2),
		TermsToDate INT
)

-- variable to hold dynamically created query
DECLARE @SQLStatement VARCHAR(MAX) =
	' 
		INSERT INTO
			#RD
		SELECT
			RD.SEQ,
			RD.LN_SEQ,
			RD.LN_GRD_RPS_SEQ,
			FMT.Label, 
			RD.LD_LON_1_DSB,
			RD.LA_LON_AMT_GTR,
			RD.LA_CUR_PRI,
			RD.LR_ITR,
			RD.LC_TYP_SCH_DIS,
			RD.LA_TOT_RPD_DIS,
			RD.LA_ANT_CAP,
			CONVERT(VARCHAR(10), RD.LD_RPS_1_PAY_DU, 101) AS [DueDate],
			RD.LN_RPS_TRM,
			RD.LA_RPS_ISL,
			NULL
		FROM 
			OPENQUERY
			(	LEGEND_TEST_VUK3, 
				''SELECT
					ROW_NUMBER() OVER (PARTITION BY LN66.BF_SSN, LN66.LN_SEQ, LN66.LN_RPS_SEQ ORDER BY LN66.LN_GRD_RPS_SEQ) AS SEQ,
					LN10.LN_SEQ,
					LN66.LN_GRD_RPS_SEQ,
					LN10.IC_LON_PGM,
					LN10.LD_LON_1_DSB,
					LN10.LA_LON_AMT_GTR,
					LN10.LA_CUR_PRI,
					LN72.LR_ITR,
					LN65.LC_TYP_SCH_DIS,
					LN65.LA_TOT_RPD_DIS,
					LN65.LA_ANT_CAP,
					RS10.LD_RPS_1_PAY_DU,
					LN66.LN_RPS_TRM,
					LN66.LA_RPS_ISL
				FROM
					PKUB.PD10_PRS_NME PD10
					INNER JOIN PKUB.LN10_LON LN10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN
					(
						SELECT
							LN72.BF_SSN,
							LN72.LN_SEQ,
							LN72.LR_ITR,
							ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
						FROM	
							PKUB.LN72_INT_RTE_HST LN72
							INNER JOIN PKUB.PD10_PRS_NME PD10 ON PD10.DF_PRS_ID = LN72.BF_SSN
						WHERE
							LN72.LC_STA_LON72 = ''''A''''
							AND
							PD10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
					) LN72 ON LN10.BF_SSN = LN72.BF_SSN
							AND LN10.LN_SEQ = LN72.LN_SEQ
							AND LN72.SEQ = 1
					INNER JOIN PKUB.LN65_LON_RPS LN65 
						ON LN65.BF_SSN = LN10.BF_SSN
						AND LN65.LN_SEQ = LN10.LN_SEQ
					INNER JOIN PKUB.LN66_LON_RPS_SPF LN66
						ON LN66.BF_SSN = LN65.BF_SSN
						AND LN66.LN_SEQ = LN65.LN_SEQ
						AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ
					INNER JOIN PKUB.RS10_BR_RPD RS10
						ON RS10.BF_SSN = LN65.BF_SSN
						AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
				WHERE 
					PD10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
				AND LN65.LC_STA_LON65 = ''''A''''
			''
			) RD  
			INNER JOIN CDW..FormatTranslation FMT ON RD.IC_LON_PGM = FMT.Start
	'
 
-- execuate the dynamically created sql statement (inserting results into temp table)
EXEC (@SQLStatement)

-- CTE to calculate running total of repayment term months
;WITH SUMS
AS
(
	SELECT
		SEQ, 
		LN_SEQ, 
		LN_GRD_RPS_SEQ,
		LN_RPS_TRM, 
		TermsToDate = LN_RPS_TRM -- start with first term
	FROM
		#RD
	WHERE
		SEQ = 1 -- start with sequence 1
	
	UNION ALL
	
	SELECT
		RD.SEQ, 
		RD.LN_SEQ, 
		RD.LN_GRD_RPS_SEQ,
		RD.LN_RPS_TRM, 
		S.TermsToDate + RD.LN_RPS_TRM -- add terms for current record to running total
	FROM
		SUMS S
		INNER JOIN #RD RD ON RD.SEQ = S.SEQ + 1 AND RD.LN_SEQ = S.LN_SEQ
)
UPDATE
	RD
SET
	RD.TermsToDate = S.TermsToDate,
	RD.DueDate = DATEADD(MONTH, S.TermsToDate - RD.LN_RPS_TRM, RD.DueDate)
FROM 
	SUMS S
	INNER JOIN #RD RD ON RD.SEQ = S.SEQ AND RD.LN_SEQ = S.LN_SEQ
WHERE
	S.SEQ != 1 -- don't change starting due date

-- select data for letter
SELECT
	Label AS [Loan Program], 
	CONVERT(VARCHAR(10) ,LD_LON_1_DSB, 101) AS [First Disbursement Date],
	LA_LON_AMT_GTR AS [Original Balance],
	LA_CUR_PRI AS [Current Principal],
	LR_ITR AS [Interest Rate],
	CASE 
		WHEN LC_TYP_SCH_DIS IN ('I5') THEN 'REPAYE'
		WHEN LC_TYP_SCH_DIS IN ('CA','CP') THEN 'PAYE'
		WHEN LC_TYP_SCH_DIS IN ('CQ','C1','C2','C3') THEN 'ICR'
		WHEN LC_TYP_SCH_DIS IN ('IB','IL','IP','I3') THEN 'IBR'
		ELSE 'IDR'
	END AS [Schedule Type],
	LA_TOT_RPD_DIS AS [Total Repay Amount],
	LA_ANT_CAP AS [Anticipated Cap],
	CONVERT(VARCHAR(10), DueDate, 101) AS [Due Date],
	LN_RPS_TRM AS [Repay Term in Months],
	LA_RPS_ISL AS [Installment Amount]
FROM
	#RD
ORDER BY
	LN_SEQ,
	SEQ

SET @ROWS = @@ROWCOUNT

-- cleanup temp table
IF OBJECT_ID('tempdb..#RD') IS NOT NULL DROP TABLE #RD

IF @ROWS = 0 
	BEGIN
		RAISERROR('[dbo].[LT_TS06BAPIDR_Loans] - No data returned for AccountNumber %s',11,2, @AccountNumber)
	END

END