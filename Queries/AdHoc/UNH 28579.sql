SELECT * FROM UDW..PD10_Borrower WHERE DF_SPE_ACC_ID = '6626190195'

SELECT * FROM OPENQUERY(DUSTER, 
	'
	SELECT 
		BF_SSN,
		LN_SEQ,
		LD_BIL_CRT,
		LN_SEQ_BIL_WI_DTE,
		LN_BIL_OCC_SEQ,
		LI_FNL_BIL_LON
	FROM 
		OLWHRM1.LN80_LON_BIL_CRF 
	WHERE 
		BF_SSN = ''528795255''
	')