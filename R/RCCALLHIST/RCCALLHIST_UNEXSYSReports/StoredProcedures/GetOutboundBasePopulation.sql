CREATE PROCEDURE [rccallhist].[GetOutboundBasePopulation]
	@Campaigns rccallhist.CampaignPrefixes READONLY
AS


SELECT 
	CA.LCMCAId AS RowId, 
	CA.CallType AS CallType, 
	CA.ListId AS ListId, 
	CA.CampaignId AS CampaignId, --Call Campaign, There is also CampaignGroup but I dont think that is what were looking for
	CA.AccountNumber AS AccountNumber, 
	ISNULL(LEFT(CA.CValue, 3), '') AS AreaCode, 
	ISNULL(SUBSTRING(CA.CValue, 4, LEN(CA.CValue) - 3), '') AS Phone, 
	CA.[Status] AS AdditionalStatus,
	O.DISPLAYNAME AS [Status], --DispositionCode
	CA.UserId AS AgentId, --AgentId
	CA.StartTime AS StartTime, 
	F.FileId AS VoxFileName, 
	CA.Duration AS TimeConnect,			
	CA.ReservationCallDuration AS TimeACW,
	CA.ReservationCallDuration AS TimeHold,
	CA.ReservationCallDuration AS AgentHold,
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
	CA.Bussfld25 AS DialerField25,
	CA.Bussfld26 AS DialerField26
FROM 
	UNEXSYSReports.dbo.RPT_CallActivity CA 
	LEFT JOIN UNEXSYSReports.dbo.OBD_OUTCOME O
		ON O.OUTCOMEID = CA.CallOutcome
		AND O.ChannelType IN ('0', '2')
	LEFT JOIN UNEXSYSReports.dbo.OBD_GUActivity F
		ON F.FileId = CA.GID
	LEFT JOIN @Campaigns C
		ON CA.CampaignId LIKE (C.CampaignPrefix + '%')
WHERE 
	CA.CallType != 5 
	AND CA.[Status] IS NOT NULL
	AND CAST(CA.StartTime AS DATE) = CAST(GETDATE() AS DATE)
	AND CA.CValue IS NOT NULL
	AND C.CampaignPrefix IS NOT NULL

RETURN 0
