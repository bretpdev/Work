--1 insert complete status records into StatusPIR
SELECT
	COUNT(*) AS CNT1_STATUSPIR_INS
FROM
	(
		SELECT
			DISTINCT
			STA.Request,
			STA.Class
		FROM 
			[BSYS].[dbo].[SCKR_DAT_PIR] PIR
			INNER JOIN [BSYS].[dbo].[SCKR_REF_StatusPIR] STA
				ON PIR.Request = STA.Request
				AND PIR.Class = STA.Class

		WHERE
			PIR.CurrentStatus NOT IN ('Complete', 'Closed', 'Returned')
			AND DATEDIFF(MONTH,PIR.CurrentStatusDate,GETDATE()) > 6	
	) CNT


--2 Set SCKR_REF_StatusPIR end date to current date
SELECT
	COUNT(*) AS CNT2_REFSTA_NULL_END
	--,STA.Sequence
FROM
	( 
		SELECT DISTINCT
			PIR.Request,
			PIR.Class
		FROM 
			[BSYS].[dbo].[SCKR_DAT_PIR] PIR
		WHERE
			PIR.CurrentStatus NOT IN ('Complete', 'Closed', 'Returned')
			AND DATEDIFF(MONTH,PIR.CurrentStatusDate,GETDATE()) > 6
	) PIR
	INNER JOIN [BSYS].[dbo].[SCKR_REF_StatusPIR] STA
		ON PIR.Request = STA.Request
		AND PIR.Class = STA.Class
WHERE
	 STA.[Status] NOT IN ('Complete', 'Closed', 'Returned')
	AND STA.[End] IS NULL


--3 Update SCKR_DAT_PIR to close PIR records
SELECT
	COUNT(*) AS CNT3_DAT_PIR
FROM 
	[BSYS].[dbo].[SCKR_DAT_PIR] PIR
WHERE
	CurrentStatus NOT IN ('Complete', 'Closed', 'Returned')
	AND DATEDIFF(MONTH,PIR.CurrentStatusDate,GETDATE()) > 6

