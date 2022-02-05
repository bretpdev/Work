CREATE TABLE [dbo].[LenderUpdates]
(
	[LenderUpdateId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MOD] CHAR NOT NULL, 
    [LenderId] VARCHAR(6) NOT NULL, 
    [FullName] VARCHAR(40) NOT NULL, 
    [ShortName] VARCHAR(20) NOT NULL, 
    [Address1] VARCHAR(30) NOT NULL, 
    [Address2] VARCHAR(30) NULL, 
    [City] VARCHAR(20) NOT NULL, 
    [State] CHAR(2) NOT NULL, 
    [Zip] VARCHAR(9) NOT NULL, 
    [Valid] BIT NOT NULL, 
    [DateVerified] DATETIME NOT NULL DEFAULT GetDate(), 
    [Type] CHAR(2) NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GetDate(), 
    [AddedBy] VARCHAR(100) NOT NULL, 
    [ProcessedAt] DATETIME NULL, 
    [ProcessedBy] VARCHAR(100) NULL
)
