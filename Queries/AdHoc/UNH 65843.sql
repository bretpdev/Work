USE UDW;
GO

;WITH POP AS
(--borrowers with both IR and 51 queues active
	SELECT DISTINCT
		WQ20.BF_SSN
		--WQ20.WF_QUE,
		--WQ20.WF_SUB_QUE,
		--WQ20.WN_CTL_TSK,
		--WQ20.PF_REQ_ACT,
		--WQ20.WC_STA_WQUE20,
		--WQ20.WF_USR_ASN_BY_TSK
	FROM 
		WQ20_TSK_QUE WQ20
	WHERE
		WQ20.WF_QUE IN ('1R')
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')

	INTERSECT

	--51 CREDIT BUREAU TASKS
	SELECT DISTINCT
		WQ20.BF_SSN
		--WQ20.WF_QUE,
		--WQ20.WF_SUB_QUE,
		--WQ20.WN_CTL_TSK,
		--WQ20.PF_REQ_ACT,
		--WQ20.WC_STA_WQUE20,
		--WQ20.WF_USR_ASN_BY_TSK
	FROM 
		WQ20_TSK_QUE WQ20
	WHERE
		WQ20.WF_QUE IN ('51')
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
		AND WQ20.WF_USR_ASN_BY_TSK NOT LIKE ('UT%')
),
UNIONS AS 
(
	SELECT DISTINCT
		WQ20.BF_SSN,
		WQ20.WF_QUE,
		WQ20.WF_SUB_QUE,
		WQ20.WN_CTL_TSK,
		WQ20.PF_REQ_ACT,
		WQ20.WC_STA_WQUE20,
		WQ20.WF_USR_ASN_BY_TSK
	FROM 
		WQ20_TSK_QUE WQ20
		INNER JOIN POP 
			ON WQ20.BF_SSN = POP.BF_SSN
	WHERE
		WQ20.WF_QUE IN ('1R')
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')

	UNION

	--51 CREDIT BUREAU TASKS
	SELECT DISTINCT
		WQ20.BF_SSN,
		WQ20.WF_QUE,
		WQ20.WF_SUB_QUE,
		WQ20.WN_CTL_TSK,
		WQ20.PF_REQ_ACT,
		WQ20.WC_STA_WQUE20,
		WQ20.WF_USR_ASN_BY_TSK
	FROM 
		WQ20_TSK_QUE WQ20
		INNER JOIN POP 
			ON WQ20.BF_SSN = POP.BF_SSN
	WHERE
		WQ20.WF_QUE IN ('51')
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
		AND WQ20.WF_USR_ASN_BY_TSK NOT LIKE ('UT%')
)
	SELECT
		*
	FROM
		UNIONS
	ORDER BY
		BF_SSN,
		WN_CTL_TSK






/*SCRAP*/

--SELECT TOP 10 *
--FROM
--	LN85_LON_ATY


--select * from LK10_LS_CDE_LKP order by PM_ATR


--SELECT * FROM WQ20_TSK_QUE
--WHERE WF_QUE = '1R' 
--ORDER BY WN_CTL_TSK


--SELECT * FROM WQ20_TSK_QUE
--WHERE WF_QUE = '51' 
--	AND WF_USR_ASN_BY_TSK NOT LIKE ('UT%')
--ORDER BY WN_CTL_TSK



--SELECT TOP 1000 * 
--FROM
--	WQ20_TSK_QUE T1R
--	INNER JOIN WQ20_TSK_QUE T51
--		ON T1R.WN_CTL_TSK = T51.WN_CTL_TSK
--		--ON T1R.PF_REQ_ACT = T51.PF_REQ_ACT

--		--ON T1R.WX_MSG_1_TSK = T51.WX_MSG_1_TSK
--		--OR T1R.WX_MSG_2_TSK = T51.WX_MSG_2_TSK
--WHERE
--	T1R.WF_QUE = '1R' 
--	AND T51.WF_QUE = '51' 
--	AND T51.WF_USR_ASN_BY_TSK NOT LIKE ('UT%')
--	--AND LTRIM(RTRIM(COALESCE(T1R.WX_MSG_1_TSK,''))) != '' 
--	--AND LTRIM(RTRIM(COALESCE(T1R.WX_MSG_2_TSK,''))) != '' 
--	--AND LTRIM(RTRIM(COALESCE(T51.WX_MSG_1_TSK,''))) != '' 
--	--AND LTRIM(RTRIM(COALESCE(T51.WX_MSG_2_TSK,''))) != '' 







--SELECT TOP 1000 * 
--FROM
--	WQ15_TSK_SUB_QUE T1R
--	INNER JOIN WQ15_TSK_SUB_QUE T51
--		ON T1R.WF_SUB_QUE = T51.WF_SUB_QUE
--WHERE
--	T1R.WF_QUE = '1R' 
--	AND T51.WF_QUE = '51' 
--	AND T51.WF_USR_ASN_BY_TSK NOT LIKE ('UT%')







--SELECT * FROM WQ20_TSK_QUE
--WHERE WF_QUE = '51' 
--	AND WF_USR_ASN_BY_TSK NOT LIKE ('UT%')
--ORDER BY WN_CTL_TSK





----SELECT DISTINCT WF_USR_ASN_BY_TSK FROM UDW..WQ20_TSK_QUE
----WHERE WF_QUE = '51'
----SELECT DISTINCT WF_USR_ASN_BY_TSK FROM UDW..WQ20_TSK_QUE
----WHERE WF_QUE = '1R'



