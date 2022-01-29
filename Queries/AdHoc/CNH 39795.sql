USE CDW
GO

DROP TABLE IF EXISTS #BASE_DATA

SELECT
	*,
	LTRIM(RTRIM(REPLACE(SUBSTRING(P.LX_ATY, CHARINDEX('ATION NUMBER IS ', P.LX_ATY) + XX, XX), ' ',''))) AS CONF_NUMBER,
	SUBSTRING(P.LX_ATY, CHARINDEX('DATE OF ', P.LX_ATY) + X, XX) AS EFF_DATE
INTO #BASE_DATA
FROM
(
	SELECT DISTINCT
		AYXX.BF_SSN,
		AYXX.LD_ATY_REQ_RCV,
		STUFF(
		(
			SELECT 
					' ' + SUB.LX_ATY AS [text()]
			FROM 
				AYXX_ATY_TXT SUB
			WHERE
				SUB.BF_SSN = AYXX.BF_SSN
				AND SUB.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
		FOR XML PATH('')
		)
		,X,X, '') AS LX_ATY
			
	FROM	
		AYXX_BR_LON_ATY AYXX
		INNER JOIN AYXX_ATY_TXT AYXX
			ON AYXX.BF_SSN = AYXX.BF_SSN
			AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
	WHERE
		AYXX.PF_REQ_ACT = 'OPSWP'
		AND AYXX.LD_ATY_REQ_RCV BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
) P

SELECT DISTINCT
	BD.*,
	RMXX.*
FROM 
	#BASE_DATA BD
	INNER JOIN
	(
		SELECT * FROM OPENQUERY(LEGEND,
		'
			SELECT
				*
			FROM
				WEBFLSX.RMXX_ONL_PAY
			WHERE
				LD_PAY > ''XX/XX/XXXX''
		')
	) RMXX
		ON RMXX.BF_SSN = BD.BF_SSN
		AND RMXX.PF_IRL_GNR_ID = BD.CONF_NUMBER
	LEFT JOIN 
	(
		SELECT DISTINCT
			BF_SSN,
			LD_FAT_EFF
		FROM
			LNXX_FIN_ATY
		WHERE
			LD_FAT_EFF BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX' 
			AND PC_FAT_TYP = 'XX'
			AND PC_FAT_SUB_TYP = 'XX'
			AND ISNULL(LC_FAT_REV_REA,'') = ''
	) LNXX
		ON LNXX.BF_SSN = BD.BF_SSN
		AND LNXX.LD_FAT_EFF = BD.EFF_DATE
	

WHERE 
	EFF_DATE BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	AND LNXX.BF_SSN IS NULL