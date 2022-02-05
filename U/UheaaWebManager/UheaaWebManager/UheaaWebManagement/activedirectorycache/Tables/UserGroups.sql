CREATE TABLE [activedirectorycache].[UserGroups]
(
	[UserGroupId] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] INT,
	[GroupId] INT,
	[UpdatedAt] DATETIME DEFAULT GETDATE(), 
	[DeletedAt] DATETIME NULL,
    CONSTRAINT [FK_UserGroups_Users] FOREIGN KEY ([UserId]) REFERENCES activedirectorycache.[Users]([UserId]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserGroups_Groups] FOREIGN KEY ([GroupId]) REFERENCES activedirectorycache.[Groups]([GroupId]) ON DELETE CASCADE
)
