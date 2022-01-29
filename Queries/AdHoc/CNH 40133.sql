SELECT
	PDXX.DF_SPE_ACC_ID,
	WQXX.WF_CRT_DTS_WQXX AS TASK_UPDATED_DATE,
	WQXX.WC_STA_WQUEXX AS TASK_STATUS,
	WQXX.WF_USR_ASN_TSK AS USER_ASSIGNED_TO_TASK
FROM 
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..WQXX_TSK_QUE_HST WQXX
		ON WQXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'
	AND WQXX.WF_QUE = 'XA'
	AND WQXX.WF_SUB_QUE = 'XX'
	AND WQXX.PF_REQ_ACT = 'IDRPR'
	AND WQXX.WN_CTL_TSK = 'XXXXXXXXXXXXXXXXX '
order by 
	WF_CRT_DTS_WQXX