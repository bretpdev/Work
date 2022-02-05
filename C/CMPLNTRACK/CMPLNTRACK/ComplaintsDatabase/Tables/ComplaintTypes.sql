CREATE TABLE [complaints].[ComplaintTypes]
(
	[ComplaintTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeName] VARCHAR(50) NULL,
	[AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] NVARCHAR(50) NULL
)
