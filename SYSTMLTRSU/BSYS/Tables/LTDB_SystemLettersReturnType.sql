CREATE TABLE [dbo].[LTDB_SystemLettersReturnType] (
    [ReturnTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [ReturnType]   VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([ReturnTypeId] ASC)
);