CREATE TABLE [print].[Letters] (
    [LetterId]         INT           IDENTITY (1, 1) NOT NULL,
    [Letter]           VARCHAR (10)  NOT NULL,
    [PagesPerDocument] INT           NULL,
    [Instructions]     VARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([LetterId] ASC) WITH (FILLFACTOR = 95)
);

