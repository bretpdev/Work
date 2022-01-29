--From 01/01/2017 to 12/31/2018, can you provide the following for all guarantors and all claim types:

---Identify all accounts that are in claim paid status and have an endorser
---Indicate the guarantor that paid the claim
---Indicate dates that endorser due diligence letters were sent out
---Indicate the notes and/or type of letter sent

--As a separate request, we want to catch any instances of this outside of the audit period. Would you be able to provide the same information requested above, but for the period of January 1, 2019 in a separate doc?

--Thank you very much!

SELECT
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	LN10.LN_SEQ AS LoanSequence,
	CASE WHEN LN20.LC_EDS_TYP = 'M' THEN 'Coborrower' ELSE 'Endorser' END AS EndorserType,
	LN10.IF_GTR AS LoanGuarantor,
	DATEADD(DAY,360,LN16.LD_DLQ_OCC ) AS DefaultDate
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.BF_SSN = LN10.BF_SSN
		AND LN20.LN_SEQ = LN10.LN_SEQ
		AND LN20.LC_STA_LON20 = 'A'
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LN_DLQ_MAX >= 360
		--AND LN20.LC_EDS_TYP = 'S'
WHERE
	DW01.WC_DW_LON_STA = '12'
	AND CAST(LN10.LD_PIF_RPT AS DATE) BETWEEN '2017-01-01' AND '2018-12-31'


SELECT
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	LN10.LN_SEQ AS LoanSequence,
	CASE WHEN LN20.LC_EDS_TYP = 'M' THEN 'Coborrower' ELSE 'Endorser' END AS EndorserType,
	LN10.IF_GTR AS LoanGuarantor,
	DATEADD(DAY,360,LN16.LD_DLQ_OCC ) AS DefaultDate
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.BF_SSN = LN10.BF_SSN
		AND LN20.LN_SEQ = LN10.LN_SEQ
		AND LN20.LC_STA_LON20 = 'A'
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LN_DLQ_MAX >= 360
		--AND LN20.LC_EDS_TYP = 'S'
WHERE
	DW01.WC_DW_LON_STA = '12'
	AND CAST(LN10.LD_PIF_RPT AS DATE) BETWEEN '2019-01-01' AND CAST(GETDATE() AS DATE)
