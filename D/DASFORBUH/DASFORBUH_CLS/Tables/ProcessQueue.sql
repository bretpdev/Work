﻿CREATE TABLE [dasforbuh].[ProcessQueue]
(
	[ProcessQueueId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountNumber] CHAR(10) NOT NULL, 
    [BeginDate] DATE NOT NULL, 
    [EndDate] DATE NOT NULL,
	[ForbearanceTypeId] INT NULL,
	[ForbearanceAddedOn] DATETIME,
	[ArcAddProcessingId] BIGINT,
	[DisasterId] INT NULL,
	[LD_DLQ_OCC] DATE NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT getdate(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT system_user, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL, 
    [DeletedNotes] VARCHAR(200) NULL, 
    CONSTRAINT [FK_ProcessQueue_ForbearanceTypes] FOREIGN KEY ([ForbearanceTypeId]) REFERENCES [dasforbuh].[ForbearanceTypes]([ForbearanceTypeId]), 
    CONSTRAINT [FK_ProcessQueue_Disasters] FOREIGN KEY ([DisasterId]) REFERENCES [dasforbuh].[Disasters]([DisasterId])
)