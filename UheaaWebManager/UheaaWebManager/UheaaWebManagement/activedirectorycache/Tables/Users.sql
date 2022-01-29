CREATE TABLE [activedirectorycache].[Users]
(
	[UserId] INT PRIMARY KEY IDENTITY,
	[AssociatedWindowsUsername] VARCHAR(100),
	[UpdatedAt] DATETIME DEFAULT GETDATE(),
	[DeletedAt] DATETIME NULL,
	CONSTRAINT [CK_Users_AssociatedWindowsUsername_NoDomain] CHECK (AssociatedWindowsUsername NOT LIKE '%\%')
)
