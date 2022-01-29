  --DM_PRS_LST LIKE  '% Jr'
  --DM_PRS_LST LIKE '% Sr'
  --DM_PRS_LST LIKE '% II'
  --DM_PRS_LST LIKE  '% III'I'
  --DM_PRS_LST LIKE '% IV'
  --DM_PRS_LST LIKE '% V'
  --DM_PRS_LST LIKE '% VI'

SELECT DISTINCT
	DF_SPE_ACC_ID,
	DM_PRS_1,
	DM_PRS_LST
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
WHERE
	DM_PRS_1 NOT LIKE 'P%'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND 
	(
		--with space between name and suffix
		DM_PRS_LST LIKE '% Jr%'
		OR DM_PRS_LST LIKE '% Sr%'
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