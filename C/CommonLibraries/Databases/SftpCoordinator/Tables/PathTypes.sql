CREATE TABLE [dbo].[PathTypes]
(
	[PathTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(32) NOT NULL, 
    [RootPath] NVARCHAR(256) NOT NULL default('\')
)
