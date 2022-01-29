SELECT DISTINCT
	NCH.AccountIdentifier,
--<<<<<<< HEAD
--	NCH.ActivityDate,
--	NCH.CallCampaign,
--	CONVERT(VARCHAR(10), LN16.LD_DLQ_OCC, 101) as DATE_DELQ_STARTED
--=======
	NCH.CallCampaign,
	NCH.ActivityDate AS CallDate,
-->>>>>>> d7feaf06debc0cf1153265fc88d3f5d72afa1dc1
	--DD.LN_DLQ_MAX,
	--DATEADD(DAY, LN_DLQ_MAX, DD.LD_DLQ_OCC),
	DD.LD_DLQ_OCC AS DelinquencyDate
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = NCH.AccountIdentifier
--<<<<<<< HEAD
--	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
--		ON LN16.BF_SSN = PD10.DF_PRS_ID
--		AND LN16.LD_DLQ_OCC <= NCH.ACTIVITYDATE
--WHERE
--	RegionId = 2
--	AND IsInbound = 0
--	AND ActivityDate >= '01/01/2020'
--	AND DATEADD(DAY, LN_DLQ_MAX, LD_DLQ_OCC) >= '01/01/2020'
--ORDER BY AccountIdentifier, LD_DLQ_OCC
--=======
	INNER JOIN UDW.calc.DailyDelinquency DD
		ON DD.BF_SSN = PD10.DF_PRS_ID
		AND DD.LN_DLQ_MAX >= 180
	INNER JOIN 
	(
		SELECT
			PD41.DF_PRS_ID,
			PD41.DC_ALW_ADL_PHN_HST
		FROM
		UDW..PD41_PHN_HST PD41
		INNER JOIN
		(
			SELECT	
				PD41.DF_PRS_ID,
				MAX(PD41.DN_PHN_SEQ) AS DN_PHN_SEQ
			FROM
				UDW..PD41_PHN_HST PD41
			GROUP BY 
				PD41.DF_PRS_ID
		) PD41_MAX_SEQ
			ON PD41.DF_PRS_ID = PD41_MAX_SEQ.DF_PRS_ID
			AND PD41.DN_PHN_SEQ = PD41_MAX_SEQ.DN_PHN_SEQ
	) PD41
		ON DD.BF_SSN = PD41.DF_PRS_ID
WHERE
	--CallCampaign IN ('UNOW')
	ActivityDate >= '01/01/2020'
	AND DATEADD(DAY, LN_DLQ_MAX, DD.LD_DLQ_OCC) >= '01/01/2020'
	AND PD41.DC_ALW_ADL_PHN_HST NOT IN ('L','P', 'X')
ORDER BY AccountIdentifier, CallCampaign, CallDate, DelinquencyDate
-->>>>>>> d7feaf06debc0cf1153265fc88d3f5d72afa1dc1
