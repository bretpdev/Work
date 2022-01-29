USE NobleCalls
GO

UPDATE
	O
SET
	O.OUTCOMEGROUPID = Staging.OUTCOMEGROUPID,
	O.OUTCOMEID = Staging.OUTCOMEID,
	O.DISPLAYNAME = Staging.DISPLAYNAME,
	O.BUSINESSOUTCOME = Staging.BUSINESSOUTCOME,
	O.CHANNELTYPE = Staging.CHANNELTYPE,
	O.RPCType = Staging.RPCType,
	O.BOParentId = Staging.BOParentId,
	O.IsLiveCallOutcome = Staging.IsLiveCallOutcome,
	O.DESCRIPTION = Staging.DESCRIPTION
FROM
	agenttabl.OBD_OutcomeUNEXSYS O
	INNER JOIN agenttabl.OBD_OutcomeUNEXSYS_Staging Staging
		ON O.OUTCOMEGROUPID = Staging.OUTCOMEGROUPID
		AND O.OUTCOMEID = Staging.OUTCOMEID
		AND O.DISPLAYNAME = Staging.DISPLAYNAME
		AND O.BUSINESSOUTCOME = Staging.BUSINESSOUTCOME
		AND O.CHANNELTYPE = Staging.CHANNELTYPE
		AND O.RPCType = Staging.RPCType
		AND O.BOParentId = Staging.BOParentId
		AND O.IsLiveCallOutcome = Staging.IsLiveCallOutcome
		AND O.DESCRIPTION = Staging.DESCRIPTION


UPDATE
	R
SET
	R.LCMCAId = Staging.LCMCAId,
	R.RefCallID = Staging.RefCallID,
	R.ContactId = Staging.ContactId,
	R.CampaignGroup = Staging.CampaignGroup,
	R.CampaignId = Staging.CampaignId,
	R.GID = Staging.GID,
	R.ListId = Staging.ListId,
	R.DialplanName = Staging.DialplanName,
	R.ConditionId = Staging.ConditionId,
	R.CValue = Staging.CValue,
	R.StartTime = Staging.StartTime,
	R.EndTime = Staging.EndTime,
	R.Duration = Staging.Duration,
	R.CallMode = Staging.CallMode,
	R.CallOutcome = Staging.CallOutcome,
	R.Bussfld1 = Staging.Bussfld1,
	R.Bussfld2 = Staging.Bussfld2,
	R.Bussfld3 = Staging.Bussfld3,
	R.Bussfld4 = Staging.Bussfld4,
	R.Bussfld5 = Staging.Bussfld5,
	R.Bussfld6 = Staging.Bussfld6,
	R.Bussfld7 = Staging.Bussfld7,
	R.Bussfld8 = Staging.Bussfld8,
	R.Bussfld9 = Staging.Bussfld9,
	R.Bussfld10 = Staging.Bussfld10,
	R.Bussfld11 = Staging.Bussfld11,
	R.Bussfld12 = Staging.Bussfld12,
	R.Bussfld13 = Staging.Bussfld13,
	R.Bussfld14 = Staging.Bussfld14,
	R.Bussfld15 = Staging.Bussfld15,
	R.Bussfld16 = Staging.Bussfld16,
	R.Bussfld17 = Staging.Bussfld17,
	R.Bussfld18 = Staging.Bussfld18,
	R.Bussfld19 = Staging.Bussfld19,
	R.Bussfld20 = Staging.Bussfld20,
	R.Bussfld21 = Staging.Bussfld21,
	R.Bussfld22 = Staging.Bussfld22,
	R.Bussfld23 = Staging.Bussfld23,
	R.Bussfld24 = Staging.Bussfld24,
	R.Bussfld25 = Staging.Bussfld25,
	R.ScrubListId = Staging.ScrubListId,
	R.paceId = Staging.paceId,
	R.MakeCallChannel = Staging.MakeCallChannel,
	R.TargetCampaignId = Staging.TargetCampaignId,
	R.TargetContactId = Staging.TargetContactId,
	R.AgentPeripheralNumber = Staging.AgentPeripheralNumber,
	R.RouterCallKey = Staging.RouterCallKey,
	R.RouterCallKeyDay = Staging.RouterCallKeyDay,
	R.RecoveryKey = Staging.RecoveryKey,
	R.TargetAmount = Staging.TargetAmount,
	R.AgentComments = Staging.AgentComments,
	R.ChannelType = Staging.ChannelType,
	R.DialerTime = Staging.DialerTime,
	R.ChildListID = Staging.ChildListID,
	R.DeliveredType = Staging.DeliveredType,
	R.TargetCampaignGroup = Staging.TargetCampaignGroup,
	R.CallType = Staging.CallType,
	R.UserId = Staging.UserId,
	R.AccountNumber = Staging.AccountNumber,
	R.ContactTries = Staging.ContactTries,
	R.Status = Staging.Status,
	R.Callbackdatetime = Staging.Callbackdatetime,
	R.DeliveredTime = Staging.DeliveredTime,
	R.CallStartTime = Staging.CallStartTime,
	R.ScheduledDeliveryTime = Staging.ScheduledDeliveryTime,
	R.TXFRCALLCHANNEL = Staging.TXFRCALLCHANNEL,
	R.RECORDED = Staging.RECORDED,
	R.SKILLGROUPSKILLTARGETID = Staging.SKILLGROUPSKILLTARGETID,
	R.ICMId = Staging.ICMId,
	R.IsWireless = Staging.IsWireless,
	R.BUSSFLD26 = Staging.BUSSFLD26,
	R.CallReferenceID = Staging.CallReferenceID,
	R.ICRCallKey = Staging.ICRCallKey,
	R.PeripheralCallKey = Staging.PeripheralCallKey,
	R.RouterCallKeySequenceNumber = Staging.RouterCallKeySequenceNumber,
	R.ReservationCallDuration = Staging.ReservationCallDuration,
	R.PreviewTime = Staging.PreviewTime,
	R.DialingMode = Staging.DialingMode,
	R.SFUID = Staging.SFUID,
	R.ContactDetail = Staging.ContactDetail,
	R.SFLeadId = Staging.SFLeadId,
	R.SFCampaignId = Staging.SFCampaignId,
	R.SFContactId = Staging.SFContactId,
	R.CallBackRequestedBy = Staging.CallBackRequestedBy,
	R.CallbackRegisteredType = Staging.CallbackRegisteredType,
	R.CallBackAttemptType = Staging.CallBackAttemptType,
	R.CurrentCycle = Staging.CurrentCycle,
	R.IsCurrentCycleCompleted = Staging.IsCurrentCycleCompleted,
	R.Campaignkey = Staging.Campaignkey,
	R.PreviousLeadScore = Staging.PreviousLeadScore,
	R.CurrentLeadScore = Staging.CurrentLeadScore,
	R.StatusReasonId = Staging.StatusReasonId,
	R.StatusChangedBy = Staging.StatusChangedBy,
	R.StatusChangedAt = Staging.StatusChangedAt,
	R.ActualCValue = Staging.ActualCValue,
	R.DNCBussField = Staging.DNCBussField,
	R.BlockedBy = Staging.BlockedBy,
	R.IsInbound = Staging.IsInbound,
	R.DeviceID = Staging.DeviceID,
	R.OverridePEWCValidation = Staging.OverridePEWCValidation,
	R.DialerAgentCallback = Staging.DialerAgentCallback,
	R.DNCStartDate = Staging.DNCStartDate,
	R.DNCEndDate = Staging.DNCEndDate,
	R.DNCType = Staging.DNCType,
	R.IdentityAuthenticationEnabled = Staging.IdentityAuthenticationEnabled,
	R.IdentityAuthenticationSuccess = Staging.IdentityAuthenticationSuccess,
	R.SMSTransactionCount = Staging.SMSTransactionCount,
	R.StateLawGroupName = Staging.StateLawGroupName,
	R.DNCBussField1 = Staging.DNCBussField1,
	R.PreviewDuration = Staging.PreviewDuration,
	R.NextScheduleDateTime = Staging.NextScheduleDateTime,
	R.NextScheduleMode = Staging.NextScheduleMode,
	R.NICEContactId = Staging.NICEContactId,
	R.TotalPrimaryAuth = Staging.TotalPrimaryAuth,
	R.VerifiedPrimaryAuth = Staging.VerifiedPrimaryAuth,
	R.TotalSecondaryAuth = Staging.TotalSecondaryAuth,
	R.VerifiedSecondaryAuth = Staging.VerifiedSecondaryAuth,
	R.ComputedDurationInMS = Staging.ComputedDurationInMS,
	R.CampaignCategoryID = Staging.CampaignCategoryID,
	R.DNCCampaignCategoryID = Staging.DNCCampaignCategoryID,
	R.DiallerReferenceID = Staging.DiallerReferenceID
FROM
	agenttabl.RPT_CallActivityUNEXSYS R
	INNER JOIN agenttabl.RPT_CallActivityUNEXSYS_Staging Staging
		ON R.CallId = Staging.CallId
WHERE
	LTRIM(RTRIM(R.CallId)) != ''
