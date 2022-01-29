DECLARE @start DATETIME = '2017-05-01'
DECLARE @end DATETIME = '2017-05-31'

DECLARE @BorrowerPop TABLE(SSN CHAR(9), LN_SEQ INT, LF_LON_CUR_OWN VARCHAR(8), Category VARCHAR(20))
INSERT INTO @BorrowerPop
SELECT DISTINCT
	B.SSN,
	LN10.LN_SEQ,
	LN10.LF_LON_CUR_OWN,
	'First Time' AS Category
FROM
	(
		SELECT 
			COUNT(L.application_id) AS CountApp, 
			L.application_id
		FROM 
			[IncomeBasedRepaymentUheaa].[dbo].[Loans] L
 			INNER JOIN [IncomeBasedRepaymentUheaa].[dbo].[Applications] A
				ON L.application_id = A.application_id
		WHERE 
			CAST(created_at AS DATE) BETWEEN @start AND @end
		GROUP BY 
			L.application_id
		HAVING 
			COUNT(L.application_ID) = 1
	) FirstTime
	INNER JOIN IncomeBasedRepaymentUheaa..Loans L
		ON L.application_id = FirstTime.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = B.SSN 
		AND LN10.LN_SEQ = L.loan_seq		

UNION ALL

SELECT DISTINCT
	B.SSN,
	LN10.LN_SEQ,
	LN10.LF_LON_CUR_OWN,
	'Renewal' AS Category
FROM
	[IncomeBasedRepaymentUheaa].[dbo].[Applications] A
	INNER JOIN IncomeBasedRepaymentUheaa..Loans L
		ON L.application_id = A.application_id
	INNER JOIN IncomeBasedRepaymentUheaa..Borrowers B
		ON B.borrower_id = L.borrower_id
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = B.SSN 
		AND LN10.LN_SEQ = L.loan_seq		
WHERE 
	A.Repayment_Plan_Reason_id IN ('2','3')
  	AND CAST(A.created_at AS DATE) BETWEEN @start AND @end

SELECT COUNT(DISTINCT SSN) AS BorrowerCount, LF_LON_CUR_OWN, Category FROM @BorrowerPop GROUP BY LF_LON_CUR_OWN, Category

SELECT * FROM @BorrowerPop ORDER BY Category, SSN, LN_SEQ

SELECT DISTINCT
	LN50.BF_SSN,
	LN50.LN_SEQ,
	DF10.LC_DFR_TYP,
	CASE WHEN LN50.LD_DFR_BEG >= @start AND LN50.LD_DFR_END <= @end THEN DATEDIFF(DAY, LN50.LD_DFR_BEG, LN50.LD_DFR_END)
			WHEN LN50.LD_DFR_BEG >= @start AND LN50.LD_DFR_END > @end THEN DATEDIFF(DAY, LN50.LD_DFR_BEG, @end)
			WHEN LN50.LD_DFR_BEG < @start AND LN50.LD_DFR_END <= @end THEN DATEDIFF(DAY, @start, LN50.LD_DFR_END)
			WHEN LN50.LD_DFR_BEG < @start AND LN50.LD_DFR_END > @end THEN DATEDIFF(DAY, @start, @end)
	END AS DefermentDays,
	BP.Category
FROM
	CDW..DF10_BR_DFR_REQ DF10
	INNER JOIN CDW..LN50_BR_DFR_APV LN50 ON LN50.BF_SSN = DF10.BF_SSN AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
	INNER JOIN @BorrowerPop BP ON BP.SSN = LN50.BF_SSN
WHERE
	DF10.LC_DFR_STA = 'A'
	AND DF10.LC_STA_DFR10 = 'A'
	AND LN50.LC_STA_LON50 = 'A'
	AND LN50.LC_DFR_RSP != '003' 
	AND
	(
		LN50.LD_DFR_BEG BETWEEN @start AND @end
		OR LN50.LD_DFR_END BETWEEN @start AND @end
	)
	

SELECT DISTINCT
	LN60.BF_SSN,
	LN60.LN_SEQ,
	FB10.LC_FOR_TYP,
	CASE WHEN LN60.LD_FOR_BEG >= @start AND LN60.LD_FOR_END <= @end THEN DATEDIFF(DAY, LN60.LD_FOR_BEG, LN60.LD_FOR_END)
			WHEN LN60.LD_FOR_BEG >= @start AND LN60.LD_FOR_END > @end THEN DATEDIFF(DAY, LN60.LD_FOR_BEG, @end)
			WHEN LN60.LD_FOR_BEG < @start AND LN60.LD_FOR_END <= @end THEN DATEDIFF(DAY, @start, LN60.LD_FOR_END)
			WHEN LN60.LD_FOR_BEG < @start AND LN60.LD_FOR_END > @end THEN DATEDIFF(DAY, @start, @end)
	END AS ForbearanceDays,
	BP.Category
FROM
	CDW..FB10_BR_FOR_REQ FB10
	INNER JOIN CDW..LN60_BR_FOR_APV LN60 ON LN60.BF_SSN = FB10.BF_SSN AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
	INNER JOIN @BorrowerPop BP ON BP.SSN = LN60.BF_SSN
WHERE
	FB10.LC_FOR_STA = 'A'
	AND FB10.LC_STA_FOR10 = 'A'
	AND LN60.LC_STA_LON60 = 'A'
	AND LN60.LC_FOR_RSP != '003' 
	AND
	(
		LN60.LD_FOR_BEG BETWEEN @start AND @end
		OR LN60.LD_FOR_END BETWEEN @start AND @end
	)
