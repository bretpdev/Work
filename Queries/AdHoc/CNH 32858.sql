--run on UHEAASQLDB
USE CDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
GO

DECLARE @ZIPCODESX TABLE (ZIPS VARCHAR(X))

INSERT INTO @ZIPCODESX (ZIPS)
VALUES
 ('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID		[AccountNumber]
	,PDXX.DM_PRS_X			[First Name]
	,PDXX. DM_PRS_LST		[Last Name]
	,PDXX.DX_ADR_EML		[Email]
	,LNXX.LN_DLQ_MAX		[Days Delinquent]
	,PDXX.DX_STR_ADR_X		[Address X]
	,PDXX.DX_STR_ADR_X		[Address X]
	,PDXX.DX_STR_ADR_X		[Address X]
	,PDXX.DM_CT				[City]
	,PDXX.DC_DOM_ST			[State]
	,PDXX.DF_ZIP_CDE		[Zip]
	,PDXX.DN_DOM_PHN_LCL    [Phone number]
	,PDXX.DN_DOM_PHN_XCH	[Phone exchange]
	,PDXX.DN_DOM_PHN_ARA    [Phone Area Code]
FROM
	..LNXX_LON LNXX
	INNER JOIN ..PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN ..PDXX_PRS_ADR PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN @ZIPCODESX Z
		ON PDXX.DF_ZIP_CDE = Z.ZIPS
	LEFT JOIN ..LNXX_LON_DLQ_HST LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LC_STA_LONXX = 'X'
	LEFT JOIN ..DWXX_DW_CLC_CLU DWXX
		ON LNXX.BF_SSN = DWXX.BF_SSN
		AND LNXX.BF_SSN = DWXX.LN_SEQ
		AND DWXX.WC_DW_LON_STA = 'XX'
	LEFT JOIN ..PDXX_PRS_ADR_EML PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		AND�PDXX.DI_VLD_ADR_EML�=�'Y'
	LEFT JOIN ..PDXX_PRS_PHN PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		AND PDXX.DI_PHN_VLD = 'Y'
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND�LNXX.LA_CUR_PRI�>�X.XX
	AND�PDXX.DI_VLD_ADR�=�'Y'�
	AND�PDXX.DC_DOM_ST�=�'NY'�
