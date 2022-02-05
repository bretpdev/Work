CREATE TABLE [dbo].[CorrLog] (
    [DocID]             VARCHAR (50)  NOT NULL,
    [DocType]           VARCHAR (500) NOT NULL,
    [ARC]               VARCHAR (50)  NOT NULL,
    [CodeNumber]        VARCHAR (50)  NOT NULL,
    [SeqNum]            VARCHAR (50)  NOT NULL,
    [NotOnTS24ChangeTo] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_CorrLog] PRIMARY KEY CLUSTERED ([DocID] ASC, [DocType] ASC)
);

