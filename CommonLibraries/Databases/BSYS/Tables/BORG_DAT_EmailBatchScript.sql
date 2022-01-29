CREATE TABLE [dbo].[BORG_DAT_EmailBatchScript] (
    [SASFile]        VARCHAR (100)  NOT NULL,
    [HTMLFile]       VARCHAR (100)  NOT NULL,
    [SendingAddress] VARCHAR (100)  NOT NULL,
    [SubjectLine]    VARCHAR (300)  NOT NULL,
    [ARC]            VARCHAR (5)    NULL,
    [ActionCode]     VARCHAR (5)    NULL,
    [CommentText]    VARCHAR (1000) NULL,
    CONSTRAINT [PK_BORG_DAT_EmailBatchScript] PRIMARY KEY CLUSTERED ([SASFile] ASC)
);

