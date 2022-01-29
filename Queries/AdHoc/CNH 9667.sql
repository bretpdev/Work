/*Find all CornerStone borrowers that are NOT on e-corr that have a Canadian address*/
USE CDW
GO

SELECT DISTINCT 
	*
FROM 
	[DBO].[PDXX_BORROWER] PDXX
	INNER JOIN [DBO].[PHXX_CONTACTEMAIL] PHXX
		ON PDXX.DF_SPE_ACC_ID = PHXX.DF_SPE_ACC_ID
	INNER JOIN [DBO].[PDXX_ADDRESS] PDXX
		ON PHXX.DF_SPE_ACC_ID = PDXX.DF_SPE_ACC_ID
WHERE 
	PDXX.[DM_FGN_CNY] = 'CANADA'
	AND	(--if both are true, then Ecorr. If either one is false, then no Ecorr.
			PHXX.[DI_VLD_CNC_EML_ADR] != X --valid email address
			OR PHXX.[DI_CNC_EBL_OPI ] != X --Ecorr indicator
		) 

