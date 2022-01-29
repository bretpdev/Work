CREATE TABLE [compfafsa].[Classes]
(
	[ClassId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Class] VARCHAR(10) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(200) NULL
)
