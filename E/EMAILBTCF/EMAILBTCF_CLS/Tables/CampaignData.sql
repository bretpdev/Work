CREATE TABLE [emailbtcf].[CampaignData]
(
	[CampaignDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmailCampaignId] INT NOT NULL, 
    [Recipient] VARCHAR(254) NOT NULL, 
    [AccountNumber] CHAR(10) NOT NULL, 
    [FirstName] VARCHAR(100) NOT NULL, 
    [LastName] VARCHAR(100) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(100) NOT NULL DEFAULT SYSTEM_USER, 
	[EmailSentAt] DATETIME NULL,
    [ArcProcessedAt] DATETIME NULL, 
	[ArcAddProcessingId] BIGINT NULL,
    [InactivatedAt] DATETIME NULL, 
    [LineDataId] INT NULL, 
    CONSTRAINT [FK_CampaignData_EmailCampaigns] FOREIGN KEY ([EmailCampaignId]) REFERENCES [emailbtcf].[EmailCampaigns]([EmailCampaignId]),

)
