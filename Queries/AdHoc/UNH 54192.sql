USE UDW
GO

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	WQ20.BF_SSN, 
	WQ20.WF_QUE, 
	WQ20.WF_SUB_QUE, 
	WQ20.WN_CTL_TSK, 
	WQ20.PF_REQ_ACT, 
	WQ20.WD_ACT_REQ, 
	WQ20.WC_STA_WQUE20 
FROM 
	..WQ20_TSK_QUE WQ20
	INNER JOIN ..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ20.WN_CTL_TSK
WHERE 
	WQ20.WF_QUE = '1E' 
	AND WQ20.WF_SUB_QUE = '02'
	AND WQ20.WD_ACT_REQ BETWEEN CAST('11/01/2015' AS DATE) AND CAST('12/31/2016' AS DATE)
ORDER BY 
	WQ20.WD_ACT_REQ
