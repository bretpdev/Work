CREATE PROCEDURE [verforbfed].[BorrowerMadeFullPaymentInLastYear]
	@Ssn CHAR(9)
AS

IF OBJECT_ID('tempdb..#RepaymentData', 'U') IS NOT NULL
BEGIN
       DROP TABLE #RepaymentData
END

IF OBJECT_ID('tempdb..#ReportData', 'U') IS NOT NULL
BEGIN
       DROP TABLE #ReportData
END

SELECT
       ROW_NUMBER() OVER (PARTITION BY RS10.BF_SSN, LN10.LN_SEQ, RS10.LN_RPS_SEQ, LN66.LN_GRD_RPS_SEQ ORDER BY RS10.LN_RPS_SEQ DESC) [REP_SEQ],
       LN10.BF_SSN,
       LN10.LN_SEQ,
       RS10.LN_RPS_SEQ,
       LN66.LN_GRD_RPS_SEQ,
       LN10.IC_LON_PGM,
       LN10.LD_LON_1_DSB,
       LN10.LA_LON_AMT_GTR,
       LN10.LA_CUR_PRI,
       LN65.LC_TYP_SCH_DIS,
       LN65.LA_TOT_RPD_DIS,
       LN65.LA_ANT_CAP,
       RS10.LD_RPS_1_PAY_DU,
       CAST(ISNULL(LN66.LN_RPS_TRM, 0) AS INT) [LN_RPS_TRM],
       LN66.LA_RPS_ISL,
       DATEDIFF(MONTH, RS10.LD_RPS_1_PAY_DU, GETDATE()) [GradationMonths],
       CAST(RS10.LD_RPS_1_PAY_DU AS DATE) [TermStartDate],
       CAST(0 AS INT) [TermsToDate]
INTO
    #RepaymentData
FROM
    PD10_PRS_NME PD10
    INNER JOIN LN10_LON LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
    INNER JOIN RS10_BR_RPD RS10 ON RS10.BF_SSN = PD10.DF_PRS_ID
    INNER JOIN LN65_LON_RPS LN65 ON LN65.BF_SSN = LN10.BF_SSN AND LN65.LN_SEQ = LN10.LN_SEQ AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
    INNER JOIN LN66_LON_RPS_SPF LN66 ON LN66.BF_SSN = LN65.BF_SSN AND LN66.LN_SEQ = LN65.LN_SEQ AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ  
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND
	LN10.LA_CUR_PRI > 0.00
	AND
	LN65.LC_STA_LON65 = 'A'
	AND
	RS10.LC_STA_RPST10 = 'A'
	AND
	LN10.BF_SSN = @Ssn
ORDER BY
       LN10.BF_SSN,
       LN10.LN_SEQ

;WITH SUMS
AS
(
       SELECT
              BF_SSN,
              LN_SEQ, 
              LN_GRD_RPS_SEQ,
              LN_RPS_TRM, 
              TermsToDate = RD.LN_RPS_TRM -- start with first term
       FROM
              #RepaymentData RD
       WHERE
              LN_GRD_RPS_SEQ = 1 -- start with sequence 1
              AND
              REP_SEQ = 1 -- newest repayment schedule
       
       UNION ALL
       
       SELECT
              RD.BF_SSN,
              RD.LN_SEQ, 
              RD.LN_GRD_RPS_SEQ,
              RD.LN_RPS_TRM, 
              S.TermsToDate + RD.LN_RPS_TRM -- add terms for current record to running total
       FROM
              SUMS S
              INNER JOIN #RepaymentData RD ON RD.LN_GRD_RPS_SEQ = S.LN_GRD_RPS_SEQ + 1 AND RD.BF_SSN = S.BF_SSN AND RD.LN_SEQ = S.LN_SEQ AND RD.REP_SEQ = 1 -- newest repayment schedule
)
UPDATE
       RD
SET
       RD.TermsToDate = S.TermsToDate,
       RD.TermStartDate = DATEADD(MONTH, S.TermsToDate - RD.LN_RPS_TRM, RD.TermStartDate)
FROM 
       SUMS S
       INNER JOIN #RepaymentData RD ON RD.LN_GRD_RPS_SEQ = S.LN_GRD_RPS_SEQ AND RD.BF_SSN = S.BF_SSN AND RD.LN_SEQ = S.LN_SEQ AND RD.REP_SEQ = 1

-- SELECT Report DATA
SELECT
       RD.BF_SSN,
       RD.LN_SEQ,
       RD.LD_RPS_1_PAY_DU [1stPaymentDue],
       RD.LC_TYP_SCH_DIS,
       RD.LA_RPS_ISL [MonthlyPayment],
       RD.LN_GRD_RPS_SEQ [CurrentGradation],
       RD.LA_CUR_PRI [OutstandingPrincipal],
       DW01.WA_TOT_BRI_OTS [OutstandingInterest],
       RD.TermsToDate,
       RD.LN_RPS_TRM,
       (RD.TermsToDate - RD.LN_RPS_TRM + 1) [bottom_end],
       CASE WHEN RD.GradationMonths < 1 THEN 1 ELSE RD.GradationMonths END [GradationMonths],
       RD.TermsToDate [top_end]
INTO 
       #ReportData
FROM
       #RepaymentData RD
       INNER JOIN DW01_DW_CLC_CLU DW01 ON DW01.BF_SSN = RD.BF_SSN AND DW01.LN_SEQ = RD.LN_SEQ
WHERE
       (CASE WHEN RD.GradationMonths < 1 THEN 1 ELSE RD.GradationMonths END) BETWEEN (RD.TermsToDate - RD.LN_RPS_TRM + 1 /*lower bound should start at one more than prevous terms upper bound*/) AND RD.TermsToDate
ORDER BY
       RD.BF_SSN,
       RD.LN_SEQ,
       RD.LN_GRD_RPS_SEQ

DECLARE @FullPayment MONEY
SELECT 
	@FullPayment = ISNULL(SUM(MonthlyPayment), 0)
FROM #ReportData

DECLARE @MaxPayment MONEY
SELECT
	@MaxPayment = -(ISNULL(SUM(LN90.LA_FAT_NSI), 0) + ISNULL(SUM(LN90.LA_FAT_LTE_FEE), 0) + ISNULL(SUM(LN90.LA_FAT_ILG_PRI), 0) + ISNULL(SUM(LN90.LA_FAT_CUR_PRI), 0))
FROM
	LN65_LON_RPS LN65
	INNER JOIN LN90_FIN_ATY LN90
		ON LN90.BF_SSN = LN65.BF_SSN AND LN90.LN_SEQ = LN65.LN_SEQ 
WHERE
	LN65.BF_SSN = @Ssn
	AND
	LN90.LC_STA_LON90 = 'A'
	AND 
	LN90.PC_FAT_TYP = '10' 
	AND 
	LN90.PC_FAT_SUB_TYP = '10'
	AND
	NULLIF(LN90.LC_FAT_REV_REA, '') IS NULL --can't have a reversal reason
	AND
	LN90.LD_FAT_EFF BETWEEN DATEADD(YEAR, -1, GETDATE()) AND GETDATE()


SELECT
	CAST(CASE 
		WHEN @MaxPayment = 0 AND @FullPayment = 0 THEN 0
		WHEN @MaxPayment >= @FullPayment THEN 1
		ELSE 0 
	END AS BIT)


RETURN 0