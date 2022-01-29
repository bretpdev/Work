CREATE TABLE [dbo].[AuthList] (
    [AuthLevel] SMALLINT IDENTITY (1, 1) NOT NULL,
    [LevelDesc] TEXT     NULL,
    CONSTRAINT [PK_AuthList] PRIMARY KEY CLUSTERED ([AuthLevel] ASC)
);

