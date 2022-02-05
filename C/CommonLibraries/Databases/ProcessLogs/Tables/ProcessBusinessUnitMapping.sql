CREATE TABLE [dbo].[ProcessBusinessUnitMapping]
(
	[ProcessBusinessUnitMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ScriptId] NVARCHAR(10) NOT NULL, 
    [BusinessUnitId] INT NOT NULL, 
    [StartedAt] DATETIME NOT NULL, 
    [EndedAt] DATETIME NULL, 
    CONSTRAINT [FK_ScriptBusinessUnitMapping_BusinessUnits] FOREIGN KEY (BusinessUnitId) REFERENCES [BusinessUnits]([BusinessUnitId]) 
)
