--I need to query the XC queue. I'm looking for the most recent tasks created that are tied to both ARCs (FRBCU, and PLUSF). 
--because this task is created at loan level, I am expecting there to be duplicate requests for the same person. 
--please provide me with the tasks created as of X/X - X/X. I hope that this will at least include the two different arcs. 
--In the output file provide: accnt number, arc, date created. 

SELECT
	PDXX.DF_SPE_ACC_ID,
	WQXX.PF_REQ_ACT,
	WQXX.WD_ACT_REQ,
	WQXX.WC_STA_WQUEXX
FROM
	CDW..WQXX_TSK_QUE WQXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = WQXX.BF_SSN
WHERE
	WQXX.WF_QUE = 'XC'
	AND CAST(WQXX.WD_ACT_REQ AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'