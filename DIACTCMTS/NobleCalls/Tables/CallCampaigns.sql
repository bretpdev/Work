CREATE TABLE [dbo].[CallCampaigns] (
    [CallCampaignId] INT         IDENTITY (1, 1) NOT NULL,
    [CallCampaign]   VARCHAR (50) NOT NULL,
    [RegionId]       INT         NOT NULL,
	[IsSpecialCampaign] BIT         NULL,
    PRIMARY KEY CLUSTERED ([CallCampaignId] ASC),
    CONSTRAINT [FK_CallCampaigns_ToRegions] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Regions] ([RegionId]),
    CONSTRAINT [AK_CallCampaigns_CallCampaign] UNIQUE NONCLUSTERED ([CallCampaign] ASC)
);

