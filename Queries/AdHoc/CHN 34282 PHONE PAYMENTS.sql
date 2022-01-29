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
		
	GROUP BY
		MONTH(NF_ONL_PAY_DTS), 
		YEAR(NF_ONL_PAY_DTS)
	ORDER BY 
		YEAR(NF_ONL_PAY_DTS),
		MONTH(NF_ONL_PAY_DTS)
')



SELECT 
month(ProcessedDate), YEAR(ProcessedDate), count(*) FROM CLS..CheckByPhone WHERE YEAR(ProcessedDate) >= 2017 group by month(ProcessedDate), YEAR(ProcessedDate) order by YEAR(ProcessedDate) ,month(ProcessedDate)
