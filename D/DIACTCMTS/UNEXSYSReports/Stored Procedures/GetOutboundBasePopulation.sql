CREATE PROCEDURE [dbo].[GetOutboundBasePopulation]
	@Campaigns CampaignPrefixes READONLY
AS

SELECT 
	CA.LCMCAId AS rowid, 
	CA.CallType AS call_type, 
	CA.ListId AS listid, 
	CA.CampaignId AS appl, --Call Campaign, There is also CampaignGroup but I dont think that is what were looking for
	CA.Bussfld1 AS lm_filler2, 
	ISNULL(LEFT(CA.CValue, 3), '') AS areacode, 
	ISNULL(SUBSTRING(CA.CValue, 4, LEN(CA.CValue) - 3), '') AS phone, 
	CA.[Status] AS addi_status, --maybe not needed CH.addi_status, --AdditionalDispositionCode 
	O.DISPLAYNAME AS [status], --DispositionCode
	CA.UserId AS tsr, --AgentId
	CAST(CA.StartTime AS DATE) AS act_date, 
	CA.StartTime AS act_time, 
	F.FileId AS vox_file_name, 
	CA.Duration AS time_connect,			
	CA.ReservationCallDuration AS TimeACW,
	CA.ReservationCallDuration AS TimeHold,
	CA.ReservationCallDuration AS AgentHold,
	'' AS Filler1, --CH.lm_filler1 AS Filler1, --string STATE, maybe in ContactDetail maybe not needed
	NULL AS Filler3, --CH.lm_filler3 AS Filler3, --int maybe not needed
	NULL AS Filler4, --CH.lm_filler4 AS Filler4, --int maybe not needed
	NULL AS d_record_id, --CH.d_record_id, --Not needed
	CA.Bussfld1 AS DialerField1,
	CA.Bussfld2 AS DialerField2,
	CA.Bussfld3 AS DialerField3,
	CA.Bussfld4 AS DialerField4,
	CA.Bussfld5 AS DialerField5,
	CA.Bussfld6 AS DialerField6,
	CA.Bussfld7 AS DialerField7,
	CA.Bussfld8 AS DialerField8,
	CA.Bussfld9 AS DialerField9,
	CA.Bussfld10 AS DialerField10,
	CA.Bussfld11 AS DialerField11,
	CA.Bussfld12 AS DialerField12,
	CA.Bussfld13 AS DialerField13,
	CA.Bussfld14 AS DialerField14,
	CA.Bussfld15 AS DialerField15,
	CA.Bussfld16 AS DialerField16,
	CA.Bussfld17 AS DialerField17,
	CA.Bussfld18 AS DialerField18,
	CA.Bussfld19 AS DialerField19,
	CA.Bussfld20 AS DialerField20,
	CA.Bussfld21 AS DialerField21,
	CA.Bussfld22 AS DialerField22,
	CA.Bussfld23 AS DialerField23,
	CA.Bussfld24 AS DialerField24,
	CA.Bussfld25 AS DialerField25
FROM 
	UNEXSYSReports.dbo.RPT_CallActivity CA 
	LEFT JOIN UNEXSYSReports.dbo.OBD_OUTCOME O
		ON O.OUTCOMEID = CA.CallOutcome
		--AND O.DELETED = '0'
		AND O.ChannelType IN ('0', '2')
	LEFT JOIN UNEXSYSReports.dbo.OBD_GUActivity F
		ON F.FileId = CA.GID
	LEFT JOIN @Campaigns C
		ON CA.CampaignId LIKE (C.CampaignPrefix + '%')
WHERE 
	CA.CallType != 5 
	AND CA.[Status] IS NOT NULL
	AND CAST(CA.StartTime AS DATE) >= CAST(GETDATE() - 7 AS DATE)
	AND CA.CValue IS NOT NULL
	AND C.CampaignPrefix IS NOT NULL

RETURN 0
