CREATE TABLE [dbo].[BinAndQueueXRef] (
    [Priority] INT          NULL,
    [BinID]    INT          NOT NULL,
    [Queue]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_BinAndQueueXRef] PRIMARY KEY CLUSTERED ([BinID] ASC, [Queue] ASC)
);

