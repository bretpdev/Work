USE UDW
GO
	SELECT 
		PD10.DF_SPE_ACC_ID						[Account Number],
		CONVERT(VARCHAR, LN16.LD_DLQ_OCC, 101)	[Del Occurred]
	FROM
		LN10_LON LN10
		INNER JOIN PD10_PRS_NME		PD10	ON	PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN LN16_LON_DLQ_HST LN16	ON	LN16.BF_SSN = LN10.BF_SSN
												AND	LN16.LN_SEQ	= LN10.LN_SEQ
		INNER JOIN DW01_DW_CLC_CLU	DW01	ON	DW01.BF_SSN = LN16.BF_SSN
												AND	DW01.LN_SEQ = LN16.LN_SEQ
												AND DW01.WC_DW_LON_STA NOT IN ('06', '08')
	WHERE	
		DATEADD(DAY, 360, LN16.LD_DLQ_OCC) < GETDATE()
		AND LN16.LC_STA_LON16 = 1
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0
