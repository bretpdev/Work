SELECT * FROM OPENQUERY(LEGEND,
'
	SELECT 
		MONTH(NF_ONL_PAY_DTS) AS MON, 
		YEAR(NF_ONL_PAY_DTS) AS Y,
		COUNT(*) AS C 
	FROM 
		WEBFLS1.RM03_ONL_PAY 
	WHERE 
		YEAR(NF_ONL_PAY_DTS) >=2017 
		AND NF_USR_CRT_ONL_PAY NOT LIKE ''UT%''
	GROUP BY
		MONTH(NF_ONL_PAY_DTS), 
		YEAR(NF_ONL_PAY_DTS)
	ORDER BY 
		YEAR(NF_ONL_PAY_DTS),
		MONTH(NF_ONL_PAY_DTS)
')