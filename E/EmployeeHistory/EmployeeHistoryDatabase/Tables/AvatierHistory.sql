CREATE TABLE [dbo].[AvatierHistory]
(
	[AvatierHistoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmployeeId] VARCHAR(50) NULL, 
	[UserGuid] VARCHAR(50),
    [Role] VARCHAR(50) NULL, 
    [Title] VARCHAR(50) NULL, 
    [Department] VARCHAR(50) NULL, 
    [ManagerEmployeeId] VARCHAR(50) NULL, 
    [FirstName] VARCHAR(50) NULL, 
    [MiddleName] VARCHAR(50) NULL, 
    [LastName] VARCHAR(50) NULL, 
    [HireDate] DATETIME NULL, 
    [TerminationDate] DATETIME NULL, 
    [UpdateTypeId] INT NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [FK_AvatierHistory_UpdateType] FOREIGN KEY ([UpdateTypeId]) REFERENCES [UpdateTypes]([UpdateTypeId])
)
