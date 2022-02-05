CREATE TABLE [payhistlpp].[Run]
(
	[RunId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RunTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [UserAccessId] INT NOT NULL, 
    [MachineRun] VARCHAR(50) NOT NULL DEFAULT USER_NAME(),
    [FileDirectory] VARCHAR(100) NULL,
    [Tilp] BIT NOT NULL DEFAULT 0,
    [CompletedAt] DATETIME NULL, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Run_UserAccess] FOREIGN KEY (UserAccessId) REFERENCES payhistlpp.UserAccess(UserAccessId)
)