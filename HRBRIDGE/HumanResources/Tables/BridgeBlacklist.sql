CREATE TABLE [hrbridge].[BridgeBlacklist]
(
	[BlacklistId] INT NOT NULL PRIMARY KEY IDENTITY,
	[UID] VARCHAR(20),
	[FirstName] VARCHAR(500),
	[LastName] VARCHAR(500),
	[Destination] VARCHAR(100)
)
