/*
WQXX.WF-QUE = RB and WQ.WX_MSG_X_TSK = Ineligible or Invalid School Code

Output
Account number = PDXX.DF-SPE-ACC-ID linked to WQXX.BF-SSN
SSN = WQXX.BF_SSN
Task status = WQXX.WC_STA_WQUEXX
School code from task = WQXX.WX_MSG_X_TSK = characters XX - XX
Subqueue = WQXX.WF_SUB_QUE
*/
USE CDW
GO

SELECT
	PDXX.DF_SPE_ACC_ID [AccountNumber],
	WQXX.BF_SSN,
	WQXX.WC_STA_WQUEXX [TaskStatus],
	SUBSTRING(WQXX.WX_MSG_X_TSK, XX, X) [SchoolCode],
	WQXX.WF_SUB_QUE [Subqueue]
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN WQXX_TSK_QUE WQXX ON WQXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	WQXX.WF_QUE = 'RB'
	AND
	WQXX.WX_MSG_X_TSK = 'Ineligible or Invalid School Code'
