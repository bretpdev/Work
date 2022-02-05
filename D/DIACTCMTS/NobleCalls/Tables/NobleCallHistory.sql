CREATE TABLE [dbo].[NobleCallHistory] (
    [NobleCallHistoryId]        INT           IDENTITY (1, 1) NOT NULL,
    [NobleRowId]                BIGINT           NULL,
    [AccountIdentifier]         VARCHAR (40)  NULL,
    [CallType]                  INT           NULL,
    [ListId]                    VARCHAR (20)  NULL,
    [CallCampaign]              VARCHAR (150)   NULL,
    [ActivityDate]              DATETIME      NULL,
    [PhoneNumber]               VARCHAR (20)  NULL,
    [AgentId]                   VARCHAR (100)   NULL,
    [DispositionCode]           VARCHAR (150)   NULL,
    [AdditionalDispositionCode] VARCHAR (150)   NULL,
    [RegionId]                  INT           NULL,
    [CreatedAt]                 DATETIME      CONSTRAINT [DF__tmp_ms_xx__Creat__2A4B4B5E_1] DEFAULT (getdate()) NOT NULL,
    [ArcAddProcessingId]        INT           NULL,
    [IsReconciled]              BIT           CONSTRAINT [DF__tmp_ms_xx__IsRec__2B3F6F97_1] DEFAULT ((0)) NULL,
    [ReconciledAt]              DATETIME      NULL,
    [Deleted]                   BIT           CONSTRAINT [DF__tmp_ms_xx__Delet__2C3393D0_1] DEFAULT ((0)) NULL,
    [DeletedBy]                 VARCHAR (50)  NULL,
    [DeletedAt]                 DATETIME      NULL,
    [VoxFileId]                 VARCHAR (50)  NULL,
    [IsInbound]                 BIT           CONSTRAINT [DF_NobleCallHistory_IsInbound_1] DEFAULT ((0)) NOT NULL,
    [VoxFileLocation]           VARCHAR (100) NULL,
    [CallLength]                INT           CONSTRAINT [DF_NobleCallHistory_CallLength_1] DEFAULT ((0)) NULL,
	[TimeACW]                   INT           NULL,
    [TimeHold]                  INT           NULL,
    [AgentHold]                 INT           NULL,
    [Filler1]                   VARCHAR (20)  NULL,
    [Filler3]                   INT           NULL,
    [Filler4]                   INT           NULL,
    [d_record_id]               INT           NULL,
    [CoborrowerAccountNumber] VARCHAR(10) NULL, 
    [DialerField1] VARCHAR(256) NULL,
    [DialerField2] VARCHAR(256) NULL,
    [DialerField3] VARCHAR(256) NULL,
    [DialerField4] VARCHAR(256) NULL,
    [DialerField5] VARCHAR(256) NULL,
    [DialerField6] VARCHAR(256) NULL,
    [DialerField7] VARCHAR(256) NULL,
    [DialerField8] VARCHAR(256) NULL,
    [DialerField9] VARCHAR(256) NULL,
    [DialerField10] VARCHAR(256) NULL,
    [DialerField11] VARCHAR(256) NULL,
    [DialerField12] VARCHAR(256) NULL,
    [DialerField13] VARCHAR(256) NULL,
    [DialerField14] VARCHAR(256) NULL,
    [DialerField15] VARCHAR(256) NULL,
    [DialerField16] VARCHAR(256) NULL,
    [DialerField17] VARCHAR(256) NULL,
    [DialerField18] VARCHAR(256) NULL,
    [DialerField19] VARCHAR(256) NULL,
    [DialerField20] VARCHAR(256) NULL,
    [DialerField21] VARCHAR(MAX) NULL,
    [DialerField22] VARCHAR(MAX) NULL,
    [DialerField23] VARCHAR(MAX) NULL,
    [DialerField24] VARCHAR(MAX) NULL,
    [DialerField25] VARCHAR(MAX) NULL,
    CONSTRAINT [PK__tmp_ms_x__660FB327286302EC_1] PRIMARY KEY CLUSTERED ([NobleCallHistoryId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_NobleCallHistory_ToRegions_1] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Regions] ([RegionId])
);


GO
CREATE NONCLUSTERED INDEX [IX_NobleCallHistory_RegionId_ArcAddProcessingId_Deleted_IsInbound]
    ON [dbo].[NobleCallHistory]([RegionId] ASC, [ArcAddProcessingId] ASC, [Deleted] ASC, [IsInbound] ASC)
    INCLUDE([NobleCallHistoryId], [AccountIdentifier], [CallType], [CallCampaign], [ActivityDate], [PhoneNumber], [AgentId], [DispositionCode], [AdditionalDispositionCode]) WITH (FILLFACTOR = 85);


GO
CREATE NONCLUSTERED INDEX [IX_NobleCallHistory_RegionId_IsReconciled_Deleted_IsInbound_AccountIdentifier_CallType_CreatedAt]
    ON [dbo].[NobleCallHistory]([RegionId] ASC, [IsReconciled] ASC, [Deleted] ASC, [IsInbound] ASC, [AccountIdentifier] ASC, [CallType] ASC, [CreatedAt] ASC)
    INCLUDE([CallCampaign]) WITH (FILLFACTOR = 85);


GO
CREATE NONCLUSTERED INDEX [IX_NobleCallHistoryId]
    ON [dbo].[NobleCallHistory]([NobleCallHistoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RecconciledAt_DeletedAt_CallCampaign_CreatedAt_ArcAddProcessingId]
    ON [dbo].[NobleCallHistory]([ReconciledAt] ASC, [DeletedAt] ASC, [CallCampaign] ASC, [CreatedAt] ASC, [ArcAddProcessingId] ASC)
    INCLUDE([AccountIdentifier]);


GO
CREATE NONCLUSTERED INDEX [IX_RegionId_IsReconciled_Deleted_IsInbound_CallCampaign_CreatedAt]
    ON [dbo].[NobleCallHistory]([RegionId] ASC, [IsReconciled] ASC, [Deleted] ASC, [IsInbound] ASC, [CallCampaign] ASC, [CreatedAt] ASC)
    INCLUDE([NobleCallHistoryId], [AccountIdentifier]);


GO
CREATE NONCLUSTERED INDEX [IX_RegionId_DeletedAt_IsInbound_ActivityDate]
    ON [dbo].[NobleCallHistory]([RegionId] ASC, [DeletedAt] ASC, [IsInbound] ASC, [ActivityDate] ASC)
    INCLUDE([AccountIdentifier]);

