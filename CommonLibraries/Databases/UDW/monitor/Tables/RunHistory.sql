CREATE TABLE [monitor].[RunHistory] (
    [RunHistoryId] INT          IDENTITY (1, 1) NOT NULL,
    [StartedAt]    DATETIME     DEFAULT (getdate()) NOT NULL,
    [EndedAt]      DATETIME     NULL,
    [RunBy]        VARCHAR (50) DEFAULT (suser_sname()) NULL,
    PRIMARY KEY CLUSTERED ([RunHistoryId] ASC) WITH (FILLFACTOR = 95)
);

