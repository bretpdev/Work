CREATE TABLE [dbo].[CampaignPrefixes]
(
	[CampaignPrefixId] INT NOT NULL PRIMARY KEY IDENTITY,
	[CampaignPrefix] VARCHAR(100) NOT NULL,
	[ScriptId] VARCHAR(100) NOT NULL,
	[RegionId] INT NOT NULL,
	[Active] BIT DEFAULT(1)
)
