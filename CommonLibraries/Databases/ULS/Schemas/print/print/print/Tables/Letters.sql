CREATE TABLE [print].[Letters] (
    [LetterId]         INT          IDENTITY (1, 1) NOT NULL,
    [Letter]           VARCHAR (10) NOT NULL,
    [PagesPerDocument] INT          NULL,
    PRIMARY KEY CLUSTERED ([LetterId] ASC)
);

