/*BANA added to IBR since transfer*/
SELECT * FROM OPENQUERY (DUSTER,'
	SELECT DISTINCT
		LN10.BF_SSN
		,LN10.LN_SEQ
		,LN10.LC_STA_LON10
		,LN10.LA_CUR_PRI
		,LN10.LF_LON_CUR_OWN
		,LN10.LD_LON_ACL_ADD
		,LN65_IB.LD_CRT_LON65		AS LD_CRT_LON65
		,LN65_IB.LC_TYP_SCH_DIS		AS LC_TYP_SCH_DIS
		,LN65_IB.LC_STA_LON65		AS LC_STA_LON65
		,LN65_PRIOR.LD_CRT_LON65 	AS PRIOR_LD_CRT_LON65
		,LN65_PRIOR.LC_TYP_SCH_DIS	AS PRIOR_LC_TYP_SCH_DIS
		,LN65_PRIOR.LC_STA_LON65	AS PRIOR_LC_STA_LON65
	FROM
		OLWHRM1.LN10_LON LN10
		LEFT OUTER JOIN 
			(
				SELECT
					BF_SSN
					,LN_SEQ
					,LD_CRT_LON65 
					,LC_TYP_SCH_DIS
					,LC_STA_LON65
				FROM
					OLWHRM1.LN65_LON_RPS
				WHERE
					LC_TYP_SCH_DIS = ''IB''
					AND LC_STA_LON65 = ''A'' 
			)LN65_IB
				ON LN10.BF_SSN = LN65_IB.BF_SSN
				AND LN10.LN_SEQ = LN65_IB.LN_SEQ
		LEFT OUTER JOIN 
			(
				SELECT
					BF_SSN
					,LN_SEQ
					,LD_CRT_LON65 
					,LC_TYP_SCH_DIS
					,LC_STA_LON65
				FROM
					OLWHRM1.LN65_LON_RPS
				WHERE
					LC_TYP_SCH_DIS = ''IB''
					AND LC_STA_LON65 != ''A''
			)LN65_PRIOR
				ON LN10.BF_SSN = LN65_PRIOR.BF_SSN
				AND LN10.LN_SEQ = LN65_PRIOR.LN_SEQ
	WHERE
		LN10.LC_STA_LON10 = ''R''
		AND LN10.LA_CUR_PRI > ''0''
		AND LN10.LF_LON_CUR_OWN LIKE ''829769%''
		AND LN65_IB.LD_CRT_LON65 > LN10.LD_LON_ACL_ADD
');

------------

/*Compass borrowers on IBR*/
SELECT * FROM OPENQUERY (DUSTER,'
	SELECT DISTINCT
		''on IBR'' AS COMPASS
		,LN10.BF_SSN
		,LN10.LN_SEQ
		,LN10.LC_STA_LON10
		,LN10.LA_CUR_PRI
		,LN10.LF_LON_CUR_OWN
		,LN65.LC_TYP_SCH_DIS
		,LN65.LC_STA_LON65
	FROM
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.LN65_LON_RPS LN65
			ON LN10.BF_SSN = LN65.BF_SSN
			AND LN10.LN_SEQ = LN65.LN_SEQ
	WHERE
		LN10.LC_STA_LON10 = ''R''
		AND LN10.LA_CUR_PRI > ''0''
		AND LN10.LF_LON_CUR_OWN LIKE ''829769%''
		AND LN65.LC_TYP_SCH_DIS IN (''IB'' , ''IL'')
		AND LN65.LC_STA_LON65 = ''A''
');

/*Compass borrowers in repayment*/
SELECT * FROM OPENQUERY (DUSTER,'
	SELECT DISTINCT
		''in repayment'' AS COMPASS
		,LN10.BF_SSN
		,LN10.LN_SEQ
		,LN10.LC_STA_LON10
		,LN10.LA_CUR_PRI
		,LN10.LF_LON_CUR_OWN
		,LN65.LC_TYP_SCH_DIS
		,LN65.LC_STA_LON65
	FROM
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.LN65_LON_RPS LN65
			ON LN10.BF_SSN = LN65.BF_SSN
			AND LN10.LN_SEQ = LN65.LN_SEQ
	WHERE
		LN10.LC_STA_LON10 = ''R''
		AND LN10.LA_CUR_PRI > ''0''
		AND LN10.LF_LON_CUR_OWN LIKE ''829769%''
		AND LN65.LC_STA_LON65 = ''A''
');


/*transfer on IBR*/
SELECT DISTINCT 
	'EA27_BANA' AS TRANSFER_ON_IBR
	,A.BorrowerSSN
	,A.loan_number
	,A.RepaymentPlanCode
	,B.PrincipalBalanceOutstanding
FROM 
	[EA27_BANA].[dbo].[_03PaymentDataRecord] A
	INNER JOIN [EA27_BANA].[dbo].[_07_08DisbClaimEnrollRecord] B
		ON A.loan_number = B.loan_number
		AND A.BorrowerSSN = B.BorrowerSSN
WHERE 
	A.RepaymentPlanCode IN ('IP','IB')
	AND B.PrincipalBalanceOutstanding > 0

UNION ALL

SELECT DISTINCT 
	'EA27_BANA_1' AS TRANSFER_ON_IBR
	,A.BorrowerSSN
	,A.loan_number
	,A.RepaymentPlanCode
	,B.PrincipalBalanceOutstanding
FROM 
	[EA27_BANA_1].[dbo].[_03PaymentDataRecord] A
	INNER JOIN [EA27_BANA_1].[dbo].[_07_08DisbClaimEnrollRecord] B
		ON A.loan_number = B.loan_number
		AND A.BorrowerSSN = B.BorrowerSSN
WHERE 
	A.RepaymentPlanCode IN ('IP','IB')
	AND B.PrincipalBalanceOutstanding > 0

UNION ALL

SELECT DISTINCT 
	'EA27_BANA_2' AS TRANSFER_ON_IBR
	,A.BorrowerSSN
	,A.loan_number
	,A.RepaymentPlanCode
	,B.PrincipalBalanceOutstanding
FROM 
	[EA27_BANA_2].[dbo].[_03PaymentDataRecord] A
	INNER JOIN [EA27_BANA_2].[dbo].[_07_08DisbClaimEnrollRecord] B
		ON A.loan_number = B.loan_number
		AND A.BorrowerSSN = B.BorrowerSSN
WHERE 
	A.RepaymentPlanCode IN ('IP','IB')
	AND B.PrincipalBalanceOutstanding > 0

/*transfer in repayment*/
SELECT DISTINCT 
	'EA27_BANA' AS TRANSFER_IN_REPAYMENT
	,A.BorrowerSSN
	,A.loan_number
	,A.LoanStatusCode
	,B.PrincipalBalanceOutstanding
FROM 
	[EA27_BANA].[dbo].[_01BorrowerRecord] A
	INNER JOIN [EA27_BANA].[dbo].[_07_08DisbClaimEnrollRecord] B
		ON A.loan_number = B.loan_number
		AND A.BorrowerSSN = B.BorrowerSSN
WHERE 
	A.LoanStatusCode IN ('30','33')
	AND B.PrincipalBalanceOutstanding > 0

UNION ALL

SELECT DISTINCT 
	'EA27_BANA_1' AS TRANSFER_IN_REPAYMENT
	,A.BorrowerSSN
	,A.loan_number
	,A.LoanStatusCode
	,B.PrincipalBalanceOutstanding
FROM 
	[EA27_BANA_1].[dbo].[_01BorrowerRecord] A
	INNER JOIN [EA27_BANA_1].[dbo].[_07_08DisbClaimEnrollRecord] B
		ON A.loan_number = B.loan_number
		AND A.BorrowerSSN = B.BorrowerSSN
WHERE 
	A.LoanStatusCode IN ('30','33')
	AND B.PrincipalBalanceOutstanding > 0

UNION ALL

SELECT DISTINCT 
	'EA27_BANA_2' AS TRANSFER_IN_REPAYMENT
	,A.BorrowerSSN
	,A.loan_number
	,A.LoanStatusCode
	,B.PrincipalBalanceOutstanding
FROM 
	[EA27_BANA_2].[dbo].[_01BorrowerRecord] A
	INNER JOIN [EA27_BANA_2].[dbo].[_07_08DisbClaimEnrollRecord] B
		ON A.loan_number = B.loan_number
		AND A.BorrowerSSN = B.BorrowerSSN
WHERE 
	A.LoanStatusCode IN ('30','33')
	AND B.PrincipalBalanceOutstanding > 0

