﻿CREATE TABLE [dbo].[DocIds]
(
	[DocIdId] TINYINT NOT NULL PRIMARY KEY IDENTITY, 
    [DocIdValue] CHAR(5) NOT NULL, 
    [DocTypeId] TINYINT NOT NULL, 
    [AddedBy] NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    [RemovedBy] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_DocIds_DocTypes] FOREIGN KEY ([DocTypeId]) REFERENCES [DocTypes]([DocTypeId])
)
