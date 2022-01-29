CREATE TABLE [dbo].[CentralizedPrintingBusinessUnitErrorKey] (
    [BusinessUnitId] INT           NOT NULL,
    [ErrorType]      VARCHAR (5)   NOT NULL,
    [KeyType]        VARCHAR (5)   NOT NULL,
    [Value]          VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_PRNT_REF_BusinessUnitErrorArc] PRIMARY KEY CLUSTERED ([BusinessUnitId] ASC, [ErrorType] ASC, [KeyType] ASC)
);

