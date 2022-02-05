CREATE TYPE [dbo].[CallData] AS TABLE (
    [NobleRowId]                INT           NULL,
    [CallType]                  INT           NULL,
    [AccountIdentifier]         VARCHAR (40)  NULL,
    [AreaCode]                  VARCHAR (30)  NULL,
    [PhoneNumber]               VARCHAR (100) NULL,
    [CallCampaign]              VARCHAR (5)   NULL,
    [DispositionCode]           VARCHAR (2)   NULL,
    [AdditionalDispositionCode] VARCHAR (2)   NULL,
    [AgentId]                   VARCHAR (4)   NULL,
    [ActivityDate]              DATETIME      NULL,
    [EffectiveTime]             VARCHAR (100) NULL,
    [ListId]                    VARCHAR (20)  NULL,
    [VoxFileId]                 VARCHAR (50)  NULL,
    [IsInbound]                 BIT           NOT NULL,
    [CallLength]                INT           NOT NULL);








