USE UDW
GO

SELECT
	WQ21.WF_QUE,
	WQ21.WF_SUB_QUE,
	WQ21.WF_CRT_DTS_WQ21 AS DATE_TASK_CLOSED,
	WQ21.WF_USR_ASN_TSK,
	PD10.DF_SPE_ACC_ID,
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [NAME]
FROM
	UDW..WQ21_TSK_QUE_HST WQ21
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ21.BF_SSN
WHERE
	WQ21.WF_QUE IN ('1M','2M','MS')
	AND WQ21.WF_CRT_DTS_WQ21 >= '07/01/2019'
	AND WQ21.WC_STA_WQUE20 IN ('C','X')