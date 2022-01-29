CREATE TABLE [dbo].[LTDB_SystemLettersStoredProcedures] (
    [SystemLettersStoredProcedureId] INT           IDENTITY (1, 1) NOT NULL,
    [LetterId]                       INT           NOT NULL,
    [StoredProcedureName]            VARCHAR (100) NOT NULL,
    [ReturnTypeId]                   INT           NOT NULL,
    [Active]                         BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([SystemLettersStoredProcedureId] ASC),
    CONSTRAINT [FK_ReturnType] FOREIGN KEY ([ReturnTypeId]) REFERENCES [dbo].[LTDB_SystemLettersReturnType] ([ReturnTypeId])
);
