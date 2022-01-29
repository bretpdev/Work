USE CDW
GO

SELECT DISTINCT
	DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	MAX(CASE 
		WHEN WC_STA_WQUEXX = 'U' THEN WF_CRT_DTS_WQXX
	END) OVER (PARTITION BY PDXX.DF_SPE_ACC_ID) AS CREATE_DATE,
	MAX(CASE	
		WHEN WC_STA_WQUEXX = 'C' THEN WQXX.WF_CRT_DTS_WQXX
	END) OVER (PARTITION BY PDXX.DF_SPE_ACC_ID) AS COMPLETE_DATE,
	MAX(CASE	
		WHEN WC_STA_WQUEXX = 'C' THEN WQXX.WF_USR_ASN_TSK
	END) OVER (PARTITION BY PDXX.DF_SPE_ACC_ID)  AS USER_COMPLETED_TASK,
	CASE WHEN AYXXD.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS HAS_DFDNY,
	CASE WHEN AYXXD.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS HAS_DFAPV
FROM
	CDW..WQXX_TSK_QUE_HST WQXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = WQXX.BF_SSN
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXXD
		ON AYXXD.BF_SSN = WQXX.BF_SSN
		AND AYXXD.PF_REQ_ACT = 'DFDNY'
		AND AYXXD.LC_STA_ACTYXX = 'A'
		AND CAST(AYXXD.LD_ATY_REQ_RCV AS DATE) >= CAST(WQXX.WF_CRT_DTS_WQXX AS DATE)
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXXA
		ON AYXXA.BF_SSN = WQXX.BF_SSN
		AND AYXXA.PF_REQ_ACT = 'DFAPV'
		AND AYXXA.LC_STA_ACTYXX = 'A'
		AND CAST(AYXXA.LD_ATY_REQ_RCV AS DATE) >= CAST(WQXX.WF_CRT_DTS_WQXX AS DATE)
WHERE
	WF_QUE = 'SX'
	AND WF_SUB_QUE = 'XX'
	AND WF_CRT_DTS_WQXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND WQXX.PF_REQ_ACT = 'LSXXX'