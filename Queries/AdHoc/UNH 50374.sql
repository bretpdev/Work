SELECT 
	*
FROM OPENQUERY
(
	DUSTER,
	'
		SELECT
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT
		FROM
			OLWHRM1.WQ20_TSK_QUE
		WHERE
			WF_QUE IN (''MS'',''2M'',''1M'',''0M'')
			AND WC_STA_WQUE20 = ''U''
	'
)


SELECT 
	*
FROM OPENQUERY
(
	LEGEND,
	'
		SELECT
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT
		FROM
			PKUB.WQ20_TSK_QUE
		WHERE
			WF_QUE IN (''MS'',''2M'',''1M'',''0M'')
			AND WC_STA_WQUE20 = ''U''
	'
)