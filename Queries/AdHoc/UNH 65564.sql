
--select * into #credit from openquery(duster, 
--'
--select 
--	bf_ssn, 
--	ln_seq, 
--	min(LD_RPT_CRB) as date_rpt 
--from 
--	olwhrm1.LN56_LON_CRB_RPT
--where
--	LC_RPT_STA_CRB IN (''80'',''81'',''82'',''83'',''84'') 
--group by 
--	bf_ssn,
--	 ln_seq ')

--select * into #bors from openquery(duster,
--'
--SELECT DISTINCT
--	DF_PRS_ID,
--	DM_PRS_1,
--	DM_PRS_LST,
--	LN_SEQ,
--	LD_LON_ACL_ADD,
--	LD_LON_EFF_ADD
--FROM
--	OLWHRM1.LN10_LON LN10
--	INNER JOIN OLWHRM1.PD10_PRS_NME PD10
--		ON PD10.DF_PRS_ID = LN10.BF_SSN
--')

SELECT DISTINCT
	B.DF_PRS_ID AS SSN,
	LTRIM(RTRIM(B.DM_PRS_1)) + ' '+ LTRIM(RTRIM(B.DM_PRS_LST)) AS [Name],
	B.LN_SEQ AS LOAN_SEQ,
	B.LD_LON_ACL_ADD AS ACTUAL_LOAN_ADD_DATE,
	B.LD_LON_EFF_ADD AS EFFECTIVE_LOAN_ADD_DATE,
	[PriorServicerFirstUnpaidInstall] AS NEXT_PAY_DUE,
	c.date_rpt as date_reported_to_credit,
	CASE 
		WHEN C.date_rpt < [PriorServicerFirstUnpaidInstall] THEN 'Y' 
		WHEN [PriorServicerFirstUnpaidInstall] IS NULL THEN 'N/A'
		ELSE 'N' 
	END  REPORTED_BEFORE_DATE
	
FROM 
	[EA27_BANA].[dbo].[_03PaymentDataRecord] PR
	INNER JOIN #bors B
		ON B.DF_PRS_ID = PR.BORROWERSSN
	LEFT JOIN #credit C  
		ON C.BF_SSN = PR.BORROWERSSN
		AND C.LN_SEQ = B.LN_SEQ

UNION ALL

SELECT DISTINCT
	B.DF_PRS_ID AS SSN,
	LTRIM(RTRIM(B.DM_PRS_1)) + ' '+ LTRIM(RTRIM(B.DM_PRS_LST)) AS [Name],
	B.LN_SEQ AS LOAN_SEQ,
	B.LD_LON_ACL_ADD AS ACTUAL_LOAN_ADD_DATE,
	B.LD_LON_EFF_ADD AS EFFECTIVE_LOAN_ADD_DATE,
	[PriorServicerFirstUnpaidInstall] AS NEXT_PAY_DUE,
	c.date_rpt as date_reported_to_credit,
	CASE 
		WHEN C.date_rpt < [PriorServicerFirstUnpaidInstall] THEN 'Y' 
		WHEN [PriorServicerFirstUnpaidInstall] IS NULL THEN 'N/A'
		ELSE 'N' 
	END  REPORTED_BEFORE_DATE
	
FROM 
	[EA27_BANA_1].[dbo].[_03PaymentDataRecord] PR
	INNER JOIN #bors B
		ON B.DF_PRS_ID = PR.BORROWERSSN
	LEFT JOIN #credit C  
		ON C.BF_SSN = PR.BORROWERSSN
		AND C.LN_SEQ = B.LN_SEQ

UNION ALL

SELECT DISTINCT
	B.DF_PRS_ID AS SSN,
	LTRIM(RTRIM(B.DM_PRS_1)) + ' '+ LTRIM(RTRIM(B.DM_PRS_LST)) AS [Name],
	B.LN_SEQ AS LOAN_SEQ,
	B.LD_LON_ACL_ADD AS ACTUAL_LOAN_ADD_DATE,
	B.LD_LON_EFF_ADD AS EFFECTIVE_LOAN_ADD_DATE,
	[PriorServicerFirstUnpaidInstall] AS NEXT_PAY_DUE,
	c.date_rpt as date_reported_to_credit,
	CASE 
		WHEN C.date_rpt < [PriorServicerFirstUnpaidInstall] THEN 'Y' 
		WHEN [PriorServicerFirstUnpaidInstall] IS NULL THEN 'N/A'
		ELSE 'N' 
	END  REPORTED_BEFORE_DATE
	
FROM 
	[EA27_BANA_2].[dbo].[_03PaymentDataRecord] PR
	INNER JOIN #bors B
		ON B.DF_PRS_ID = PR.BORROWERSSN
	LEFT JOIN #credit C  
		ON C.BF_SSN = PR.BORROWERSSN
		AND C.LN_SEQ = B.LN_SEQ

UNION ALL

SELECT DISTINCT
	B.DF_PRS_ID AS SSN,
	LTRIM(RTRIM(B.DM_PRS_1)) + ' '+ LTRIM(RTRIM(B.DM_PRS_LST)) AS [Name],
	B.LN_SEQ AS LOAN_SEQ,
	B.LD_LON_ACL_ADD AS ACTUAL_LOAN_ADD_DATE,
	B.LD_LON_EFF_ADD AS EFFECTIVE_LOAN_ADD_DATE,
	[PriorServicerFirstUnpaidInstall] AS NEXT_PAY_DUE,
	c.date_rpt as date_reported_to_credit,
	CASE 
		WHEN C.date_rpt < [PriorServicerFirstUnpaidInstall] THEN 'Y' 
		WHEN [PriorServicerFirstUnpaidInstall] IS NULL THEN 'N/A'
		ELSE 'N' 
	END  REPORTED_BEFORE_DATE
	
FROM 
	[EA27_BANA_3].[dbo].[_03PaymentDataRecord] PR
	INNER JOIN #bors B
		ON B.DF_PRS_ID = PR.BORROWERSSN
	LEFT JOIN #credit C  
		ON C.BF_SSN = PR.BORROWERSSN
		AND C.LN_SEQ = B.LN_SEQ


UNION ALL

SELECT DISTINCT
	B.DF_PRS_ID AS SSN,
	LTRIM(RTRIM(B.DM_PRS_1)) + ' '+ LTRIM(RTRIM(B.DM_PRS_LST)) AS [Name],
	B.LN_SEQ AS LOAN_SEQ,
	B.LD_LON_ACL_ADD AS ACTUAL_LOAN_ADD_DATE,
	B.LD_LON_EFF_ADD AS EFFECTIVE_LOAN_ADD_DATE,
	[PriorServicerFirstUnpaidInstall] AS NEXT_PAY_DUE,
	c.date_rpt as date_reported_to_credit,
	CASE 
		WHEN C.date_rpt < [PriorServicerFirstUnpaidInstall] THEN 'Y' 
		WHEN [PriorServicerFirstUnpaidInstall] IS NULL THEN 'N/A'
		ELSE 'N' 
	END  REPORTED_BEFORE_DATE
	
FROM 
	[EA27_BANA_4_SmallFile].[dbo].[_03PaymentDataRecord] PR
	INNER JOIN #bors B
		ON B.DF_PRS_ID = PR.BORROWERSSN
	LEFT JOIN #credit C  
		ON C.BF_SSN = PR.BORROWERSSN
		AND C.LN_SEQ = B.LN_SEQ

order by 
	REPORTED_BEFORE_DATE desc