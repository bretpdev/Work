CREATE TABLE [dbo].[CallCampaigns] (
    [CallCampaignId]    INT         IDENTITY (1, 1) NOT NULL,
    [CallCampaign]      VARCHAR (5) NOT NULL,
    [RegionId]          INT         NOT NULL,
    [IsSpecialCampaign] BIT         CONSTRAINT [DF_CallCampaigns_IsSpecialCampaign] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__CallCamp__0B67F7EE888FB953] PRIMARY KEY CLUSTERED ([CallCampaignId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_CallCampaigns_ToRegions] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Regions] ([RegionId]),
    CONSTRAINT [AK_CallCampaigns_CallCampaign] UNIQUE NONCLUSTERED ([CallCampaign] ASC) WITH (FILLFACTOR = 95)
);



GO