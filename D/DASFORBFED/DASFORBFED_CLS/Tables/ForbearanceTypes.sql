CREATE TABLE [dasforbfed].[ForbearanceTypes]
(
	[ForbearanceTypeId] INT NOT NULL PRIMARY KEY,
	[Description] VARCHAR(50) NOT NULL,
	[ActivityComment] VARCHAR(200) NOT NULL,
	[AddedOn] DATETIME DEFAULT GETDATE() NOT NULL, 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER 
)
