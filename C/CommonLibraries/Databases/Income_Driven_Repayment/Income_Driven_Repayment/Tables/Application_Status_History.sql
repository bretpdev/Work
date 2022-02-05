CREATE TABLE [dbo].[Application_Status_History]
(
	[ApplicationStatusHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ApplicationId] INT NOT NULL, 
    [UpdatedBy] VARCHAR(50) NOT NULL, 
    [UpdatedAt] DATETIME NOT NULL, 
    [Active] BIT NOT NULL
)
