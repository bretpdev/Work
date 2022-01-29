--Issue:
--Can you provide a list of all the SB tasks (Closed/Completed) today by UTXXXXX. Provide the output file with the following info:

--Account Number
--First / Last Name
--Date & Time Closed (only for today X.XX.XX)



SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	WQXX.WF_LST_DTS_WQXX
	--,WQXX.WF_QUE
	--,WQXX.WN_CTL_TSK
	--,WQXX.WC_STA_WQUEXX
FROM 
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN 
	CDW..WQXX_TSK_QUE_HST WQXX
		ON PDXX.DF_PRS_ID = WQXX.BF_SSN
WHERE
	WQXX.WF_QUE = 'SB'
	AND WQXX.WC_STA_WQUEXX IN ('C','X')
	AND CAST(WQXX.WF_LST_DTS_WQXX AS DATE) = 'XXXX-XX-XX'
	AND WQXX.WF_USR_ASN_TSK = 'UTXXXXX'
ORDER BY
	PDXX.DF_SPE_ACC_ID
