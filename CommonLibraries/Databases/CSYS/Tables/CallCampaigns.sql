CREATE TABLE [dbo].[CallCampaigns] (
    [CallCampaignId] INT         IDENTITY (1, 1) NOT NULL,
    [CallCampaign]   VARCHAR (5) NOT NULL,
    [RegionId]       INT         NOT NULL,
    PRIMARY KEY CLUSTERED ([CallCampaignId] ASC),
    FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Region] ([RegionId]),
    FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Region] ([RegionId])
);


