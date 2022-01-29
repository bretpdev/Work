CREATE PROCEDURE [rccallhist].[GetInboundCalls]
AS

DECLARE @ScriptId VARCHAR(100) = 'RCCALLHIST'
DECLARE @RepayCentsiblyRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'REPAYCENTSIBLY')

SELECT
	CCD.sessionId AS RowId, --Primary Key Field
	CCD.contactType AS CallType,
	CCD.dialingListID AS ListId,
	CCD.CSQName AS CampaignId,
	CCD.accountNumber AS AccountNumber,
	CASE WHEN  CCD.originatorType = 3 THEN ISNULL(CASE WHEN LEN(CCD.originatorDN) >= 3 THEN LEFT(CCD.originatorDN, 3) ELSE CCD.originatorDN END,'') ELSE '' END AS AreaCode,
	CASE WHEN  CCD.originatorType = 3 THEN ISNULL(CASE WHEN LEN(CCD.originatorDN) > 3 THEN SUBSTRING(CCD.originatorDN, 4, LEN(CCD.originatorDN) - 3) ELSE '' END, '') ELSE '' END AS Phone,
	CCD.callResult AS AdditionalStatus,
	CCD.disposition AS [Status],
	CCD.resourceID AS AgentId, --Additional Primary Key Fields
	--CAST(DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), CCD.startDateTime) AS DATE) AS StartTime,
	DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), CCD.startDateTime) AS StartTime,
	'' AS VoxFileName,
	CCD.connectTime AS TimeConnect,
	CCD.workTime AS TimeACW,
	CCD.ringTime AS TimeHold,
	CCD.holdTime AS AgentHold,
	--CCD.contactid AS Filler1,
	CCD.sessionSeqNum AS SessionSeqNum, --Additional Primary Key Fields
	CCD.nodeID AS NodeId, --Additional Primary Key Fields
	CCD.profileID AS ProfileId, --Additional Primary Key Fields
	CCD.customVariable1 AS DialerField1,
	CCD.customVariable2 AS DialerField2,
	CCD.customVariable3 AS DialerField3,
	CCD.customVariable4 AS DialerField4,
	CCD.customVariable5 AS DialerField5,
	CCD.customVariable6 AS DialerField6,
	CCD.customVariable7 AS DialerField7,
	CCD.customVariable8 AS DialerField8,
	CCD.customVariable9 AS DialerField9,
	CCD.customVariable10 AS DialerField10
FROM
	--INFORMIX Database
	OPENQUERY(uccx,
	'
		SELECT
			CCD.sessionId,
			CCD.contactType,
			CCD.dialingListID,
			CCD.campaignId,
			CCD.accountNumber,
			CCD.originatorType,
			CCD.originatorDN,
			CCD.contactDisposition,
			CQD.disposition,
			CSQ.CSQName,
			ACD.callResult,
			ACD.resourceID,
			CCD.startDateTime,
			CCD.connectTime,
			ACD.workTime,
			ACD.holdTime,
			ACD.ringTime,
			CCD.sessionSeqNum,
			CCD.nodeID,
			CCD.profileID,
			CCD.contactid,
			CCD.customVariable1,
			CCD.customVariable2,
			CCD.customVariable3,
			CCD.customVariable4,
			CCD.customVariable5,
			CCD.customVariable6,
			CCD.customVariable7,
			CCD.customVariable8,
			CCD.customVariable9,
			CCD.customVariable10
		FROM
			AgentConnectionDetail ACD, ContactCallDetail CCD, ContactQueueDetail CQD, ContactServiceQueue CSQ
		WHERE
			--ContactCallDetail Inner Join
			CCD.contactid = ACD.contactid
			AND CCD.sessionID = ACD.sessionID
			AND CCD.sessionSeqNum = ACD.sessionSeqNum
			AND CCD.nodeID = ACD.nodeID
			AND CCD.profileID = ACD.profileID
			--ContactQueueDetail INNER JOIN	
			AND CQD.contactid = CCD.contactid
			AND CQD.sessionID = CCD.sessionID
			AND CQD.sessionSeqNum = CCD.sessionSeqNum
			AND CQD.profileID = CCD.profileID
			AND CQD.nodeID = CCD.nodeID
			AND CQD.targetType = 0
			--ContactServiceQueue Inner Join
			AND CQD.targetID = CSQ.recordID
			AND CSQ.active

			
	') CCD
	INNER JOIN CampaignPrefixes CP
		ON CCD.CSQName LIKE (CP.CampaignPrefix + '%')
		AND CP.RegionId IN (@RepayCentsiblyRegion)
		AND CP.ScriptId = @ScriptId
		AND CP.Active = 1
WHERE
	CAST(CCD.startDateTime AS DATE) = CAST(GETDATE() AS DATE)
	AND CCD.originatorType = 3 --Incoming call
