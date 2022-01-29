CREATE TYPE [dbo].[ScriptLetterData] AS TABLE
(
	Header VARCHAR(50),
	HeaderType VARCHAR(50),
	[Order] INT,
	Active BIT
)
