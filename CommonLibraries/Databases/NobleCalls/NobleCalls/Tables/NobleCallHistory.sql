CREATE TABLE [dbo].[NobleCallHistory] (
    [NobleCallHistoryId]        INT           IDENTITY (1, 1) NOT NULL,
    [NobleRowId]                INT           NULL,
    [AccountIdentifier]         VARCHAR (40)  NULL,
    [CallType]                  INT           NULL,
    [ListId]                    VARCHAR (20)  NULL,
    [CallCampaign]              VARCHAR (5)   NULL,
    [ActivityDate]              DATETIME      NULL,
    [PhoneNumber]               VARCHAR (10)  NULL,
    [AgentId]                   VARCHAR (4)   NULL,
    [DispositionCode]           VARCHAR (2)   NULL,
    [AdditionalDispositionCode] VARCHAR (2)   NULL,
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
    [VoxVerifiedAt]             DATETIME      NULL,
    CONSTRAINT [PK__tmp_ms_x__660FB327286302EC_1] PRIMARY KEY CLUSTERED ([NobleCallHistoryId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_NobleCallHistory_ToRegions_1] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Regions] ([RegionId])
);











GO



GO


