DECLARE @start DATE = 'XX/XX/XXXX'
DECLARE @end DATE = 'XX/XX/XXXX'

/************ FIRST TIME *******************/
SELECT DISTINCT
	APP.application_id
	,BOR.SSN
	,PDXX.DM_PRS_LST
	,PDXX.DM_PRS_X
	,PDXX.DM_PRS_MID
	,PDXX.DM_PRS_LST_SFX
	,LNXX.LN_SEQ
	,APP.[received_date] AS Received_Date
	,'First Time' AS Category
INTO
	#FIRST_TIME
FROM
	(
		SELECT
			COUNT(LN_.application_id) AS CountApp, 
			LN_.application_id
		FROM 
			[Income_Driven_Repayment].[dbo].[Loans] LN_
 			INNER JOIN [Income_Driven_Repayment].[dbo].[Applications] APP_
				ON LN_.application_id = APP_.application_id
		WHERE 
			CAST(APP_.[received_date] AS DATE) BETWEEN CAST(@start AS DATE) AND CAST(@end AS DATE)
		GROUP BY 
			LN_.application_id
		HAVING 
			COUNT(LN_.application_ID) = X
	) FirstTime
	INNER JOIN [Income_Driven_Repayment]..Loans LN
		ON LN.application_id = FirstTime.application_id
	INNER JOIN [Income_Driven_Repayment]..Borrowers BOR
		ON BOR.borrower_id = LN.borrower_id
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = BOR.SSN 
		AND LNXX.LN_SEQ = LN.loan_seq
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN [Income_Driven_Repayment].[dbo].[Applications] APP
		ON LN.application_id = APP.application_id


/************ RENEWAL *******************/
SELECT DISTINCT
	APP.application_id
	,BOR.SSN
	,PDXX.DM_PRS_LST
	,PDXX.DM_PRS_X
	,PDXX.DM_PRS_MID
	,PDXX.DM_PRS_LST_SFX
	,LNXX.LN_SEQ
	,APP.[received_date] AS Received_Date
	,'Renewal' AS Category
INTO
	#RENEWAL
FROM
	[Income_Driven_Repayment].[dbo].[Applications] APP
	INNER JOIN [Income_Driven_Repayment]..Loans LN
		ON LN.application_id = APP.application_id
	INNER JOIN [Income_Driven_Repayment]..Borrowers BOR
		ON BOR.borrower_id = LN.borrower_id
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = BOR.SSN
		AND LNXX.LN_SEQ = LN.loan_seq
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	APP.Repayment_Plan_Reason_id ='X'
  	AND CAST(APP.[received_date] AS DATE) BETWEEN CAST(@start AS DATE) AND CAST(@end AS DATE)


/************ removing duplicates from RENEWAL that are in FIRST TIME *******************/

--SELECT * FROM #FIRST_TIME ORDER BY application_id, SSN, LN_SEQ
--SELECT * FROM #RENEWAL ORDER BY application_id, SSN, LN_SEQ

SELECT
	R.*
INTO
	#DUPES
FROM
	#RENEWAL R
	INNER JOIN #FIRST_TIME F
		ON R.application_id = F.application_id
		AND R.SSN = F.SSN
		AND R.LN_SEQ = F.LN_SEQ
		AND R.Received_Date = F.Received_Date

--SELECT * FROM #DUPES

SELECT
	R.*
INTO
	#RENEWAL_DEDUPE
FROM
	#RENEWAL R
	LEFT JOIN #DUPES D
		ON R.application_id = D.application_id
WHERE
	D.application_id IS NULL

--SELECT * FROM #RENEWAL_DEDUPE


/************ ADDING IDR PLAN *******************/

;WITH CTE AS 
(
	SELECT DISTINCT
		APP.application_id
		,RPT.repayment_plan
		,LN.loan_seq
	FROM
		[Income_Driven_Repayment].[dbo].[Applications] APP
		INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Selected] RPS
			ON APP.application_id = RPS.application_id
		INNER JOIN 
		(
			SELECT
				[repayment_plan_type_status_history_id]
				,[repayment_plan_type_status_mapping_id]
				,MAX(repayment_plan_type_id) AS repayment_plan_type_id
			FROM
				[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History]
			GROUP BY
				[repayment_plan_type_status_history_id]
				,[repayment_plan_type_status_mapping_id]
		) HIST
			ON RPS.repayment_plan_type_id = HIST.repayment_plan_type_id
		INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUBST
			ON HIST.repayment_plan_type_status_mapping_id = SUBST.repayment_plan_type_substatus_id
		INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Type] RT
			ON RPS.repayment_type_id = RT.repayment_type_id
		INNER JOIN [Income_Driven_Repayment]..Loans LN
			ON LN.application_id = APP.application_id
		INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type] RPT
			ON RPT.repayment_plan_type_id = RT.repayment_plan_type_id
	WHERE
		CAST(APP.[received_date] AS DATE) BETWEEN CAST(@start AS DATE) AND CAST(@end AS DATE)
	) 
SELECT 
	POP.*
	,CTE.repayment_plan
FROM 
	CTE
	INNER JOIN
	(
		SELECT * FROM #FIRST_TIME
		UNION
		SELECT * FROM #RENEWAL_DEDUPE
	)POP
		ON POP.application_id = CTE.application_id
		AND POP.LN_SEQ = CTE.loan_seq
ORDER BY
	application_id
	,LN_SEQ


DROP TABLE #DUPES
DROP TABLE #FIRST_TIME
DROP TABLE #RENEWAL
DROP TABLE #RENEWAL_DEDUPE