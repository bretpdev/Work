/* --REMOVING INVALID DATA
SELECT 
	*
FROM 
	[EA27_BANA].[dbo].[CRITERIA_LOAN_SALE]
WHERE 
	Borrower_Benefit_Code = 'Borrow'
--2

DELETE FROM 
	[EA27_BANA].[dbo].[CRITERIA_LOAN_SALE]
WHERE 
	Borrower_Benefit_Code = 'Borrow'
--2
*/

;WITH CTE AS (
	--wave 1-3 data
	SELECT 
		'1-3' AS Wave
		,[Deal_ID]
		,[Borrower_SSN]
		,[Borrower_Last_Name]
		,[Borrower_First_Name]
		,[Commonline_Unique_ID]
		,[Loan_Guarantee_Date]
		,[Borrower_Benefit_Code]
		,[Plan_Applied_Date]
		,[On_Time_Payments]
		,[Interest_Status]
		,[Interest_Applied]
		,[Rebate_Status]
		,[Rebate_Applied]
		,[Rebate_Amount]
		,[Disqualification_Date]
		,[ACH_Status]
		,[SPC_Status]
		,[Loan_Ident]
		,[Subsidy_Cd]
		,[Reduced_Interest_Rate]
		,[Plan_Seq]
		,[Program_End]
		,[Incent_Plan_Opt]
		,[LON_IDENT]
		,[UHEAA_CODE]
		,[CSVID] 
	FROM 
		[EA27].[dbo].[_CSVSOURCE_NH_27479] --290453
		
	UNION ALL
		
	--wave 4 data
	SELECT 
		'4' AS Wave
		,CLS.* 
		,CLS.Loan_Ident	AS [LON_IDENT]
		,CASE
			WHEN BORROWER_BENEFIT_CODE IN (0290,0835) THEN 'BI1'
			WHEN BORROWER_BENEFIT_CODE IN (1560,1565,1717) THEN 'BI2'
			WHEN BORROWER_BENEFIT_CODE IN (0007,1716) THEN 'BI3'
			WHEN BORROWER_BENEFIT_CODE IN (1000,1520,4400,5101,6742,6749) THEN 'BI4'
			WHEN BORROWER_BENEFIT_CODE IN (1570,1580,6720) THEN 'BI5'
			WHEN BORROWER_BENEFIT_CODE = 1706 THEN 'BI6'
			WHEN BORROWER_BENEFIT_CODE IN (0830,1715) THEN 'BI7'
			WHEN BORROWER_BENEFIT_CODE IN (2585,2590) THEN 'BI8'
			WHEN BORROWER_BENEFIT_CODE IN (6740,6741) THEN 'BI9'
			WHEN BORROWER_BENEFIT_CODE IN (6744,6745) THEN 'BIA'
			WHEN BORROWER_BENEFIT_CODE IN (6746,6747) THEN 'BIB'
			WHEN BORROWER_BENEFIT_CODE = 1710 THEN 'BIC'
			WHEN BORROWER_BENEFIT_CODE IN (0006,1020) THEN 'BID'
			WHEN BORROWER_BENEFIT_CODE IN (0230,0800,1010,1530) THEN 'BIE'
			WHEN BORROWER_BENEFIT_CODE IN (2740,6748) THEN 'BR1'
			WHEN BORROWER_BENEFIT_CODE IN (2204,2205,2206,2207,2208,2550,2552,2555,2560,2565) THEN 'BT1'
			WHEN BORROWER_BENEFIT_CODE IN (2214,2215,2216,2217,2530,2533,2535,2540,2570,2573,2575,2580,7722,7723,7724,7725) THEN 'BT2'
			WHEN BORROWER_BENEFIT_CODE IN (3330,3335,3340) THEN 'BT3'
			ELSE ''
		END AS UHEAA_CODE
		,NULL AS [CSVID] 
	FROM 
		[EA27_BANA].[dbo].[CRITERIA_LOAN_SALE] CLS --9956
)
SELECT DISTINCT 
	CTE.Wave
	,A.DF_SPE_ACC_ID	
	,A.SSN 			
	,A.LoanSeq 	
	,A.Tier	
	,A.LF_GTR_RFR_XTN
	,CTE.LON_IDENT
	,A.CLUID
	,CTE.COMMONLINE_UNIQUE_ID
	,CTE.UHEAA_CODE AS BorrowerBenefitType
	,CTE.INTEREST_STATUS
	,CTE.REBATE_STATUS
	,A.PM_BBS_PGM
	,A.LD_LON_BBT_BEG
	,A.LD_LON_EFF_ADD
	,NULL AS CorrectedStatus  
	,NULL AS PCVPayments 		
	,NULL AS DisqualDate 		
	,NULL AS DisqualReason 		
	,NULL AS Comment 	
FROM OPENQUERY 
	(DUSTER,'
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID	
			,LN10.BF_SSN 			AS SSN 			
			,LN10.LN_SEQ 			AS LoanSeq 	
			,LN10.LF_GTR_RFR_XTN 
			,LN10A.CLUID
			,LN55A.LF_LON_BBS_TIR	AS Tier
			,LN55.PM_BBS_PGM
			,LN55.LD_LON_BBT_BEG
			,LN10.LD_LON_EFF_ADD
		FROM
			OLWHRM1.LN10_LON LN10
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN
			(
				SELECT DISTINCT
					LN10.BF_SSN
					,LN10.LN_SEQ
					,CONCAT(LN10.LF_LON_ALT, LPAD(LN10.LN_LON_ALT_SEQ,2,0)) AS CLUID
				FROM
					OLWHRM1.LN10_LON LN10
			)LN10A
				ON LN10.BF_SSN = LN10A.BF_SSN
				AND LN10.LN_SEQ = LN10A.LN_SEQ
			INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
				ON LN10.BF_SSN = LN60.BF_SSN
				AND LN10.LN_SEQ = LN60.LN_SEQ
			INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
				ON LN10.BF_SSN = FB10.BF_SSN
			INNER JOIN OLWHRM1.LN55_LON_BBS_TIR LN55
				ON LN10.BF_SSN = LN55.BF_SSN
				AND LN10.LN_SEQ = LN55.LN_SEQ
			INNER JOIN 
			(
				SELECT DISTINCT
					LN55.BF_SSN
					,LN55.LN_SEQ
					,MAX(LN55.LF_LON_BBS_TIR) AS LF_LON_BBS_TIR
				FROM
					OLWHRM1.LN55_LON_BBS_TIR LN55
				GROUP BY 
					LN55.BF_SSN
					,LN55.LN_SEQ
			)LN55A
				ON LN55.BF_SSN = LN55A.BF_SSN
				AND LN55.LN_SEQ = LN55A.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 = ''R''
			AND LN10.LA_CUR_PRI >= 0
			AND LN10.LF_LON_CUR_OWN LIKE ''829769%''
			AND LN60.LC_STA_LON60 = ''A''
			AND FB10.LC_FOR_STA = ''A''
			AND FB10.LC_STA_FOR10 = ''A''
			AND FB10.LC_FOR_TYP = ''17''
			AND LN55.LC_STA_LN55 = ''A''
			AND LN55.PM_BBS_PGM IN (''BT1'',''BT2'',''BT3'')
			AND LN55.LC_LON_BBT_STA = ''D''
			AND LN55.LD_LON_BBT_STA BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_APL
			AND LN55.LD_LON_BBT_STA >= LN60.LD_FOR_BEG 
			AND LN55.LD_LON_BBT_STA <= LN60.LD_FOR_APL
			AND LN55.LC_LON_BBT_DSQ_REA = ''04''
			--AND LN55.LD_LON_BBT_BEG > LN10.LD_LON_EFF_ADD
	') A
INNER JOIN 
	CTE
	ON A.SSN = CTE.Borrower_SSN
	--AND A.LF_GTR_RFR_XTN = B.LON_IDENT
	AND A.CLUID = CTE.Commonline_Unique_ID
