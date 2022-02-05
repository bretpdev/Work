CREATE TABLE [activedirectorycache].[Groups]
(
	[GroupId] INT NOT NULL PRIMARY KEY IDENTITY,
	[GroupName] VARCHAR(100),
	[RoleId] INT NOT NULL,
	[AddedAt] DATETIME DEFAULT GETDATE(),
	[DeletedAt] DATETIME NULL,
    CONSTRAINT [FK_Groups_Roles] FOREIGN KEY ([RoleId]) REFERENCES webapi.Roles([RoleId])
)
