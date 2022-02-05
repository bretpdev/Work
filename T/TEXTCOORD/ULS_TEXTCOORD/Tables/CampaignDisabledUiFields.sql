CREATE TABLE [textcoord].[CampaignDisabledUiFields]
(
	[CampaignDisabledUiFieldId] INT NOT NULL PRIMARY KEY IDENTITY,
	[CampaignId] INT NOT NULL,
	[UiFieldId] INT NOT NULL, 
    CONSTRAINT [FK_CampaignDisabledUiFields_Campaigns] FOREIGN KEY ([CampaignId]) REFERENCES [textcoord].[Campaigns]([CampaignId]),
	CONSTRAINT [FK_CampaignDisabledUiFields_UiFields] FOREIGN KEY ([UiFieldId]) REFERENCES [textcoord].[UiFields]([UiFieldId])
)