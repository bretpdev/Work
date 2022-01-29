USE CDW
GO

SELECT DISTINCT
	DF_SPE_ACC_ID,
	DM_PRS_X,
	DM_PRS_LST
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	DM_PRS_X NOT LIKE 'P%'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND 
	(
		--with space between name and suffix
		DM_PRS_LST LIKE '% Jr'
		OR DM_PRS_LST LIKE '% Sr'
		OR DM_PRS_LST LIKE '% I'
		OR DM_PRS_LST LIKE '% II'
		OR DM_PRS_LST LIKE  '% III'
		OR DM_PRS_LST LIKE '% IV'
		OR DM_PRS_LST LIKE '% V'
		OR DM_PRS_LST LIKE '% VI'
		OR DM_PRS_LST LIKE '% VII'
		OR DM_PRS_LST LIKE '% VIII'

		--no space between name and suffix (returns too many false positives e.g LEVI)
		--OR DM_PRS_LST LIKE  '%Jr'
		--OR DM_PRS_LST LIKE '%Sr'
		--OR DM_PRS_LST LIKE '%I'
		--OR DM_PRS_LST LIKE '%II'
		--OR DM_PRS_LST LIKE  '%III'
		--OR DM_PRS_LST LIKE '%IV'
		--OR DM_PRS_LST LIKE '%V'
		--OR DM_PRS_LST LIKE '%VI'	
	)