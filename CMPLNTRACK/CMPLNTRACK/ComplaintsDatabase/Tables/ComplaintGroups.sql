CREATE TABLE [complaints].[ComplaintGroups]
(
	[ComplaintGroupId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GroupName] NVARCHAR(50) NOT NULL,
	[AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] NVARCHAR(50) NULL
)
