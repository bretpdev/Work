CREATE TABLE [dbo].[AllowedUsers]
(
	[AllowedUserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AllowedUser] VARCHAR(50) NOT NULL Unique, 
    [IsAdmin] BIT NOT NULL DEFAULT 0 
)
