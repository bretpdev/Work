--Issue:
--I need to query the system for all #40 Forbearance types that were added by UT03098 - Josh Gutierrez, and have an end date on/after 2/1/2022. 

--Output file:

--Account Number / #40 Forb Start date / #40 Forb End date.


SELECT
	PD10.DF_SPE_ACC_ID,
	CAST(LN60.LD_FOR_BEG AS DATE) AS BeginDate,
	CAST(LN60.LD_FOR_END AS DATE) AS EndDate,
	CAST(LN60.LD_FOR_APL AS DATE) AS AppliedDate
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..LN60_BR_FOR_APV LN60
		ON LN60.BF_SSN = LN10.BF_SSN
		AND LN60.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..FB10_BR_FOR_REQ FB10
		ON FB10.BF_SSN = LN60.BF_SSN
		AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
WHERE
	FB10.LC_FOR_TYP = '40'
	AND CAST(LN60.LD_FOR_END AS DATE) >= '2022-02-01'
	AND FB10.LF_USR_CRT_REQ_FOR = 'UT03098'