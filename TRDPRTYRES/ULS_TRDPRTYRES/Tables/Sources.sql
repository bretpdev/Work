﻿CREATE TABLE [trdprtyres].[Sources]
(
	[SourcesId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Source] VARCHAR(50) NOT NULL, 
    [SourceCode] VARCHAR(2) NOT NULL, 
    [IsOnelink] BIT NOT NULL DEFAULT 0, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
