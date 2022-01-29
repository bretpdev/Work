CREATE TABLE [print].[FileHeaders] (
    [FileHeaderId]        INT           IDENTITY (1, 1) NOT NULL,
    [FileHeader]          VARCHAR (MAX) NOT NULL,
    [StateIndex]          INT           NOT NULL,
    [AccountNumberIndex]  INT           NOT NULL,
    [CostCenterCodeIndex] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([FileHeaderId] ASC) WITH (FILLFACTOR = 95)
);

