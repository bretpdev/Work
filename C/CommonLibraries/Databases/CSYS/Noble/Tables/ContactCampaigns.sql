CREATE TABLE [Noble].[ContactCampaigns] (
    [ContactCampaignId] INT           IDENTITY (1, 1) NOT NULL,
    [CampaignCode]      VARCHAR (6)   NOT NULL,
    [CampaignName]      VARCHAR (100) NULL,
    [GroupId]           INT           NULL,
    [Status]            BIT           NULL,
    [Invalidate]        BIT           NULL,
    [CallType]          BIT           NULL,
    [ModifiedAt]        DATETIME      NULL,
    [ModifiedBy]        VARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([ContactCampaignId] ASC),
    FOREIGN KEY ([GroupId]) REFERENCES [Noble].[Groups] ([GroupId])
);

