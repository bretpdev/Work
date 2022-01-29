--get last updated datetime stamp:
SELECT 'LN64' AS [TABLE], * FROM OPENQUERY (DUSTER,'
	SELECT 
		MAX(LF_CRT_DTS_MR64) AS CreateDate
	FROM
		OLWHRM1.MR64_BR_TAX
')

UNION ALL

SELECT 'LN65' AS [TABLE], * FROM OPENQUERY (DUSTER,'
	SELECT 
		MAX(WF_CRT_DTS_MR65) AS CreateDate
	FROM
		OLWHRM1.MR65_MSC_TAX_RPT
');



--updated AES criteria
SELECT * FROM OPENQUERY (DUSTER,'
	SELECT 
		*
	FROM
		OLWHRM1.MR64_BR_TAX
	WHERE
		LF_TAX_YR = ''2017''
		AND DAYS(LD_INT_PD_RPT_BR) = DAYS(''2018-01-02'')
');

SELECT * FROM OPENQUERY (DUSTER,'
	SELECT 
		*
	FROM
		OLWHRM1.MR65_MSC_TAX_RPT
	WHERE
		LF_TAX_YR = ''2017''
		AND DAYS(LD_INT_PD_RPT_BR) = DAYS(''2018-01-02'')--this criteria is invalid. Candice checking with AES.
');
