CREATE PROCEDURE [dbo].[GetOutboundCalls]
(
	@OutboundPopulation OutboundPopulation READONLY
)
AS
DECLARE @ScriptId VARCHAR(100) = 'DIACTCMTS'
DECLARE @OnelinkRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'ONELINK')
DECLARE @CompassRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'COMPASSUHEAA')

DELETE FROM _OutboundPopulation 

--Set up the processing tables
INSERT INTO 
	_OutboundPopulation 
SELECT
	rowid,
	call_type,
	listid,
	appl,
	lm_filler2,
	areacode,
	phone,
	addi_status,
	[status],
	tsr,
	act_date,
	act_time,
	vox_file_name,
	time_connect,
	TimeACW,
	TimeHold,
	AgentHold,
	Filler1,
	Filler3,
	Filler4,
	d_record_id,
	DialerField1,
    DialerField2,
    DialerField3,
    DialerField4,
    DialerField5,
    DialerField6,
    DialerField7,
    DialerField8,
    DialerField9,
    DialerField10,
    DialerField11,
    DialerField12,
    DialerField13,
    DialerField14,
    DialerField15,
    DialerField16,
    DialerField17,
    DialerField18,
    DialerField19,
    DialerField20,
    DialerField21,
    DialerField22,
    DialerField23,
    DialerField24,
    DialerField25
FROM 
	@OutboundPopulation

--START ONELINK POPULATION, UDW_DF_SPE_ACC_ID is just a convention and will contain onelink account information in the onelink population

SELECT DISTINCT
	PD01.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
	NULL AS UDW_CoborrowerAccountNumber,
	OQ.rowid, 
	OQ.call_type, 
	OQ.listid, 
	OQ.appl, 
	OQ.DialerField7 AS lm_filler2, --Dialer field 7 contains borrower account number for LGP
	OQ.areacode, 
	OQ.phone, 
	OQ.addi_status, 
	OQ.[status], 
	OQ.tsr,
	OQ.act_date, 
	OQ.act_time, 
	OQ.vox_file_name, 
	OQ.time_connect,
	OQ.TimeACW,
	OQ.TimeHold,
	OQ.AgentHold,
	OQ.Filler1,
	OQ.Filler3,
	OQ.Filler4,
	OQ.d_record_id,
	OQ.DialerField1, 
	OQ.DialerField2, 
	OQ.DialerField3, 
	OQ.DialerField4, 
	OQ.DialerField5, 
	OQ.DialerField6, 
	OQ.DialerField7, 
	OQ.DialerField8, 
	OQ.DialerField9, 
	OQ.DialerField10,
	OQ.DialerField11,
	OQ.DialerField12,
	OQ.DialerField13,
	OQ.DialerField14,
	OQ.DialerField15,
	OQ.DialerField16,
	OQ.DialerField17,
	OQ.DialerField18,
	OQ.DialerField19,
	OQ.DialerField20,
	OQ.DialerField21,
	OQ.DialerField22,
	OQ.DialerField23,
	OQ.DialerField24,
	OQ.DialerField25
FROM 
	_OutboundPopulation OQ
	INNER JOIN CampaignPrefixes CP
		ON OQ.appl LIKE (CP.CampaignPrefix + '%')
		AND CP.RegionId = @OnelinkRegion
		AND CP.ScriptId = @ScriptId
		AND CP.Active = 1
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON OQ.DialerField7 = PD01.DF_SPE_ACC_ID
		AND OQ.DialerField1 = PD01.DF_PRS_ID

UNION

SELECT DISTINCT
	PD01_BOR.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
	PD01.DF_SPE_ACC_ID AS UDW_CoborrowerAccountNumber,
	OQ.rowid, 
	OQ.call_type, 
	OQ.listid, 
	OQ.appl, 
	OQ.DialerField7 AS lm_filler2, --Dialer field 7 contains account number for LGP
	OQ.areacode, 
	OQ.phone, 
	OQ.addi_status, 
	OQ.[status], 
	OQ.tsr,
	OQ.act_date, 
	OQ.act_time, 
	OQ.vox_file_name, 
	OQ.time_connect,
	OQ.TimeACW,
	OQ.TimeHold,
	OQ.AgentHold,
	OQ.Filler1,
	OQ.Filler3,
	OQ.Filler4,
	OQ.d_record_id,
	OQ.DialerField1, 
	OQ.DialerField2, 
	OQ.DialerField3, 
	OQ.DialerField4, 
	OQ.DialerField5, 
	OQ.DialerField6, 
	OQ.DialerField7, 
	OQ.DialerField8, 
	OQ.DialerField9, 
	OQ.DialerField10,
	OQ.DialerField11,
	OQ.DialerField12,
	OQ.DialerField13,
	OQ.DialerField14,
	OQ.DialerField15,
	OQ.DialerField16,
	OQ.DialerField17,
	OQ.DialerField18,
	OQ.DialerField19,
	OQ.DialerField20,
	OQ.DialerField21,
	OQ.DialerField22,
	OQ.DialerField23,
	OQ.DialerField24,
	OQ.DialerField25
FROM 
	_OutboundPopulation OQ
	INNER JOIN CampaignPrefixes CP
		ON OQ.appl LIKE (CP.CampaignPrefix + '%')
		AND CP.RegionId = @OnelinkRegion
		AND CP.ScriptId = @ScriptId
		AND CP.Active = 1
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON OQ.DialerField1 = PD01.DF_PRS_ID --COBORROWER ACCOUNT NUMBER
	INNER JOIN 
	(
		SELECT DISTINCT
			GA01.DF_PRS_ID_BR,
			GA01.DF_PRS_ID_EDS
		FROM
			ODW..GA01_APP GA01
		WHERE
			GA01.AC_EDS_TYP = 'C'
	) GA01
		ON GA01.DF_PRS_ID_EDS = PD01.DF_PRS_ID
	INNER JOIN ODW..PD01_PDM_INF PD01_BOR
		ON GA01.DF_PRS_ID_BR = PD01_BOR.DF_PRS_ID
WHERE
	PD01_BOR.DF_SPE_ACC_ID = OQ.DialerField4

UNION

--ONELINK REFERENCE POPULATION 
SELECT DISTINCT
	PD01.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
	NULL AS UDW_CoborrowerAccountNumber,
	OQ.rowid, 
	OQ.call_type, 
	OQ.listid, 
	OQ.appl, 
	OQ.DialerField7 AS lm_filler2, --Dialer field 7 contains borrower account number for LGP
	OQ.areacode, 
	OQ.phone, 
	OQ.addi_status, 
	OQ.[status], 
	OQ.tsr,
	OQ.act_date, 
	OQ.act_time, 
	OQ.vox_file_name, 
	OQ.time_connect,
	OQ.TimeACW,
	OQ.TimeHold,
	OQ.AgentHold,
	OQ.Filler1,
	OQ.Filler3,
	OQ.Filler4,
	OQ.d_record_id,
	OQ.DialerField1, 
	OQ.DialerField2, 
	OQ.DialerField3, 
	OQ.DialerField4, 
	OQ.DialerField5, 
	OQ.DialerField6, 
	OQ.DialerField7, 
	OQ.DialerField8, 
	OQ.DialerField9, 
	OQ.DialerField10,
	OQ.DialerField11,
	OQ.DialerField12,
	OQ.DialerField13,
	OQ.DialerField14,
	OQ.DialerField15,
	OQ.DialerField16,
	OQ.DialerField17,
	OQ.DialerField18,
	OQ.DialerField19,
	OQ.DialerField20,
	OQ.DialerField21,
	OQ.DialerField22,
	OQ.DialerField23,
	OQ.DialerField24,
	OQ.DialerField25
FROM 
	_OutboundPopulation OQ
	INNER JOIN CampaignPrefixes CP
		ON OQ.appl LIKE (CP.CampaignPrefix + '%')
		AND CP.RegionId = @OnelinkRegion
		AND CP.ScriptId = @ScriptId
		AND CP.Active = 1
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON OQ.DialerField7 = PD01.DF_SPE_ACC_ID
	INNER JOIN ODW..BR03_BR_REF BR03
		ON BR03.DF_PRS_ID_RFR = OQ.DialerField1
		AND BR03.DF_PRS_ID_BR = PD01.DF_PRS_ID
WHERE
	BR03.BC_STA_BR03 = 'A'

UNION

--START COMPASS POPULATION

SELECT DISTINCT
	UPD10.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
	NULL AS UDW_CoborrowerAccountNumber,
	OQ.rowid, 
	OQ.call_type, 
	OQ.listid, 
	OQ.appl, 
	OQ.DialerField4 AS lm_filler2, 
	OQ.areacode, 
	OQ.phone, 
	OQ.addi_status, 
	OQ.[status], 
	OQ.tsr,
	OQ.act_date, 
	OQ.act_time, 
	OQ.vox_file_name, 
	OQ.time_connect,
	OQ.TimeACW,
	OQ.TimeHold,
	OQ.AgentHold,
	OQ.Filler1,
	OQ.Filler3,
	OQ.Filler4,
	OQ.d_record_id,
	OQ.DialerField1, 
	OQ.DialerField2, 
	OQ.DialerField3, 
	OQ.DialerField4, 
	OQ.DialerField5, 
	OQ.DialerField6, 
	OQ.DialerField7, 
	OQ.DialerField8, 
	OQ.DialerField9, 
	OQ.DialerField10,
	OQ.DialerField11,
	OQ.DialerField12,
	OQ.DialerField13,
	OQ.DialerField14,
	OQ.DialerField15,
	OQ.DialerField16,
	OQ.DialerField17,
	OQ.DialerField18,
	OQ.DialerField19,
	OQ.DialerField20,
	OQ.DialerField21,
	OQ.DialerField22,
	OQ.DialerField23,
	OQ.DialerField24,
	OQ.DialerField25
FROM 
	_OutboundPopulation OQ
	INNER JOIN CampaignPrefixes CP
		ON OQ.appl LIKE (CP.CampaignPrefix + '%')
		AND CP.RegionId = @CompassRegion
		AND CP.ScriptId = @ScriptId
		AND CP.Active = 1
	INNER JOIN 
	(
		SELECT DISTINCT
			UPD10.DF_PRS_ID,
			UPD10.DF_SPE_ACC_ID
		FROM
			UDW..PD10_PRS_NME UPD10
			INNER JOIN UDW..LN10_LON ULN10
				ON ULN10.BF_SSN = UPD10.DF_PRS_ID
		WHERE
			ULN10.LC_STA_LON10 = 'R'
	) UPD10
		ON OQ.DialerField4 = UPD10.DF_SPE_ACC_ID
		AND OQ.DialerField1 = UPD10.DF_SPE_ACC_ID

UNION

SELECT DISTINCT
	UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID, --BORROWER ACCOUNT NUMBER
	UPD10.DF_SPE_ACC_ID AS UDW_CoborrowerAccountNumber, --COBORROWER ACCOUNT NUMBER
	OQ.rowid, 
	OQ.call_type, 
	OQ.listid, 
	OQ.appl, 
	OQ.DialerField4 AS lm_filler2, 
	OQ.areacode, 
	OQ.phone, 
	OQ.addi_status, 
	OQ.[status], 
	OQ.tsr, 
	OQ.act_date, 
	OQ.act_time, 
	OQ.vox_file_name, 
	OQ.time_connect,
	OQ.TimeACW,
	OQ.TimeHold,
	OQ.AgentHold,
	OQ.Filler1,
	OQ.Filler3,
	OQ.Filler4,
	OQ.d_record_id,
	OQ.DialerField1, 
	OQ.DialerField2, 
	OQ.DialerField3, 
	OQ.DialerField4, 
	OQ.DialerField5, 
	OQ.DialerField6, 
	OQ.DialerField7, 
	OQ.DialerField8, 
	OQ.DialerField9, 
	OQ.DialerField10,
	OQ.DialerField11,
	OQ.DialerField12,
	OQ.DialerField13,
	OQ.DialerField14,
	OQ.DialerField15,
	OQ.DialerField16,
	OQ.DialerField17,
	OQ.DialerField18,
	OQ.DialerField19,
	OQ.DialerField20,
	OQ.DialerField21,
	OQ.DialerField22,
	OQ.DialerField23,
	OQ.DialerField24,
	OQ.DialerField25
FROM 
	_OutboundPopulation OQ
	INNER JOIN CampaignPrefixes CP
		ON OQ.appl LIKE (CP.CampaignPrefix + '%')
		AND CP.RegionId = @CompassRegion
		AND CP.ScriptId = @ScriptId
		AND CP.Active = 1
	INNER JOIN UDW..PD10_PRS_NME UPD10
		ON OQ.DialerField1 = UPD10.DF_SPE_ACC_ID --COBORROWER ACCOUNT NUMBER
	INNER JOIN
	(
		SELECT DISTINCT
			LN20.LF_EDS,
			PD10.DF_SPE_ACC_ID
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW..LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
			INNER JOIN UDW..LN16_LON_DLQ_HST LN16
				ON LN16.BF_SSN = LN10.BF_SSN
				AND LN16.LN_SEQ = LN10.LN_SEQ
				AND LN16.LC_STA_LON16 = '1'
				AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)
		WHERE
			LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
	) UENDS
		ON UENDS.LF_EDS = UPD10.DF_PRS_ID
		AND UENDS.DF_SPE_ACC_ID = OQ.DialerField4

	DELETE FROM _OutboundPopulation 
GO