CREATE TABLE [dbo].[ArcAdd] (
    [RecordId]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [AccountNumber] CHAR (10)     NOT NULL,
    [RecipientId]   VARCHAR (10)  NOT NULL,
    [ARC]           VARCHAR (5)   NOT NULL,
    [ArcAddDate]    DATETIME      NOT NULL,
    [Comment]       VARCHAR (300) NULL,
    [RequestedDate] DATETIME      NOT NULL,
    [UserId]        VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_GENR_DAT_ArcAdd] PRIMARY KEY CLUSTERED ([RecordId] ASC)
);

