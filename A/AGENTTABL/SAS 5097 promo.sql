USE [NobleCalls]
GO

/****** Object:  Table [agenttabl].[RPT_CallActivityUNEXSYS_Staging]    Script Date: 8/30/2021 9:47:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging](
	[CAId] [bigint] IDENTITY(1,1) NOT NULL,
	[LCMCAId] [bigint] NOT NULL,
	[CallId] [nvarchar](38) NOT NULL,
	[RefCallID] [nvarchar](38) NULL,
	[ContactId] [bigint] NULL,
	[CampaignGroup] [nvarchar](64) NULL,
	[CampaignId] [nvarchar](64) NULL,
	[GID] [bigint] NULL,
	[ListId] [int] NOT NULL,
	[DialplanName] [nvarchar](64) NULL,
	[ConditionId] [bigint] NULL,
	[CValue] [nvarchar](64) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Duration] [bigint] NULL,
	[CallMode] [nvarchar](4) NULL,
	[CallOutcome] [nvarchar](8) NULL,
	[Bussfld1] [nvarchar](128) NULL,
	[Bussfld2] [nvarchar](128) NULL,
	[Bussfld3] [nvarchar](128) NULL,
	[Bussfld4] [nvarchar](128) NULL,
	[Bussfld5] [nvarchar](128) NULL,
	[Bussfld6] [nvarchar](128) NULL,
	[Bussfld7] [nvarchar](128) NULL,
	[Bussfld8] [nvarchar](128) NULL,
	[Bussfld9] [nvarchar](128) NULL,
	[Bussfld10] [nvarchar](128) NULL,
	[Bussfld11] [nvarchar](128) NULL,
	[Bussfld12] [nvarchar](128) NULL,
	[Bussfld13] [nvarchar](128) NULL,
	[Bussfld14] [nvarchar](128) NULL,
	[Bussfld15] [nvarchar](128) NULL,
	[Bussfld16] [nvarchar](128) NULL,
	[Bussfld17] [nvarchar](128) NULL,
	[Bussfld18] [nvarchar](128) NULL,
	[Bussfld19] [nvarchar](128) NULL,
	[Bussfld20] [nvarchar](128) NULL,
	[Bussfld21] [nvarchar](max) NULL,
	[Bussfld22] [nvarchar](max) NULL,
	[Bussfld23] [nvarchar](max) NULL,
	[Bussfld24] [nvarchar](max) NULL,
	[Bussfld25] [nvarchar](max) NULL,
	[ScrubListId] [int] NULL,
	[paceId] [bigint] NULL,
	[MakeCallChannel] [nvarchar](64) NULL,
	[TargetCampaignId] [nvarchar](64) NULL,
	[TargetContactId] [bigint] NULL,
	[AgentPeripheralNumber] [nvarchar](64) NULL,
	[RouterCallKey] [int] NULL,
	[RouterCallKeyDay] [int] NULL,
	[RecoveryKey] [float] NULL,
	[TargetAmount] [float] NULL,
	[AgentComments] [nvarchar](4000) NULL,
	[ChannelType] [int] NULL,
	[DialerTime] [datetime] NULL,
	[ChildListID] [int] NULL,
	[DeliveredType] [nvarchar](32) NULL,
	[TargetCampaignGroup] [nvarchar](64) NULL,
	[CallType] [tinyint] NULL,
	[UserId] [nvarchar](32) NULL,
	[AccountNumber] [nvarchar](30) NULL,
	[ContactTries] [tinyint] NULL,
	[Status] [int] NULL,
	[Callbackdatetime] [datetime] NULL,
	[DeliveredTime] [datetime] NULL,
	[CallStartTime] [datetime] NULL,
	[ScheduledDeliveryTime] [datetime] NULL,
	[TXFRCALLCHANNEL] [char](1) NULL,
	[RECORDED] [char](1) NULL,
	[SKILLGROUPSKILLTARGETID] [int] NULL,
	[ICMId] [int] NULL,
	[IsWireless] [bit] NULL,
	[BUSSFLD26] [nvarchar](128) NULL,
	[CallReferenceID] [nvarchar](32) NULL,
	[ICRCallKey] [int] NULL,
	[PeripheralCallKey] [int] NULL,
	[RouterCallKeySequenceNumber] [int] NULL,
	[ReservationCallDuration] [int] NULL,
	[PreviewTime] [datetime] NULL,
	[DialingMode] [nvarchar](32) NULL,
	[SFUID] [nvarchar](max) NULL,
	[ContactDetail] [nvarchar](max) NULL,
	[SFLeadId] [nvarchar](200) NULL,
	[SFCampaignId] [nvarchar](200) NULL,
	[SFContactId] [nvarchar](max) NULL,
	[CallBackRequestedBy] [nvarchar](64) NULL,
	[CallbackRegisteredType] [int] NULL,
	[CallBackAttemptType] [nvarchar](3) NULL,
	[CurrentCycle] [int] NULL,
	[IsCurrentCycleCompleted] [bit] NULL,
	[Campaignkey] [int] NOT NULL,
	[PreviousLeadScore] [int] NULL,
	[CurrentLeadScore] [int] NULL,
	[StatusReasonId] [int] NULL,
	[StatusChangedBy] [nvarchar](128) NULL,
	[StatusChangedAt] [datetime] NULL,
	[ActualCValue] [nvarchar](255) NULL,
	[DNCBussField] [nvarchar](128) NULL,
	[BlockedBy] [nvarchar](128) NULL,
	[IsInbound] [bit] NULL,
	[DeviceID] [nvarchar](255) NULL,
	[OverridePEWCValidation] [bit] NOT NULL,
	[DialerAgentCallback] [bit] NOT NULL,
	[DNCStartDate] [datetime] NULL,
	[DNCEndDate] [datetime] NULL,
	[DNCType] [nvarchar](1) NULL,
	[IdentityAuthenticationEnabled] [bit] NOT NULL,
	[IdentityAuthenticationSuccess] [bit] NOT NULL,
	[SMSTransactionCount] [int] NOT NULL,
	[StateLawGroupName] [nvarchar](256) NULL,
	[DNCBussField1] [nvarchar](128) NULL,
	[PreviewDuration] [int] NULL,
	[NextScheduleDateTime] [nvarchar](50) NULL,
	[NextScheduleMode] [nvarchar](50) NULL,
	[NICEContactId] [nvarchar](64) NULL,
	[TotalPrimaryAuth] [int] NULL,
	[VerifiedPrimaryAuth] [int] NULL,
	[TotalSecondaryAuth] [int] NULL,
	[VerifiedSecondaryAuth] [int] NULL,
	[ComputedDurationInMS] [int] NULL,
	[CampaignCategoryID] [int] NULL,
	[DNCCampaignCategoryID] [nvarchar](max) NULL,
	[DiallerReferenceID] [nvarchar](64) NULL,
 CONSTRAINT [PK__OBD_CALLACTIVITY__7F60ED60] PRIMARY KEY CLUSTERED 
(
	[CAId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  CONSTRAINT [DF__OBD_CALLA__RECOR__19DFD96C]  DEFAULT ('0') FOR [RECORDED]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [ICMId]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [Campaignkey]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [OverridePEWCValidation]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [DialerAgentCallback]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [IdentityAuthenticationEnabled]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [IdentityAuthenticationSuccess]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS_Staging] ADD  DEFAULT ((0)) FOR [SMSTransactionCount]
GO


CREATE TABLE [agenttabl].[RPT_CallActivityUNEXSYS](
	[CAId] [bigint] IDENTITY(1,1) NOT NULL,
	[LCMCAId] [bigint] NOT NULL,
	[CallId] [nvarchar](38) NOT NULL,
	[RefCallID] [nvarchar](38) NULL,
	[ContactId] [bigint] NULL,
	[CampaignGroup] [nvarchar](64) NULL,
	[CampaignId] [nvarchar](64) NULL,
	[GID] [bigint] NULL,
	[ListId] [int] NOT NULL,
	[DialplanName] [nvarchar](64) NULL,
	[ConditionId] [bigint] NULL,
	[CValue] [nvarchar](64) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Duration] [bigint] NULL,
	[CallMode] [nvarchar](4) NULL,
	[CallOutcome] [nvarchar](8) NULL,
	[Bussfld1] [nvarchar](128) NULL,
	[Bussfld2] [nvarchar](128) NULL,
	[Bussfld3] [nvarchar](128) NULL,
	[Bussfld4] [nvarchar](128) NULL,
	[Bussfld5] [nvarchar](128) NULL,
	[Bussfld6] [nvarchar](128) NULL,
	[Bussfld7] [nvarchar](128) NULL,
	[Bussfld8] [nvarchar](128) NULL,
	[Bussfld9] [nvarchar](128) NULL,
	[Bussfld10] [nvarchar](128) NULL,
	[Bussfld11] [nvarchar](128) NULL,
	[Bussfld12] [nvarchar](128) NULL,
	[Bussfld13] [nvarchar](128) NULL,
	[Bussfld14] [nvarchar](128) NULL,
	[Bussfld15] [nvarchar](128) NULL,
	[Bussfld16] [nvarchar](128) NULL,
	[Bussfld17] [nvarchar](128) NULL,
	[Bussfld18] [nvarchar](128) NULL,
	[Bussfld19] [nvarchar](128) NULL,
	[Bussfld20] [nvarchar](128) NULL,
	[Bussfld21] [nvarchar](max) NULL,
	[Bussfld22] [nvarchar](max) NULL,
	[Bussfld23] [nvarchar](max) NULL,
	[Bussfld24] [nvarchar](max) NULL,
	[Bussfld25] [nvarchar](max) NULL,
	[ScrubListId] [int] NULL,
	[paceId] [bigint] NULL,
	[MakeCallChannel] [nvarchar](64) NULL,
	[TargetCampaignId] [nvarchar](64) NULL,
	[TargetContactId] [bigint] NULL,
	[AgentPeripheralNumber] [nvarchar](64) NULL,
	[RouterCallKey] [int] NULL,
	[RouterCallKeyDay] [int] NULL,
	[RecoveryKey] [float] NULL,
	[TargetAmount] [float] NULL,
	[AgentComments] [nvarchar](4000) NULL,
	[ChannelType] [int] NULL,
	[DialerTime] [datetime] NULL,
	[ChildListID] [int] NULL,
	[DeliveredType] [nvarchar](32) NULL,
	[TargetCampaignGroup] [nvarchar](64) NULL,
	[CallType] [tinyint] NULL,
	[UserId] [nvarchar](32) NULL,
	[AccountNumber] [nvarchar](30) NULL,
	[ContactTries] [tinyint] NULL,
	[Status] [int] NULL,
	[Callbackdatetime] [datetime] NULL,
	[DeliveredTime] [datetime] NULL,
	[CallStartTime] [datetime] NULL,
	[ScheduledDeliveryTime] [datetime] NULL,
	[TXFRCALLCHANNEL] [char](1) NULL,
	[RECORDED] [char](1) NULL,
	[SKILLGROUPSKILLTARGETID] [int] NULL,
	[ICMId] [int] NULL,
	[IsWireless] [bit] NULL,
	[BUSSFLD26] [nvarchar](128) NULL,
	[CallReferenceID] [nvarchar](32) NULL,
	[ICRCallKey] [int] NULL,
	[PeripheralCallKey] [int] NULL,
	[RouterCallKeySequenceNumber] [int] NULL,
	[ReservationCallDuration] [int] NULL,
	[PreviewTime] [datetime] NULL,
	[DialingMode] [nvarchar](32) NULL,
	[SFUID] [nvarchar](max) NULL,
	[ContactDetail] [nvarchar](max) NULL,
	[SFLeadId] [nvarchar](200) NULL,
	[SFCampaignId] [nvarchar](200) NULL,
	[SFContactId] [nvarchar](max) NULL,
	[CallBackRequestedBy] [nvarchar](64) NULL,
	[CallbackRegisteredType] [int] NULL,
	[CallBackAttemptType] [nvarchar](3) NULL,
	[CurrentCycle] [int] NULL,
	[IsCurrentCycleCompleted] [bit] NULL,
	[Campaignkey] [int] NOT NULL,
	[PreviousLeadScore] [int] NULL,
	[CurrentLeadScore] [int] NULL,
	[StatusReasonId] [int] NULL,
	[StatusChangedBy] [nvarchar](128) NULL,
	[StatusChangedAt] [datetime] NULL,
	[ActualCValue] [nvarchar](255) NULL,
	[DNCBussField] [nvarchar](128) NULL,
	[BlockedBy] [nvarchar](128) NULL,
	[IsInbound] [bit] NULL,
	[DeviceID] [nvarchar](255) NULL,
	[OverridePEWCValidation] [bit] NOT NULL,
	[DialerAgentCallback] [bit] NOT NULL,
	[DNCStartDate] [datetime] NULL,
	[DNCEndDate] [datetime] NULL,
	[DNCType] [nvarchar](1) NULL,
	[IdentityAuthenticationEnabled] [bit] NOT NULL,
	[IdentityAuthenticationSuccess] [bit] NOT NULL,
	[SMSTransactionCount] [int] NOT NULL,
	[StateLawGroupName] [nvarchar](256) NULL,
	[DNCBussField1] [nvarchar](128) NULL,
	[PreviewDuration] [int] NULL,
	[NextScheduleDateTime] [nvarchar](50) NULL,
	[NextScheduleMode] [nvarchar](50) NULL,
	[NICEContactId] [nvarchar](64) NULL,
	[TotalPrimaryAuth] [int] NULL,
	[VerifiedPrimaryAuth] [int] NULL,
	[TotalSecondaryAuth] [int] NULL,
	[VerifiedSecondaryAuth] [int] NULL,
	[ComputedDurationInMS] [int] NULL,
	[CampaignCategoryID] [int] NULL,
	[DNCCampaignCategoryID] [nvarchar](max) NULL,
	[DiallerReferenceID] [nvarchar](64) NULL,
 CONSTRAINT [PK__OBD_CALLACTIVITY__7F60ED59] PRIMARY KEY CLUSTERED 
(
	[CAId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  CONSTRAINT [DF__OBD_CALLA__RECOR__19DFD96B]  DEFAULT ('0') FOR [RECORDED]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [ICMId]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [Campaignkey]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [OverridePEWCValidation]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [DialerAgentCallback]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [IdentityAuthenticationEnabled]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [IdentityAuthenticationSuccess]
GO

ALTER TABLE [agenttabl].[RPT_CallActivityUNEXSYS] ADD  DEFAULT ((0)) FOR [SMSTransactionCount]
GO


CREATE TABLE [agenttabl].[ResourceUCCX](
	[resourceid] [int] NOT NULL,
	[profileid] [int] NOT NULL,
	[resourceloginid] [varchar](128) NOT NULL,
	[resourcename] [varchar](50) NOT NULL,
	[resourcegroupid] [int] NULL,
	[resourcetype] [smallint] NOT NULL,
	[active] [bit] NOT NULL,
	[autoavail] [bit] NOT NULL,
	[extension] [varchar](50) NOT NULL,
	[orderinrg] [int] NULL,
	[dateinactive] [datetime2](7) NULL,
	[resourceskillmapid] [int] NOT NULL,
	[assignedteamid] [int] NOT NULL,
	[resourcefirstname] [varchar](50) NOT NULL,
	[resourcelastname] [varchar](50) NOT NULL,
	[resourcealias] [varchar](50) NULL,
	[capabilities] [smallint] NOT NULL,
	[resourceemailid] [varchar](255) NULL,
 CONSTRAINT [PK_ResourceUCCX] PRIMARY KEY CLUSTERED 
(
	[resourceid] ASC,
	[profileid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[ReasonCodeLabelMapUCCX](
	[code] [smallint] NOT NULL,
	[label] [varchar](40) NOT NULL,
	[category] [varchar](15) NOT NULL,
	[active] [bit] NULL,
	[dateinactive] [datetime2](7) NULL,
 CONSTRAINT [PK_ReasonCodeLabelMapUCCX] PRIMARY KEY CLUSTERED 
(
	[code] ASC,
	[category] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[OBD_OutcomeUNEXSYS_Staging](
	[OUTCOMEGROUPID] [int] NOT NULL,
	[OUTCOMEID] [int] NOT NULL,
	[DISPLAYNAME] [nvarchar](128) NULL,
	[BUSINESSOUTCOME] [int] NOT NULL,
	[CHANNELTYPE] [int] NOT NULL,
	[RPCType] [nvarchar](8) NULL,
	[BOParentId] [int] NULL,
	[Test] [int] IDENTITY(1,1) NOT NULL,
	[IsLiveCallOutcome] [bit] NULL,
	[DESCRIPTION] [nvarchar](128) NULL
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[OBD_OutcomeUNEXSYS](
	[OUTCOMEGROUPID] [int] NOT NULL,
	[OUTCOMEID] [int] NOT NULL,
	[DISPLAYNAME] [nvarchar](128) NULL,
	[BUSINESSOUTCOME] [int] NOT NULL,
	[CHANNELTYPE] [int] NOT NULL,
	[RPCType] [nvarchar](8) NULL,
	[BOParentId] [int] NULL,
	[Test] [int] IDENTITY(1,1) NOT NULL,
	[IsLiveCallOutcome] [bit] NULL,
	[DESCRIPTION] [nvarchar](128) NULL
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[ContactWrapUpDataUCCX](
	[sessionID] [numeric](18, 0) NOT NULL,
	[sessionSeqNum] [smallint] NOT NULL,
	[resourceID] [int] NOT NULL,
	[wrapupdata] [varchar](160) NOT NULL,
	[nodeid] [smallint] NOT NULL,
	[qindex] [smallint] NOT NULL,
	[startdatetime] [datetime2](7) NOT NULL,
	[wrapupindex] [smallint] NOT NULL,
	[contactid] [varchar](40) NULL,
 CONSTRAINT [PK_ContactWrapUpDataUCCX] PRIMARY KEY CLUSTERED 
(
	[sessionID] ASC,
	[sessionSeqNum] ASC,
	[resourceID] ASC,
	[wrapupdata] ASC,
	[nodeid] ASC,
	[qindex] ASC,
	[startdatetime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[ContactServiceQueueUCCX](
	[contactservicequeueid] [int] NOT NULL,
	[profileid] [int] NOT NULL,
	[csqname] [varchar](50) NOT NULL,
	[resourcepooltype] [smallint] NOT NULL,
	[resourcegroupid] [int] NULL,
	[selectioncriteria] [varchar](30) NOT NULL,
	[skillgroupid] [int] NULL,
	[servicelevel] [int] NOT NULL,
	[servicelevelpercentage] [smallint] NOT NULL,
	[active] [bit] NOT NULL,
	[autowork] [bit] NOT NULL,
	[dateinactive] [datetime2](7) NULL,
	[queuealgorithm] [varchar](30) NOT NULL,
	[recordid] [int] NOT NULL,
	[orderlist] [int] NULL,
	[wrapuptime] [smallint] NULL,
	[prompt] [varchar](256) NOT NULL,
	[privatedata] [image] NULL,
	[queuetype] [smallint] NOT NULL,
	[queuetypename] [varchar](30) NULL,
	[emailauthtype] [smallint] NOT NULL,
	[emailoauthdetails] [text] NULL,
	[accountuserid] [varchar](254) NULL,
	[accountpassword] [varchar](255) NULL,
	[channelproviderid] [int] NULL,
	[reviewqueueid] [int] NULL,
	[routingtype] [varchar](30) NULL,
	[foldername] [varchar](255) NULL,
	[pollinginterval] [int] NULL,
	[snapshotage] [int] NULL,
	[feedid] [varchar](30) NULL,
 CONSTRAINT [PK_ContactServiceQueueUCCX] PRIMARY KEY CLUSTERED 
(
	[recordid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[ContactCallDetailUCCX](
	[sessionid] [numeric](18, 0) NOT NULL,
	[sessionseqnum] [smallint] NOT NULL,
	[nodeid] [smallint] NOT NULL,
	[profileid] [int] NOT NULL,
	[contacttype] [smallint] NOT NULL,
	[contactdisposition] [smallint] NOT NULL,
	[dispositionreason] [varchar](100) NULL,
	[originatortype] [smallint] NOT NULL,
	[originatorid] [int] NULL,
	[originatordn] [varchar](30) NULL,
	[destinationtype] [smallint] NULL,
	[destinationid] [int] NULL,
	[destinationdn] [varchar](30) NULL,
	[startdatetime] [datetime2](7) NOT NULL,
	[enddatetime] [datetime2](7) NOT NULL,
	[gmtoffset] [smallint] NOT NULL,
	[callednumber] [varchar](30) NULL,
	[origcallednumber] [varchar](30) NULL,
	[applicationtaskid] [numeric](18, 0) NULL,
	[applicationid] [int] NULL,
	[applicationname] [varchar](30) NULL,
	[connecttime] [int] NULL,
	[customvariable1] [varchar](40) NULL,
	[customvariable2] [varchar](40) NULL,
	[customvariable3] [varchar](40) NULL,
	[customvariable4] [varchar](40) NULL,
	[customvariable5] [varchar](40) NULL,
	[customvariable6] [varchar](40) NULL,
	[customvariable7] [varchar](40) NULL,
	[customvariable8] [varchar](40) NULL,
	[customvariable9] [varchar](40) NULL,
	[customvariable10] [varchar](40) NULL,
	[accountnumber] [varchar](40) NULL,
	[callerentereddigits] [varchar](40) NULL,
	[badcalltag] [char](1) NULL,
	[transfer] [bit] NULL,
	[redirect] [bit] NULL,
	[conference] [bit] NULL,
	[flowout] [bit] NULL,
	[metservicelevel] [bit] NULL,
	[campaignid] [int] NULL,
	[origprotocolcallref] [varchar](32) NULL,
	[destprotocolcallref] [varchar](32) NULL,
	[callresult] [smallint] NULL,
	[dialinglistid] [int] NULL,
	[contactid] [varchar](40) NULL,
	[lastleg] [bit] NULL,
 CONSTRAINT [PK_ContactCallDetailUCCX] PRIMARY KEY CLUSTERED 
(
	[sessionid] ASC,
	[sessionseqnum] ASC,
	[nodeid] ASC,
	[profileid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[AgentStateDetailUCCX](
	[agentid] [int] NOT NULL,
	[eventdatetime] [datetime2](7) NOT NULL,
	[gmtoffset] [smallint] NOT NULL,
	[eventtype] [smallint] NOT NULL,
	[reasoncode] [smallint] NOT NULL,
	[profileid] [int] NOT NULL,
	[contactid] [varchar](40) NULL,
	[loginsessionid] [varchar](18) NULL,
 CONSTRAINT [PK_AgentStateDetailUCCX] PRIMARY KEY CLUSTERED 
(
	[agentid] ASC,
	[eventdatetime] ASC,
	[eventtype] ASC,
	[reasoncode] ASC,
	[profileid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [agenttabl].[AgentConnectionDetailUCCX](
	[sessionid] [numeric](18, 0) NOT NULL,
	[sessionseqnum] [smallint] NOT NULL,
	[nodeid] [smallint] NOT NULL,
	[profileid] [int] NOT NULL,
	[resourceid] [int] NOT NULL,
	[startdatetime] [datetime2](7) NOT NULL,
	[enddatetime] [datetime2](7) NOT NULL,
	[qindex] [smallint] NOT NULL,
	[gmtoffset] [smallint] NOT NULL,
	[ringtime] [smallint] NULL,
	[talktime] [int] NULL,
	[holdtime] [int] NULL,
	[worktime] [smallint] NULL,
	[callwrapupdata] [varchar](40) NULL,
	[callresult] [smallint] NULL,
	[dialinglistid] [int] NULL,
	[rna] [bit] NULL,
	[contactid] [varchar](40) NULL,
	[loginsessionid] [varchar](18) NULL,
	[csqrecordid] [int] NULL,
	[consultsessionid] [numeric](18, 0) NULL,
 CONSTRAINT [PK_AgentConnectionDetailUCCX] PRIMARY KEY CLUSTERED 
(
	[sessionid] ASC,
	[sessionseqnum] ASC,
	[nodeid] ASC,
	[profileid] ASC,
	[resourceid] ASC,
	[startdatetime] ASC,
	[qindex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO