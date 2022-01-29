--X. SSN
--X. Date ARC was added (date of processing)
--X. Current name on file
--X. Previous name on file
--X. Indication if the borrower has served in the military (yes or no)
--XX/X/XX - X/XX/XX
--LSKEY arc

SELECT DISTINCT
	PDXX.DF_PRS_ID AS SSN,
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) AS ArcDate,
	CentralData.dbo.TRIM(PDXX.DM_PRS_X) AS CurrentFirstName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_MID) AS CurrentMiddleName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_LST) AS CurrentLastName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_LST_SFX) AS CurrentSuffixName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_X_HST) AS PreviousFirstName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_MID_HST) AS PreviousMiddleName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_LST_HST) AS PreviousLastName,
	CentralData.dbo.TRIM(PDXX.DM_PRS_LST_SFX_HST) AS PreviousSuffixName,
	ISNULL(M.ActiveMilitaryIndicator,X) AS Military
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN
	(
		SELECT
			DF_PRS_ID,
			DM_PRS_X_HST,
			DM_PRS_MID_HST,
			DM_PRS_LST_HST,
			DM_PRS_LST_SFX_HST
		FROM
			CDW..PDXX_PRS_NME_HST
		WHERE
			DD_CRT_PDXX BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	) PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		AND 
		(
			CentralData.dbo.TRIM(PDXX.DM_PRS_X_HST) != CentralData.dbo.TRIM(PDXX.DM_PRS_X)
			OR CentralData.dbo.TRIM(PDXX.DM_PRS_MID_HST) != CentralData.dbo.TRIM(PDXX.DM_PRS_MID)
			OR CentralData.dbo.TRIM(PDXX.DM_PRS_LST_HST) != CentralData.dbo.TRIM(PDXX.DM_PRS_LST)
			OR CentralData.dbo.TRIM(PDXX.DM_PRS_LST_SFX_HST) != CentralData.dbo.TRIM(PDXX.DM_PRS_LST_SFX)
		)
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = PDXX.DF_PRS_ID
		AND AYXX.PF_REQ_ACT = 'LSKEY'
		AND CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	LEFT JOIN CDW.FsaInvMet.Daily_Military M
		ON M.BF_SSN = PDXX.DF_PRS_ID
WHERE
	SUBSTRING(PDXX.DF_PRS_ID,X,X) != 'P'
	AND CentralData.dbo.TRIM(PDXX.DF_SPE_ACC_ID) != ''
ORDER BY
	PDXX.DF_PRS_ID