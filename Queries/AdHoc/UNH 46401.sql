--SAS
SELECT
	SR.Request,
	SR.CurrentStatus,
	PIR.[Status],
	CAST(PIR.[Begin] AS DATE) AS Promoted
FROM
	BSYS..SCKR_REF_StatusPIR PIR
	INNER JOIN BSYS..SCKR_DAT_SASRequests SR
		ON PIR.Request = SR.Request
WHERE
	PIR.[Class] = 'SAS'
	AND PIR.[Status] = 'Post-Implementation Queue'
	AND CAST(PIR.[Begin] AS DATE) >= '2019-01-01'
	AND SR.CurrentStatus != 'Complete'

--Scripts
SELECT
	SR.Request,
	SR.CurrentStatus,
	PIR.[Status],
	CAST(PIR.[Begin] AS DATE) AS Promoted
FROM
	BSYS..SCKR_REF_StatusPIR PIR
	INNER JOIN BSYS..SCKR_DAT_ScriptRequests SR
		ON PIR.Request = SR.Request
WHERE
	PIR.[Class] = 'Scr'
	AND PIR.[Status] = 'Post-Implementation Queue'
	AND CAST(PIR.[Begin] AS DATE) >= '2019-01-01'
	AND SR.CurrentStatus != 'Complete'
