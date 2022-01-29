CREATE TABLE [fp].[FileHeaders] (
    [FileHeaderId] INT           IDENTITY (1, 1) NOT NULL,
    [FileHeader]   VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([FileHeaderId] ASC)
);

