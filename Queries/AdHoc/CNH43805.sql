SELECT 
	UPPER(RTRIM(PDXX.DM_PRS_X) + ' ' + SUBSTRING(RTRIM(PDXX.DM_PRS_MID), X,X) + ' ' + RTRIM(PDXX.DM_PRS_LST)) AS [NAME],
	CNH.* 
FROM 
	CDW..CNHXXXXX CNH
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = CNH.SSN