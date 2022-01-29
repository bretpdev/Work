USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @DATE1 DATE = '9/15/2018'
DECLARE @DATE2 DATE = '10/12/2018'

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AS Account_Number
	,PD10.DM_PRS_1 AS First_Name
	,PD10.DM_PRS_LST AS Last_Name
	,AY10.LD_ATY_REQ_RCV AS Received_Date
	,CAST(LN65.LD_CRT_LON65 AS DATE) AS Processed_Date
	,AY10.PF_REQ_ACT AS ARC_Type
	,RS05.BF_CRT_USR_RS05 AS Processed_By
FROM
	RS05_IBR_RPS RS05
	INNER JOIN RS10_BR_RPD RS10
		ON RS10.BF_SSN = RS05.BF_SSN
		AND RS10.BD_CRT_RS05 = RS05.BD_CRT_RS05
		AND RS10.BN_IBR_SEQ = RS05.BN_IBR_SEQ
	INNER JOIN LN65_LON_RPS LN65
		ON RS10.BF_SSN = LN65.BF_SSN
		AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
	INNER JOIN PD10_PRS_NME PD10
		ON RS10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN AY10_BR_LON_ATY AY10
		ON AY10.BF_SSN = RS05.BF_SSN
WHERE
	LN65.LC_TYP_SCH_DIS IN ('CA', 'CP', 'CQ', 'C1', 'C2', 'C3', 'IB', 'IL', 'IP', 'IS', 'I3', 'I5')
	AND LN65.LC_STA_LON65 = 'A' 
	AND LN65.LD_CRT_LON65 BETWEEN @DATE1 AND @DATE2
	AND RS05.BC_STA_RS05  = 'A'
	AND RS05.BC_IBR_INF_SRC_VER = 'ALT'
	AND RS05.BA_AGI > 0.00
	AND AY10.PF_REQ_ACT IN ('IBRDF','IDRPN')
	AND AY10.LF_LST_DTS_AY10 BETWEEN DATEADD(WEEK, -2, LN65.LD_CRT_LON65) AND DATEADD(WEEK, 2, LN65.LD_CRT_LON65) 
;