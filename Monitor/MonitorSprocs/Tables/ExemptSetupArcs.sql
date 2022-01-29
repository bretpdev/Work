CREATE TABLE [monitor].[ExemptSetupArcs]
(
	[ExemptSetupArcId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ARC] CHAR(5) NOT NULL, 
    [Queue] CHAR(2) NULL
)
