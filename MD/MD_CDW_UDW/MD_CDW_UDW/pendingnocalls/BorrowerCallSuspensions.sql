CREATE TABLE [pendingnocalls].[BorrowerCallSuspensions]
(
	[BorrowerCallSuspensionId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[AccountNumber] char(10) NOT NULL,
    [SubmittedBy] NVARCHAR(100) NOT NULL DEFAULT SYSTEM_USER, 
    [SubmittedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NOT NULL,
)