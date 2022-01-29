CREATE TABLE [emailbtcf].[EmailCampaigns]
(
	[EmailCampaignId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SasFile] VARCHAR(1000) NOT NULL, 
    [LetterId] VARCHAR(50) NOT NULL, 
    [SendingAddress] VARCHAR(100) NOT NULL, 
    [SubjectLine] VARCHAR(300) NOT NULL, 
    [Arc] VARCHAR(5) NULL, 
    [CommentText] VARCHAR(1000) NULL, 
	[AddedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[AddedBy] VARCHAR(100) NOT NULL DEFAULT SYSTEM_USER,
    [InactivatedAt] DATETIME NULL
)
