﻿CREATE TABLE [complaints].[Flags]
(
	[FlagId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FlagName] NVARCHAR(50) NOT NULL,
	[EnablesControlMailFields] BIT DEFAULT 0 NOT NULL,
	[AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] NVARCHAR(50) NULL
)
