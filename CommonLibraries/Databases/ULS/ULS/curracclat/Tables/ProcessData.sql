CREATE TABLE [curracclat].[ProcessData] (
    [ProcessDataId] INT           IDENTITY (1, 1) NOT NULL,
    [Ssn]           VARCHAR (9)   NOT NULL,
    [ProcessedAt]   DATETIME      NULL,
    [AddedAt]       DATETIME      CONSTRAINT [DF_ProcessData_AddedAt] DEFAULT (getdate()) NOT NULL,
    [AddedBy]       VARCHAR (100) CONSTRAINT [DF_ProcessData_AddedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]     DATETIME      NULL,
    [DeletedBy]     VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([ProcessDataId] ASC) WITH (FILLFACTOR = 95)
);

