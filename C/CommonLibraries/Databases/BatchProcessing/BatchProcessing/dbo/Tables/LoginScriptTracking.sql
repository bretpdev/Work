CREATE TABLE [dbo].[LoginScriptTracking]
(
	[LoginTrackingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LoginId] INT NOT NULL, 
    [ScriptId] INT NOT NULL, 
    CONSTRAINT [FK_LoginScriptTracking_CompassLogin] FOREIGN KEY ([LoginId]) REFERENCES Login([LoginId]), 
    CONSTRAINT [FK_LoginScriptTracking_Scripts] FOREIGN KEY (ScriptId) REFERENCES Scripts([ScriptId])
)
