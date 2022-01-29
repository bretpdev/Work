CREATE TABLE [dbo].[InvalidLoginTracking]
(
	[InvalidLoginTrackingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LoginId] INT NOT NULL, 
    [ScriptId] VARCHAR(10) NOT NULL, 
    [Reason] VARCHAR(150) NOT NULL, 
	[InActivated] BIT NOT NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [FK_InvalidLoginTracking_ToLogin] FOREIGN KEY (LoginId) REFERENCES [Login](LoginId)
)
