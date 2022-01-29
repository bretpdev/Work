select *
from openquery
(legend, 

'SELECT
	CONCAT(A.WX_LON_STA, A.WX_LON_SUB_STA) Status,  
	CASE 
		WHEN B.WN_DAY_DLQ_ISL < X  THEN ''A. CURRENT''
		WHEN B.WN_DAY_DLQ_ISL < XX THEN ''B. X-XX DAYS DELINQUENT''
		WHEN B.WN_DAY_DLQ_ISL > XX THEN ''C. XX> DAYS DELINQUENT''
	END AS DEL_Status,
	
	COUNT(A.LN_SEQ) Loan_Count,
	SUM(B.WA_CUR_PRI) CPB
	,wd_crt_mrXa
FROM  PKUR.MRXC_MR_LON_MTH_XX A
INNER JOIN PKUR.MRXA_LON_MTH_SSH_XX B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN PKUR.MRXB_MR_LON_MTH_XX C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
WHERE 
--WD_CRT_MRXA = LAST_DAY(CURRENT DATE - X MONTHS) AND 
B.WA_CUR_PRI <> X
GROUP BY 
	CONCAT(A.WX_LON_STA, A.WX_LON_SUB_STA), 
	CASE
		WHEN B.WN_DAY_DLQ_ISL < X THEN ''A. CURRENT''
		WHEN B.WN_DAY_DLQ_ISL < XX THEN ''B. X-XX DAYS DELINQUENT''
		WHEN B.WN_DAY_DLQ_ISL > XX THEN ''C. XX> DAYS DELINQUENT''
	END
	,wd_crt_mrXa
	')


select * from openquery(legend,'select distinct WD_CRT_MRXA, LAST_DAY(CURRENT DATE - X MONTHS) as where_criteria from PKUR.MRXA_LON_MTH_SSH_XX');


--select * from openquery(legend,'select count(*) from PKUR.MRXC_MR_LON_MTH_XX');
--select * from openquery(legend,'select count(*) from PKUR.MRXA_LON_MTH_SSH_XX');
--select * from openquery(legend,'select count(*) from PKUR.MRXB_MR_LON_MTH_XX');
