CREATE TABLE [webapi].[ControllerActions]
(
	[ControllerActionId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ControllerId] INT NOT NULL,
	[ActionName] VARCHAR(500) NOT NULL, 
    [RetiredAt] DATETIME NULL, 
    CONSTRAINT [FK_ControllerActions_Controllers] FOREIGN KEY ([ControllerId]) REFERENCES [webapi].Controllers([ControllerId])
)
