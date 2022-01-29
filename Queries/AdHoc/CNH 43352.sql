--XX/XX queue with all tasks tied to ARC (DELQE). 

USE CDW
GO

SELECT DISTINCT
--	BF_SSN, --TODO: comment out for que killer file, uncomment for user file
	WF_QUE, 
	WF_SUB_QUE, 
	WN_CTL_TSK, 
	PF_REQ_ACT

FROM
	CDW..WQXX_TSK_QUE WQXX
WHERE
	WF_QUE = 'XX'
	AND WF_SUB_QUE = 'XX'
	AND PF_REQ_ACT = 'DELQE'
	AND WC_STA_WQUEXX NOT IN ('C','X')
