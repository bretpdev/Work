USE UDW
GO

DROP TABLE IF EXISTS #ADR_BOR
DROP TABLE IF EXISTS #PHN_BOR
DROP TABLE IF EXISTS #ADR_ENR
DROP TABLE IF EXISTS #PHN_EDR;

WITH ADR_BOR AS
(
SELECT
	DF_PRS_ID,
	DN_ADR_SEQ,
	DI_VLD_ADR,
	DATE_SKIP_Y,
	CASE 
		WHEN DATE_SKIP_N IS NOT NULL THEN DATE_SKIP_N
		ELSE LEAD(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ)
	END AS DATE_SKIP_N
FROM
(
	SELECT 
		DF_PRS_ID,
		DN_ADR_SEQ,
		DI_VLD_ADR,
		CASE 
			WHEN DATE_SKIP_Y IS NOT NULL THEN DATE_SKIP_Y
			ELSE LAG(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ) 
		END AS DATE_SKIP_Y,
		DATE_SKIP_N
	FROM
	(
	SELECT
		DF_PRS_ID, 
		9999 AS DN_ADR_SEQ,
		DI_VLD_ADR,
		NULL AS DATE_SKIP_Y,
		GETDATE() AS DATE_SKIP_N
	FROM
		UDW..PD30_PRS_ADR PD30
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = PD30.DF_PRS_ID
	WHERE 
		DC_ADR = 'L'

	UNION ALL 

	SELECT  
		DF_PRS_ID,
		DN_ADR_SEQ,
		DI_VLD_ADR_HST,
		DD_CRT_PD31 AS DATE_SKIP_Y,
		NULL AS DATE_SKIP_N
	FROM	
		UDW..PD31_PRS_INA PD31
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = PD31.DF_PRS_ID
	WHERE
		PD31.DC_ADR_HST = 'L'
	) POP
) PF
)
SELECT 
	DF_PRS_ID,
	CONVERT(VARCHAR,LN16.LD_DLQ_OCC, 101) AS DATE_DELINQUENCY,
	'Address' AS [Type],
	--DN_ADR_SEQ,
	DI_VLD_ADR,
	CONVERT(VARCHAR, CASE 
		WHEN DN_ADR_SEQ = 9999 AND DATE_SKIP_Y IS NULL THEN '01/01/1900'
		ELSE DATE_SKIP_Y
	END, 101) AS DATE_SKIP_Y,
	CONVERT(VARCHAR, DATE_SKIP_N, 101) AS DATE_SKIP_N
	--LN16.LD_DLQ_OCC,
	--LN16.LD_DLQ_MAX
INTO #ADR_BOR
FROM ADR_BOR A
INNER JOIN
(
	SELECT DISTINCT
		LN16.BF_SSN,
		LN16.LN_DLQ_MAX,
		LN16.LD_DLQ_OCC,
		LN16.LD_DLQ_MAX
	FROM 
		UDW..LN16_LON_DLQ_HST LN16
	WHERE
		LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
		AND LN16.LC_DLQ_TYP = 'P'
		AND LN16.LN_DLQ_MAX >= 10
) LN16
	ON LN16.BF_SSN = A.DF_PRS_ID
WHERE
	DI_VLD_ADR = 'N'
	AND 
	(
		DATE_SKIP_Y BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
		OR
		DATE_SKIP_N BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
	)

ORDER BY DF_PRS_ID, DN_ADR_SEQ
;


WITH PHN_BOR AS
(
SELECT
	DF_PRS_ID,
	DN_ADR_SEQ,
	DI_PHN_VLD,
	DATE_SKIP_Y,
	CASE 
		WHEN DATE_SKIP_N IS NOT NULL THEN DATE_SKIP_N
		ELSE LEAD(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ)
	END AS DATE_SKIP_N
FROM
(
	SELECT 
		DF_PRS_ID,
		DN_ADR_SEQ,
		DI_PHN_VLD,
		CASE 
			WHEN DATE_SKIP_Y IS NOT NULL THEN DATE_SKIP_Y
			ELSE LAG(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ) 
		END AS DATE_SKIP_Y,
		DATE_SKIP_N
	FROM
	(
	SELECT
		DF_PRS_ID, 
		9999 AS DN_ADR_SEQ,
		DI_PHN_VLD,
		NULL AS DATE_SKIP_Y,
		GETDATE() AS DATE_SKIP_N
	FROM
		UDW..PD42_PRS_PHN PD30
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = PD30.DF_PRS_ID
	WHERE 
		DC_PHN = 'H'

	UNION ALL 

	SELECT  
		DF_PRS_ID,
		DN_PHN_SEQ,
		DI_PHN_VLD_HST,
		DD_CRT_41 AS DATE_SKIP_Y,
		NULL AS DATE_SKIP_N
	FROM	
		UDW..PD41_PHN_HST PD31
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = PD31.DF_PRS_ID
	WHERE
		PD31.DC_PHN_HST = 'H'
	) POP
) PF
)
SELECT 
	DF_PRS_ID,
	CONVERT(VARCHAR,LN16.LD_DLQ_OCC, 101) AS DATE_DELINQUENCY,
	--DN_ADR_SEQ,
	'Phone' as [Type],
	DI_PHN_VLD,
	CONVERT(VARCHAR, CASE 
		WHEN DN_ADR_SEQ = 9999 AND DATE_SKIP_Y IS NULL THEN '01/01/1900'
		ELSE DATE_SKIP_Y
	END, 101) AS DATE_SKIP_Y,
	CONVERT(VARCHAR, DATE_SKIP_N, 101) AS DATE_SKIP_N
	--LN16.LD_DLQ_OCC,
	--LN16.LD_DLQ_MAX
INTO #PHN_BOR
FROM PHN_BOR A
INNER JOIN
(
	SELECT DISTINCT
		LN16.BF_SSN,
		LN16.LN_DLQ_MAX,
		LN16.LD_DLQ_OCC,
		LN16.LD_DLQ_MAX
	FROM 
		UDW..LN16_LON_DLQ_HST LN16
	WHERE
		LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
		AND LN16.LC_DLQ_TYP = 'P'
		AND LN16.LN_DLQ_MAX >= 10
) LN16
	ON LN16.BF_SSN = A.DF_PRS_ID
WHERE
	DI_PHN_VLD = 'N'
	AND 
	(
		DATE_SKIP_Y BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
		OR
		DATE_SKIP_N BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
	)

ORDER BY DF_PRS_ID, DN_ADR_SEQ
;


WITH ADR_ENR AS
(
SELECT
	DF_PRS_ID,
	DN_ADR_SEQ,
	DI_VLD_ADR,
	DATE_SKIP_Y,
	CASE 
		WHEN DATE_SKIP_N IS NOT NULL THEN DATE_SKIP_N
		ELSE LEAD(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ)
	END AS DATE_SKIP_N
FROM
(
	SELECT 
		DF_PRS_ID,
		DN_ADR_SEQ,
		DI_VLD_ADR,
		CASE 
			WHEN DATE_SKIP_Y IS NOT NULL THEN DATE_SKIP_Y
			ELSE LAG(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ) 
		END AS DATE_SKIP_Y,
		DATE_SKIP_N
	FROM
	(
	SELECT
		DF_PRS_ID, 
		9999 AS DN_ADR_SEQ,
		DI_VLD_ADR,
		NULL AS DATE_SKIP_Y,
		GETDATE() AS DATE_SKIP_N
	FROM
		UDW..PD30_PRS_ADR PD30
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.LF_EDS = PD30.DF_PRS_ID
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = LN20.BF_SSN
	WHERE 
		DC_ADR = 'L'

	UNION ALL 

	SELECT  
		DF_PRS_ID,
		DN_ADR_SEQ,
		DI_VLD_ADR_HST,
		DD_CRT_PD31 AS DATE_SKIP_Y,
		NULL AS DATE_SKIP_N
	FROM	
		UDW..PD31_PRS_INA PD31
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.LF_EDS = PD31.DF_PRS_ID
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = LN20.BF_SSN
	WHERE
		PD31.DC_ADR_HST = 'L'
	) POP
) PF
)
SELECT 
	DF_PRS_ID,
	CONVERT(VARCHAR, LN16.LD_DLQ_OCC, 101) AS DATE_DELINQUENCY,
	'Address' AS [Type],
	--DN_ADR_SEQ,
	DI_VLD_ADR,
	CONVERT(VARCHAR, CASE 
		WHEN DN_ADR_SEQ = 9999 AND DATE_SKIP_Y IS NULL THEN '01/01/1900'
		ELSE DATE_SKIP_Y
	END, 101) AS DATE_SKIP_Y,
	CONVERT(VARCHAR, DATE_SKIP_N, 101) AS DATE_SKIP_N,
	'Y' AS IS_ENDORSER_OR_COBWR
	--LN16.LD_DLQ_OCC,
	--LN16.LD_DLQ_MAX
INTO #ADR_ENR
FROM ADR_ENR A
INNER JOIN
(
	SELECT DISTINCT
		LN16.BF_SSN,
		LN16.LN_DLQ_MAX,
		LN16.LD_DLQ_OCC,
		LN16.LD_DLQ_MAX
	FROM 
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN16.BF_SSN
			AND LN20.LN_SEQ = LN16.LN_SEQ
	WHERE
		LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
		AND LN16.LC_DLQ_TYP = 'P'
		AND LN16.LN_DLQ_MAX >= 10
) LN16
	ON LN16.BF_SSN = A.DF_PRS_ID

WHERE
	DI_VLD_ADR = 'N'
	AND 
	(
		DATE_SKIP_Y BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
		OR
		DATE_SKIP_N BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
	)

ORDER BY DF_PRS_ID, DN_ADR_SEQ
;


WITH PHN_EDR AS
(
SELECT
	DF_PRS_ID,
	DN_ADR_SEQ,
	DI_PHN_VLD,
	DATE_SKIP_Y,
	CASE 
		WHEN DATE_SKIP_N IS NOT NULL THEN DATE_SKIP_N
		ELSE LEAD(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ)
	END AS DATE_SKIP_N
FROM
(
	SELECT 
		DF_PRS_ID,
		DN_ADR_SEQ,
		DI_PHN_VLD,
		CASE 
			WHEN DATE_SKIP_Y IS NOT NULL THEN DATE_SKIP_Y
			ELSE LAG(DATE_SKIP_Y) OVER (PARTITION BY DF_PRS_ID ORDER BY DN_ADR_SEQ) 
		END AS DATE_SKIP_Y,
		DATE_SKIP_N
	FROM
	(
	SELECT
		DF_PRS_ID, 
		9999 AS DN_ADR_SEQ,
		DI_PHN_VLD,
		NULL AS DATE_SKIP_Y,
		GETDATE() AS DATE_SKIP_N
	FROM
		UDW..PD42_PRS_PHN PD30
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.LF_EDS = PD30.DF_PRS_ID
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = LN20.BF_SSN
	WHERE 
		DC_PHN = 'H'

	UNION ALL 

	SELECT  
		DF_PRS_ID,
		DN_PHN_SEQ,
		DI_PHN_VLD_HST,
		DD_CRT_41 AS DATE_SKIP_Y,
		NULL AS DATE_SKIP_N
	FROM	
		UDW..PD41_PHN_HST PD31
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.LF_EDS = PD31.DF_PRS_ID
		INNER JOIN
		(
			SELECT DISTINCT
				LN16.BF_SSN
			FROM 
				UDW..LN16_LON_DLQ_HST LN16
			WHERE
				LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
				AND LN16.LC_DLQ_TYP = 'P'
				AND LN16.LN_DLQ_MAX >= 10
		) LN16
			ON LN16.BF_SSN = LN20.BF_SSN
	WHERE
		PD31.DC_PHN_HST = 'H'
	) POP
) PF
)
SELECT 
	DF_PRS_ID,
	CONVERT(VARCHAR, LN16.LD_DLQ_OCC, 101) AS DATE_DELINQUENCY,
	--DN_ADR_SEQ,
	'Phone' as [Type],
	DI_PHN_VLD,
	CONVERT(VARCHAR, CASE 
		WHEN DN_ADR_SEQ = 9999 AND DATE_SKIP_Y IS NULL THEN '01/01/1900'
		ELSE DATE_SKIP_Y
	END, 101) AS DATE_SKIP_Y,
	CONVERT(VARCHAR, DATE_SKIP_N, 101) AS DATE_SKIP_N,
	'Y' AS IS_ENDORSER_OR_COBWR
	--LN16.LD_DLQ_MAX
INTO #PHN_EDR
FROM PHN_EDR A
INNER JOIN
(
	SELECT DISTINCT
		LN16.BF_SSN,
		LN16.LN_DLQ_MAX,
		LN16.LD_DLQ_OCC,
		LN16.LD_DLQ_MAX
	FROM 
		UDW..LN16_LON_DLQ_HST LN16
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN16.BF_SSN
			AND LN20.LN_SEQ = LN16.LN_SEQ
	WHERE
		LN16.LD_DLQ_OCC BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')
		AND LN16.LC_DLQ_TYP = 'P'
		AND LN16.LN_DLQ_MAX >= 10
) LN16
	ON LN16.BF_SSN = A.DF_PRS_ID
WHERE
	DI_PHN_VLD = 'N'
	AND 
	(
		DATE_SKIP_Y BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
		OR
		DATE_SKIP_N BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
	)

ORDER BY DF_PRS_ID, DN_ADR_SEQ
;
SELECT 
	ADR.*
FROM 
	#ADR_BOR ADR
 


union all

SELECT 
	PHN.*
FROM 
	#PHN_BOR PHN

ORDER BY 
	DF_PRS_ID;

SELECT 
	ADR.* 
FROM 
	#ADR_ENR ADR
	

union all

SELECT 
	PHN.* 
FROM 
	#PHN_EDR PHN
	
ORDER BY 
	DF_PRS_ID;

--TAB 2
SELECT DISTINCT
	AY10.BF_SSN,
	AY10.PF_REQ_ACT,
	AY10.LD_ATY_REQ_RCV
FROM
	UDW..AY10_BR_LON_ATY AY10
	INNER JOIN
	(
		SELECT DISTINCT
			DF_PRS_ID
		FROM
			#PHN_BOR

		UNION
 
		SELECT DISTINCT
			DF_PRS_ID
		FROM
			#ADR_BOR
	) POP
		ON POP.DF_PRS_ID = AY10.BF_SSN
WHERE
	AY10.LC_STA_ACTY10 = 'A'
	AND AY10.PF_REQ_ACT in 
	(
	'DDPHN',
	'KUBGS',
	'KUBSS',
	'KABA2',
	'KUBTL',
	'DC225',
	'KABAB',
	'KGNRL',
	'KLSLT',
	'KUBFR',
	'LM749',
	'KLVAP',
	'D9209',
	'S4NLS',
	'P203A',
	'KUBPF',
	'KABL2',
	'BADEM'
	)
	AND AY10.LD_ATY_REQ_RCV BETWEEN DATEADD(DAY, -10, '03/01/2020') AND DATEADD(DAY, -10, '09/30/2020')