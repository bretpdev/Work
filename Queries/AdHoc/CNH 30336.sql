SELECT * FROM OPENQUERY (LEGEND,'
	SELECT 
		*
	FROM 
		PKUB.LPXX_ITR_AND_TYP
	WHERE
		PC_STA_LPDXX = ''A''  --Current status of an interest rate parameter definition
');