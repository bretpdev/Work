CREATE TABLE [emailbatch].[EmailProcessing]
(
	[EmailProcessingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmailCampaignId] INT NOT NULL, 
    [AccountNumber] CHAR(10) NOT NULL, 
    [ActualFile] VARCHAR(300) NULL, 
    [EmailData] VARCHAR(MAX) NOT NULL, 
    [EmailSentAt] DATETIME NULL, 
	[ArcNeeded] BIT NOT NULL,
	[ArcAddProcessingId] INT NULL,
	[ProcessingAttempts] INT NOT NULL default 0,
	[AddedBy] VARCHAR(250) NOT NULL, 
    [AddedAt] DATETIME NOT NULL, 
	[DeletedBy] VARCHAR(250) NULL,
    [DeletedAt] DATETIME NULL,  
    CONSTRAINT [FK_EmailProcessing_ToEmailCampaigns] FOREIGN KEY (EmailCampaignId) REFERENCES emailbatch.EmailCampaigns(EmailCampaignId), 
    CONSTRAINT [CK_EmailProcessing_AccountNumber] CHECK (LEN(LTRIM(RTRIM(Accountnumber))) = 10)
)
