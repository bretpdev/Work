use Cdw
go

SELECT DISTINCT
	LNXX.BF_SSN,
	PDXX.DF_SPE_ACC_ID,  
	(LTRIM(RTRIM(PDXX.DM_PRS_LST)) + ', ' + LTRIM(RTRIM(PDXX.DM_PRS_X))) [NAME],
	LNXX.LD_DFR_APL,
	LNXX.LD_DFR_BEG,
	LNXX.LD_DFR_END
FROM 
	CDW..DFXX_BR_DFR_REQ DFXX
	INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
	INNER JOIN PDXX_PRS_NME PDXX
		ON DFXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	LNXX.LD_DFR_APL BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX' 
	AND LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_STA_DFRXX = 'A'
	AND DFXX.LC_DFR_STA = 'A'
	AND DFXX.LC_DFR_TYP = 'XX'
