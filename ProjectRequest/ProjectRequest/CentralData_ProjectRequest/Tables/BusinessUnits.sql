CREATE TABLE [projectrequest].[BusinessUnits]
(
	[BusinessUnitId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BusinessUnit] VARCHAR(50) NOT NULL,
	[Active] BIT NOT NULL DEFAULT 1 
)
