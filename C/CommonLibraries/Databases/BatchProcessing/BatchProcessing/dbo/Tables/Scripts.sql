CREATE TABLE [dbo].[Scripts]
(
	[ScriptId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SackerScriptId] VARCHAR(10) NOT NULL, 
    [MaxLogins] INT NOT NULL DEFAULT 1, 
    CONSTRAINT [AK_Scripts_SackerScriptId] UNIQUE (SackerScriptId),

)
