CREATE TABLE [hrbridge].[Keys]
(
	[KeyId] INT NOT NULL PRIMARY KEY IDENTITY,
	[KeyName] VARCHAR(100),
	[Key] VARCHAR(200),
	[Secret] VARCHAR(200)
)
