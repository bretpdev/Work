CREATE TABLE [emailbtcf].[LineDataMergeFieldMapping]
(
	[LineDataMergeFieldMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmailCampaignId] INT NOT NULL REFERENCES [emailbtcf].[EmailCampaigns](EmailCampaignId),
	[MergeFieldId] INT NOT NULL REFERENCES [emailbtcf].[MergeFields]([MergeFieldId]),
	[LineDataIndex] INT NOT NULL
)
