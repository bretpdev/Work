--local table only has up to XXXX data; ticket requests XXXX data
SELECT
	'LOCAL' AS SOURCE_TABLE
	,MAX(LF_TAX_YR) AS max_LF_TAX_YR
	,MAX(WF_CRT_DTS_MRXX) AS max_WF_CRT_DTS_MRXX
FROM
	CDW..MRXX_MSC_TAX_RPT
;

--remote table only has up to XXXX data; ticket requests XXXX data
SELECT 
	'REMOTE' AS SOURCE_TABLE
	,max_LF_TAX_YR
	,max_WF_CRT_DTS_MRXX
FROM 
	OPENQUERY (LEGEND,
	'
		SELECT
			MAX(LF_TAX_YR) AS max_LF_TAX_YR
			,MAX(WF_CRT_DTS_MRXX) AS max_WF_CRT_DTS_MRXX
		FROM
			PKUB.MRXX_MSC_TAX_RPT
	')
;

--run after tables have been updated with XXXX data
SELECT * FROM OPENQUERY (LEGEND,
'
	SELECT * 
	FROM PKUB.MRXX_MSC_TAX_RPT
	WHERE LF_TAX_YR = XXXX
');



--XXXX data dump for specific borrowers
SELECT 
	*
FROM
	CDW..MRXX_MSC_TAX_RPT
WHERE
	LF_TAX_YR = XXXX
	AND BF_SSN IN
	(
		 'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
		,'XXXXXXXXX'
	);
