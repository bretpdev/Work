USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[LT_TSXXBAPIDR_Loans]    Script Date: X/X/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[LT_TSXXBAPIDR_Loans]
	@AccountNumber		char(XX)
AS
BEGIN
DECLARE @ROWS INT = X

-- drop temp table to if it exist
IF OBJECT_ID('tempdb..#RD') IS NOT NULL DROP TABLE #RD

-- create temp table to hold LEGEND data
CREATE TABLE #RD
(
		SEQ INT,
		LN_SEQ INT,
		LN_GRD_RPS_SEQ INT,
		Label VARCHAR(XXX), 
		LD_LON_X_DSB DATE,
		LA_LON_AMT_GTR DECIMAL(XX,X),
		LA_CUR_PRI DECIMAL(XX,X),
		LR_ITR DECIMAL(X,X),
		LC_TYP_SCH_DIS VARCHAR(X),
		LA_TOT_RPD_DIS DECIMAL(XX,X),
		LA_ANT_CAP DECIMAL(XX,X),
		DueDate DATE,
		LN_RPS_TRM INT,
		LA_RPS_ISL DECIMAL(XX,X),
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
			RD.LD_LON_X_DSB,
			RD.LA_LON_AMT_GTR,
			RD.LA_CUR_PRI,
			RD.LR_ITR,
			RD.LC_TYP_SCH_DIS,
			RD.LA_TOT_RPD_DIS,
			RD.LA_ANT_CAP,
			CONVERT(VARCHAR(XX), RD.LD_RPS_X_PAY_DU, XXX) AS [DueDate],
			RD.LN_RPS_TRM,
			RD.LA_RPS_ISL,
			NULL
		FROM 
			OPENQUERY
			(	LEGEND, 
				''SELECT
					ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ, LNXX.LN_RPS_SEQ ORDER BY LNXX.LN_GRD_RPS_SEQ) AS SEQ,
					LNXX.LN_SEQ,
					LNXX.LN_GRD_RPS_SEQ,
					LNXX.IC_LON_PGM,
					LNXX.LD_LON_X_DSB,
					LNXX.LA_LON_AMT_GTR,
					LNXX.LA_CUR_PRI,
					LNXX.LR_ITR,
					LNXX.LC_TYP_SCH_DIS,
					LNXX.LA_TOT_RPD_DIS,
					LNXX.LA_ANT_CAP,
					RSXX.LD_RPS_X_PAY_DU,
					LNXX.LN_RPS_TRM,
					LNXX.LA_RPS_ISL
				FROM
					PKUB.PDXX_PRS_NME PDXX
					INNER JOIN PKUB.LNXX_LON LNXX
						ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					INNER JOIN
					(
						SELECT
							LNXX.BF_SSN,
							LNXX.LN_SEQ,
							LNXX.LR_ITR,
							ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ ORDER BY LD_STA_LONXX DESC) AS SEQ
						FROM	
							PKUB.LNXX_INT_RTE_HST LNXX
							INNER JOIN PKUB.PDXX_PRS_NME PDXX ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						WHERE
							LNXX.LC_STA_LONXX = ''''A''''
							AND
							PDXX.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
					) LNXX ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.SEQ = X
					INNER JOIN PKUB.LNXX_LON_RPS LNXX 
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
						AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					INNER JOIN PKUB.RSXX_BR_RPD RSXX
						ON RSXX.BF_SSN = LNXX.BF_SSN
						AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
				WHERE 
					PDXX.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
				AND LNXX.LC_STA_LONXX = ''''A''''
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
		SEQ = X -- start with sequence X
	
	UNION ALL
	
	SELECT
		RD.SEQ, 
		RD.LN_SEQ, 
		RD.LN_GRD_RPS_SEQ,
		RD.LN_RPS_TRM, 
		S.TermsToDate + RD.LN_RPS_TRM -- add terms for current record to running total
	FROM
		SUMS S
		INNER JOIN #RD RD ON RD.SEQ = S.SEQ + X AND RD.LN_SEQ = S.LN_SEQ
)
UPDATE
	RD
SET
	RD.TermsToDate = S.TermsToDate,
	RD.DueDate = DATEADD(MONTH, S.TermsToDate, RD.DueDate)
FROM 
	SUMS S
	INNER JOIN #RD RD ON RD.SEQ = S.SEQ AND RD.LN_SEQ = S.LN_SEQ
WHERE
	S.SEQ != X -- don't change starting due date

-- select data for letter
SELECT
	Label AS [Loan Program], 
	CONVERT(VARCHAR(XX) ,LD_LON_X_DSB, XXX) AS [First Disbursement Date],
	LA_LON_AMT_GTR AS [Original Balance],
	LA_CUR_PRI AS [Current Principal],
	LR_ITR AS [Interest Rate],
	LC_TYP_SCH_DIS AS [Schedule Type],
	LA_TOT_RPD_DIS AS [Total Repay Amount],
	LA_ANT_CAP AS [Anticipated Cap],
	CONVERT(VARCHAR(XX), DueDate, XXX) AS [Due Date],
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

IF @ROWS = X 
	BEGIN
		RAISERROR('[dbo].[LT_TSXXBAPIDR_Loans] - No data returned for AccountNumber %s',XX,X, @AccountNumber)
	END

END