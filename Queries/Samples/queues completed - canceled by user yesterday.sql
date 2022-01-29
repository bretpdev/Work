SELECT
	GX25.XF_USR,
	WQ21.WF_QUE,
	WQ21.WF_SUB_QUE,
	WQ21.WC_STA_WQUE20,
	MAX(Names.WM_QUE_FUL) AS QueueDescription,
	MAX(Names.WM_SUB_QUE_FUL) AS SubQueueDescription,
	COUNT(*) AS count
FROM
	CDW..WQ21_TSK_QUE_HST WQ21
	INNER JOIN CDW..GX25_USR GX25
		ON GX25.XF_USR = WQ21.WF_USR_ASN_TSK
	INNER JOIN OPENQUERY
	(LEGEND, 
	'
		SELECT DISTINCT
			WQ10.WF_QUE,
			WQ15.WF_SUB_QUE,
			WQ10.WM_QUE_FUL,
			WQ15.WM_SUB_QUE_FUL
		FROM
			PKUB.WQ10_TSK_QUE_DFN WQ10 
			INNER JOIN PKUB.WQ15_TSK_SUB_QUE WQ15 
				ON WQ15.WF_QUE = WQ10.WF_QUE	
	') Names
		ON Names.WF_QUE = WQ21.WF_QUE
		AND Names.WF_SUB_QUE = WQ21.WF_SUB_QUE
WHERE
	WQ21.WC_STA_WQUE20 IN ('X','C')
	AND WQ21.WF_LST_DTS_WQ20 BETWEEN GETDATE() - 1 AND GETDATE()
GROUP BY
	GX25.XF_USR,
	WQ21.WF_QUE,
	WQ21.WF_SUB_QUE,
	WQ21.WC_STA_WQUE20