USE UDW
GO

SELECT 
	 PH05.DF_CNC_SYS_ID
	,PH05.DF_SPE_ID
	,PH05.DI_CNC_ELT_OPI
	,PH05.DC_ELT_OPI_APL_SRC
	,PH05.DC_ELT_OPI_SRC
	,PH05.DF_DTS_ELT_OPI_EFF 
	,PH05.DF_LST_USR_ELT_OPI 
	,PH05.DI_CNC_EBL_OPI
	,PH05.DC_EBL_OPI_APL_SRC 
	,PH05.DC_EBL_OPI_SRC
	,PH05.DF_DTS_EBL_OPI_EFF 
	,PH05.DF_LST_USR_EBL_OPI
FROM 
	[dbo].[LN10_LON] LN10
	INNER JOIN [dbo].[PD10_Borrower] PD10
		ON LN10.BF_SSN = PD10.BF_SSN
	INNER JOIN [dbo].[PH05_CNC_EML] PH05
		ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
	AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
