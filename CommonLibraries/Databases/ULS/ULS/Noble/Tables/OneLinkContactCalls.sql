CREATE TABLE [Noble].[OneLinkContactCalls] (
    [OneLinkContactCallsId] INT           IDENTITY (1, 1) NOT NULL,
    [AccountNumber]         VARCHAR (10)  NULL,
    [SSN]                   VARCHAR (9)   NULL,
    [Telephone]             VARCHAR (10)  NULL,
    [Category]              INT           NULL,
    [ListId]                VARCHAR (10)  NULL,
    [ContactCampaignId]     INT           NULL,
    [AgentCode]             VARCHAR (7)   NULL,
    [AgentCode2]            VARCHAR (50)  NULL,
    [AgentName]             VARCHAR (100) NULL,
    [Status]                VARCHAR (3)   NULL,
    [AddiStatus]            VARCHAR (3)   NULL,
    [CallDate]              DATE          NULL,
    [CallTime]              VARCHAR (20)  NULL,
    [TimeConnected]         VARCHAR (8)   NULL,
    [TimeACW]               VARCHAR (8)   NULL,
    [TimeHold]              VARCHAR (8)   NULL,
    [Filler1]               VARCHAR (20)  NULL,
    [Filler2]               VARCHAR (10)  NULL,
    CONSTRAINT [PK__OneLinkC__5571777E24134F1B] PRIMARY KEY CLUSTERED ([OneLinkContactCallsId] ASC)
);



