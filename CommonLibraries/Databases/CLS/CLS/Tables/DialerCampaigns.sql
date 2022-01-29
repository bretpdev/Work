CREATE TABLE [dbo].[DialerCampaigns]
(
	[DialerCampaignId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DialerCampaign] VARCHAR(50) NOT NULL, 
    [DialerCampaignSprocId] INT NULL
    /*CONSTRAINT [FK_DialerCampaigns_ToDialerCampaignSprocs] FOREIGN KEY ([DialerCampaignSprocId]) REFERENCES [ToTable]([ToTableColumn])*/
)
