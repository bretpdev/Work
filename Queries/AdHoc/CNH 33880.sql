SELECT
	OQ.*
FROM OPENQUERY(LEGEND,
'
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	PDXX.DF_PRS_ID,
	PDXX.DX_ADR_EML
FROM
	PKUB.PDXX_PRS_NME PDXX
INNER JOIN
(
SELECT
	EMAIL.*,
	ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber
	FROM
	(
		SELECT
			PDXX.DF_PRS_ID,
			PDXX.DX_ADR_EML,
			CASE 
				WHEN DC_ADR_EML = ''H'' THEN X -- home
				WHEN DC_ADR_EML = ''A'' THEN X -- alternate
				WHEN DC_ADR_EML = ''W'' THEN X -- work
			END AS PriorityNumber
		FROM
			PKUB.PDXX_PRS_ADR_EML PDXX
			INNER JOIN PKUB.LNXX_LON LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			PDXX.DI_VLD_ADR_EML = ''Y'' -- valid email address
			AND PDXX.DC_STA_PDXX = ''A'' -- active email address record
	) Email
) PDXX 
	ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	AND PDXX.EmailPriority = X --highest priority email only
') OQ
INNER JOIN CLS.[dbo].[CNHXXXXXData] D
	ON D.SSN = OQ.DF_PRS_ID




