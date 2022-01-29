--run on UHEAASQLDB
USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
GO

DECLARE @ZIPCODES1 TABLE (ZIPS VARCHAR(5))

INSERT INTO @ZIPCODES1 (ZIPS)
VALUES
 ('10001')
,('10002')
,('10003')
,('10004')
,('10005')
,('10006')
,('10007')
,('10008')
,('10009')
,('10010')
,('10011')
,('10012')
,('10013')
,('10014')
,('10015')
,('10016')
,('10017')
,('10018')
,('10019')
,('10020')
,('10021')
,('10022')
,('10023')
,('10024')
,('10025')
,('10026')
,('10027')
,('10028')
,('10029')
,('10030')
,('10031')
,('10032')
,('10033')
,('10034')
,('10035')
,('10036')
,('10037')
,('10038')
,('10039')
,('10040')
,('10041')
,('10042')
,('10043')
,('10044')
,('10045')
,('10055')
,('10060')
,('10065')
,('10069')
,('10075')
,('10080')
,('10081')
,('10087')
,('10090')
,('10101')
,('10102')
,('10103')
,('10104')
,('10105')
,('10106')
,('10107')
,('10108')
,('10109')
,('10110')
,('10111')
,('10112')
,('10113')
,('10114')
,('10115')
,('10116')
,('10117')
,('10118')
,('10119')
,('10120')
,('10121')
,('10122')
,('10123')
,('10124')
,('10125')
,('10126')
,('10127')
,('10128')
,('10129')
,('10130')
,('10131')
,('10132')
,('10133')
,('10138')
,('10150')
,('10151')
,('10152')
,('10153')
,('10154')
,('10155')
,('10156')
,('10157')
,('10158')
,('10159')
,('10160')
,('10161')
,('10162')
,('10163')
,('10164')
,('10165')
,('10166')
,('10167')
,('10168')
,('10169')
,('10170')
,('10171')
,('10172')
,('10173')
,('10174')
,('10175')
,('10176')
,('10177')
,('10178')
,('10179')
,('10185')
,('10199')
,('10203')
,('10211')
,('10212')
,('10213')
,('10242')
,('10249')
,('10256')
,('10258')
,('10259')
,('10260')
,('10261')
,('10265')
,('10268')
,('10269')
,('10270')
,('10271')
,('10272')
,('10273')
,('10274')
,('10275')
,('10276')
,('10277')
,('10278')
,('10279')
,('10280')
,('10281')
,('10282')
,('10283')
,('10284')
,('10285')
,('10286')

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID		[AccountNumber]
	,PD10.DM_PRS_1			[First Name]
	,PD10. DM_PRS_LST		[Last Name]
	,PD32.DX_ADR_EML		[Email]
	,LN16.LN_DLQ_MAX		[Days Delinquent]
	,PD30.DX_STR_ADR_1		[Address 1]
	,PD30.DX_STR_ADR_2		[Address 2]
	,PD30.DX_STR_ADR_3		[Address 3]
	,PD30.DM_CT				[City]
	,PD30.DC_DOM_ST			[State]
	,PD30.DF_ZIP_CDE		[Zip]
	,PD42.DN_DOM_PHN_LCL    [Phone number]
	,PD42.DN_DOM_PHN_XCH	[Phone exchange]
	,PD42.DN_DOM_PHN_ARA    [Phone Area Code]
FROM
	..LN10_LON LN10
	INNER JOIN ..PD10_PRS_NME PD10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN ..PD30_PRS_ADR PD30
		ON LN10.BF_SSN = PD30.DF_PRS_ID
	INNER JOIN @ZIPCODES1 Z
		ON PD30.DF_ZIP_CDE = Z.ZIPS
	LEFT JOIN ..LN16_LON_DLQ_HST LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
	LEFT JOIN ..DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.BF_SSN = DW01.LN_SEQ
		AND DW01.WC_DW_LON_STA = '03'
	LEFT JOIN ..PD32_PRS_ADR_EML PD32
		ON LN10.BF_SSN = PD32.DF_PRS_ID
		AND PD32.DI_VLD_ADR_EML = 'Y'
	LEFT JOIN ..PD42_PRS_PHN PD42
		ON LN10.BF_SSN = PD42.DF_PRS_ID
		AND PD42.DI_PHN_VLD = 'Y'
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND PD30.DI_VLD_ADR = 'Y' 
	AND PD30.DC_DOM_ST = 'NY' 
