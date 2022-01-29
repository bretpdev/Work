CREATE PROCEDURE [rccallhist].[InsertInboundCalls]
(
	@InboundCalls InboundCalls READONLY
)
AS

BEGIN
	DELETE FROM _InboundCalls

	INSERT INTO 
		_InboundCalls
	SELECT
		RowId,
		CallType,
		ListId,
		CampaignId,
		AccountNumber,
		AreaCode,
		Phone,
		AdditionalStatus,
		[Status],
		AgentId,
		StartTime,
		VoxFileName,
		TimeConnect,
		TimeACW,
		TimeHold,
		AgentHold,
		SessionSeqNum,
		NodeId,
		ProfileId,
		DialerField1,
		DialerField2,
		DialerField3,
		DialerField4,
		DialerField5,
		DialerField6,
		DialerField7,
		DialerField8,
		DialerField9,
		DialerField10
	FROM
		@InboundCalls

	MERGE rccallhist.InboundCalls AS [Target]
	USING
	(
		SELECT
			RowId,
			CallType,
			ListId,
			CampaignId,
			AccountNumber,
			AreaCode,
			Phone,
			AdditionalStatus,
			[Status],
			AgentId,
			StartTime,
			VoxFileName,
			TimeConnect,
			TimeACW,
			TimeHold,
			AgentHold,
			SessionSeqNum,
			NodeId,
			ProfileId,
			DialerField1,
			DialerField2,
			DialerField3,
			DialerField4,
			DialerField5,
			DialerField6,
			DialerField7,
			DialerField8,
			DialerField9,
			DialerField10
		FROM
			_InboundCalls
	) [Source]
	ON
		[Target].RowId = [Source].RowId
		AND [Target].AgentId = [Source].AgentId
		AND [Target].SessionSeqNum = [Source].SessionSeqNum
		AND [Target].NodeId = [Source].NodeId
		AND [Target].ProfileId = [Source].ProfileId
	WHEN MATCHED THEN UPDATE SET
		[Target].RowId = [Source].RowId,
		[Target].CallType = [Source].CallType,
		[Target].ListId = [Source].ListId,
		[Target].CampaignId = [Source].CampaignId,
		[Target].AccountNumber = [Source].AccountNumber,
		[Target].AreaCode = [Source].AreaCode,
		[Target].Phone = [Source].Phone,
		[Target].AdditionalStatus = [Source].AdditionalStatus,
		[Target].[Status] = [Source].[Status],
		[Target].AgentId = [Source].AgentId,
		[Target].StartTime = [Source].StartTime,
		[Target].VoxFileName = [Source].VoxFileName,
		[Target].TimeConnect = [Source].TimeConnect,
		[Target].TimeACW = [Source].TimeACW,
		[Target].TimeHold = [Source].TimeHold,
		[Target].AgentHold = [Source].AgentHold,
		[Target].SessionSeqNum = [Source].SessionSeqNum,
		[Target].NodeId = [Source].NodeId,
		[Target].ProfileId = [Source].ProfileId,
		[Target].DialerField1 = [Source].DialerField1,
		[Target].DialerField2 = [Source].DialerField2,
		[Target].DialerField3 = [Source].DialerField3,
		[Target].DialerField4 = [Source].DialerField4,
		[Target].DialerField5 = [Source].DialerField5,
		[Target].DialerField6 = [Source].DialerField6,
		[Target].DialerField7 = [Source].DialerField7,
		[Target].DialerField8 = [Source].DialerField8,
		[Target].DialerField9 = [Source].DialerField9,
		[Target].DialerField10 = [Source].DialerField10
	WHEN NOT MATCHED THEN INSERT
	(
		RowId,
		CallType,
		ListId,
		CampaignId,
		AccountNumber,
		AreaCode,
		Phone,
		AdditionalStatus,
		[Status],
		AgentId,
		StartTime,
		VoxFileName,
		TimeConnect,
		TimeACW,
		TimeHold,
		AgentHold,
		SessionSeqNum,
		NodeId,
		ProfileId,
		DialerField1,
		DialerField2,
		DialerField3,
		DialerField4,
		DialerField5,
		DialerField6,
		DialerField7,
		DialerField8,
		DialerField9,
		DialerField10
	)
	VALUES
	(
		[Source].RowId,
		[Source].CallType,
		[Source].ListId,
		[Source].CampaignId,
		[Source].AccountNumber,
		[Source].AreaCode,
		[Source].Phone,
		[Source].AdditionalStatus,
		[Source].[Status],
		[Source].AgentId,
		[Source].StartTime,
		[Source].VoxFileName,
		[Source].TimeConnect,
		[Source].TimeACW,
		[Source].TimeHold,
		[Source].AgentHold,
		[Source].SessionSeqNum,
		[Source].NodeId,
		[Source].ProfileId,
		[Source].DialerField1,
		[Source].DialerField2,
		[Source].DialerField3,
		[Source].DialerField4,
		[Source].DialerField5,
		[Source].DialerField6,
		[Source].DialerField7,
		[Source].DialerField8,
		[Source].DialerField9,
		[Source].DialerField10
	);		

	DELETE FROM _InboundCalls
END