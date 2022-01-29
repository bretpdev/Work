CREATE TABLE [dbo].[Arguments]
(
	[ArgumentId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Argument] VARCHAR(20) NOT NULL, 
    [ArgumentDescription] VARCHAR(100) NOT NULL
)
