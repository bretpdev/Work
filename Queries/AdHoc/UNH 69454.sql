
--Please return any tasks that have been closed out of the R3/01 queue from August 1, 2020 to November 10, 2020.
--I need the following information returned.

--SSN, Date of payment that caused a credit, amount of credit, Date credit refunded. 


SELECT DISTINCT
	WQ21.BF_SSN AS SSN,
	CAST(WQ21.WF_CRT_DTS_WQ20 AS DATE) AS [Date of Payment],
	--wq21.WF_CRT_DTS_WQ21,
	LN90.LA_FAT_CUR_PRI AS [Amount of Credit],
	CAST(LN90.LD_FAT_PST AS DATE) AS [Date Credit Refunded]
FROM
	UDW..WQ21_TSK_QUE_HST WQ21
	LEFT JOIN UDW..LN90_FIN_ATY LN90
		ON WQ21.BF_SSN = LN90.BF_SSN
		AND CAST(LN90.LD_FAT_APL AS DATE) BETWEEN wq21.WF_CRT_DTS_WQ20 AND wq21.WF_CRT_DTS_WQ21 
		AND LN90.PC_FAT_TYP = '20' --refund

WHERE
	WQ21.WF_QUE = 'R3'
	AND WQ21.WF_SUB_QUE = '01'
	AND CAST(WQ21.WF_LST_DTS_WQ20 AS DATE) BETWEEN '2020-08-01' AND '2020-11-10'
	and wq21.WC_STA_WQUE20 = 'C' --completed task
	