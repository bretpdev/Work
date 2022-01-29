USE udw
GO

DROP TABLE IF EXISTS #CONSENT;

WITH PHN
AS
(
SELECT
	DF_PRS_ID,
	DF_SPE_ACC_ID,
	PHN,
	DF_LST_DTS_PD41,
	CONSENT_END,
	CONSENT AS CONSENT
FROM
(
SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	DN_DOM_PHN_ARA_HST + DN_DOM_PHN_XCH_HST + DN_DOM_PHN_LCL_HST AS PHN,
	DF_LST_DTS_PD41,
	LEAD(DF_LST_DTS_PD41, 1) OVER (PARTITION BY PD41.DF_PRS_ID, PD41.DC_PHN_HST ORDER BY DN_PHN_SEQ) AS CONSENT_END,
	CASE WHEN DC_ALW_ADL_PHN_HST IN ('L','P', 'X') THEN 1 ELSE 0 END AS CONSENT
FROM
	udw..PD41_PHN_HST PD41
	INNER JOIN 
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			udw..LN16_LON_DLQ_HST
	) LN16
		ON LN16.BF_SSN = PD41.DF_PRS_ID
	INNER JOIN udw..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD41.DF_PRS_ID


UNION ALL
	
SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	DN_DOM_PHN_ARA + DN_DOM_PHN_XCH + DN_DOM_PHN_LCL AS PHN,
	PD40.DF_LST_DTS_PD42,
	GETDATE() AS NEXT_DATE,
	CASE WHEN DC_ALW_ADL_PHN IN ('L','P', 'X') THEN 1 ELSE 0 END AS CONSENT
FROM
	udw..PD42_PRS_PHN PD40
	INNER JOIN 
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			udw..LN16_LON_DLQ_HST
	) LN16
		ON LN16.BF_SSN = PD40.DF_PRS_ID
	INNER JOIN udw..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
) POP

)
SELECT
	DF_PRS_ID,
	DF_SPE_ACC_ID,
	PHN,
	DF_LST_DTS_PD41 AS CONSENT_BEGIN,
	ISNULL(CONSENT_END, CASE WHEN CONSENT_END IS NULL THEN MAX(DF_LST_DTS_PD41) OVER (PARTITION BY DF_PRS_ID) END) AS CONSENT_END,
	CONSENT
INTO #CONSENT
FROM 
	PHN P
;


DROP TABLE IF EXISTS #DATA;

WITH PHN
AS
(
SELECT
	DF_PRS_ID,
	DF_SPE_ACC_ID,
	PHN,
	DF_LST_DTS_PD41,
	NEXT_DATE,
	MAX(CONSENT) AS CONSENT
FROM
(
SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	DN_DOM_PHN_ARA_HST + DN_DOM_PHN_XCH_HST + DN_DOM_PHN_LCL_HST AS PHN,
	DF_LST_DTS_PD41,
	LEAD(DF_LST_DTS_PD41, 1) OVER (PARTITION BY PD41.DF_PRS_ID, PD41.DC_PHN_HST ORDER BY DN_PHN_SEQ) AS NEXT_DATE,
	CASE WHEN DC_ALW_ADL_PHN_HST IN ('L','P', 'X') THEN 1 ELSE 0 END AS CONSENT
FROM
	udw..PD41_PHN_HST PD41
	INNER JOIN 
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			udw..LN16_LON_DLQ_HST
		--where bf_ssn = '623240215'
	) LN16
		ON LN16.BF_SSN = PD41.DF_PRS_ID
	INNER JOIN udw..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD41.DF_PRS_ID


UNION ALL
	
SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	DN_DOM_PHN_ARA + DN_DOM_PHN_XCH + DN_DOM_PHN_LCL AS PHN,
	PD40.DF_LST_DTS_PD42,
	GETDATE() AS NEXT_DATE,
	CASE WHEN DC_ALW_ADL_PHN IN ('L','P', 'X') THEN 1 ELSE 0 END AS CONSENT
FROM
	udw..PD42_PRS_PHN PD40
	INNER JOIN 
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			udw..LN16_LON_DLQ_HST
		--where bf_ssn = '623240215'
	) LN16
		ON LN16.BF_SSN = PD40.DF_PRS_ID
	INNER JOIN udw..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
) POP
GROUP BY
	DF_PRS_ID,
	DF_SPE_ACC_ID,
	PHN,
	DF_LST_DTS_PD41,
	NEXT_DATE
)
SELECT
	*,
	ISNULL(NEXT_DATE, CASE WHEN NEXT_DATE IS NULL THEN MAX(DF_LST_DTS_PD41) OVER (PARTITION BY DF_PRS_ID) END) AS FINAL_HST 
INTO #DATA
FROM 
	PHN P
;

SELECT DISTINCT
	--pop.DF_PRS_ID,
	POP.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	POP.PHN AS PHONE_NUMBER,
	CAST(POP.ActivityDate AS DATE) AS CALL_DATE,
	CAST(POP.CONSENT_BEGIN AS  DATE) AS CONSENT_BEGIN,
	CAST(POP.CONSENT_END AS DATE) AS CONSENT_END,
	--MAX(CONSENT) AS CONSENT
	POP.CONSENT
FROM
(
SELECT 
		D.DF_PRS_ID,
	D.DF_SPE_ACC_ID,
	D.PHN,
	D.CONSENT AS C,
	nch.ActivityDate,
	D.CONSENT,
	NCH.NobleCallHistoryId,
	D.DF_LST_DTS_PD41 AS CONSENT_BEGIN,
	D.FINAL_HST AS CONSENT_END
FROM 
	#DATA D
	INNER JOIN NobleCalls..NobleCallHistory NCH
		ON NCH.AccountIdentifier = D.DF_PRS_ID
		AND NCH.PhoneNumber = D.PHN
		AND NCH.ActivityDate BETWEEN D.DF_LST_DTS_PD41 AND D.FINAL_HST


WHERE
	NCH.CallCampaign IN ('BDD1','BDDX','BLST')
	AND NCH.ActivityDate BETWEEN '01/01/2021' AND '04/02/2021'

UNION ALL

SELECT
		D.DF_PRS_ID,
	D.DF_SPE_ACC_ID,
	D.PHN,
	D.CONSENT AS C,
	nch.ActivityDate,
	D.CONSENT,
	NCH.NobleCallHistoryId,
	D.DF_LST_DTS_PD41 AS CONSENT_BEGIN,
	D.FINAL_HST AS CONSENT_END
FROM 
	#DATA D
	INNER JOIN NobleCalls..NobleCallHistory NCH
		ON NCH.AccountIdentifier = D.DF_SPE_ACC_ID
		AND NCH.PhoneNumber = D.PHN
		AND NCH.ActivityDate BETWEEN D.DF_LST_DTS_PD41 AND D.FINAL_HST
WHERE
	NCH.CallCampaign IN ('BDD1','BDDX','BLST')
	AND NCH.ActivityDate BETWEEN '01/01/2021' AND '04/02/2021'
) POP
LEFT JOIN #CONSENT C
	ON C.DF_PRS_ID = POP.DF_PRS_ID
	AND C.PHN = POP.PHN
	AND POP.ActivityDate BETWEEN C.CONSENT_BEGIN  AND C.CONSENT_END 
	and c.CONSENT = 1
WHERE
	DATEDIFF(DAY, POP.CONSENT_BEGIN, POP.ActivityDate) > 1
	AND C.DF_PRS_ID IS NULL

ORDER BY
	ACCOUNT_NUMBER,
	CALL_DATE DESC