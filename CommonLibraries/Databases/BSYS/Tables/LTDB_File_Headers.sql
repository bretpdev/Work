﻿CREATE TABLE [dbo].[LTDB_File_Headers]
(
	[HeaderId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Header] VARCHAR(50) NOT NULL UNIQUE,
	[CreatedBy] VARCHAR(50) NOT NULL, 
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [UpdatedBy] VARCHAR(50) NULL, 
    [UpdatedAt] DATETIME NULL, 
    [Active] BIT NULL DEFAULT 1
)
