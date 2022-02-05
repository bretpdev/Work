CREATE TABLE [dbo].[ProcessLogs]
(
	[ProcessLogId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [StartedOn] DATETIME NOT NULL, 
	[EndedOn]  DATETIME NULL,
    [ScriptId] VARCHAR(10) NOT NULL,
	[Region] varchar(11) not null DEFAULT 'none',
    [RunBy] NVARCHAR(50) NULL DEFAULT SYSTEM_USER, 
    CONSTRAINT [CK_ScriptLogs_Region] CHECK (Region COLLATE latin1_general_cs_as IN ('cornerstone' ,'uheaa', 'none', 'pheaa')) --force case sensitivity
)
