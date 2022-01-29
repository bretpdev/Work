SELECT
	NH.BF_SSN,
	NH.WN_SEQ_GRXX,
	ISNULL(NH.WC_NDS_RPT_STA,'') AS WC_NDS_RPT_STA,
	POP.WC_NDS_LN_DCH_RPT
FROM OPENQUERY(LEGEND,
'
	SELECT
		BF_SSN,
		WN_SEQ_GRXX,
		LN_SEQ,
		WC_NDS_LN_DCH_RPT
	FROM 
		PKUB.GRXX_RPT_LON_APL


')POP
	INNER JOIN CLS.[dbo].[NHXXXXX] NH
		ON NH.BF_SSN = POP.BF_SSN
		AND NH.WN_SEQ_GRXX = POP.WN_SEQ_GRXX
		AND NH.LN_SEQ = POP.LN_SEQ