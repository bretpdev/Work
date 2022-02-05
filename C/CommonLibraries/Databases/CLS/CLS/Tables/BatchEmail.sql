CREATE TABLE [dbo].[BatchEmail] (
    [BatchEmailId]      INT            IDENTITY (1, 1) NOT NULL,
    [SASFile]           VARCHAR (100)  NOT NULL,
    [LetterId]          VARCHAR (50)   NOT NULL,
    [SendingAddress]    VARCHAR (100)  NOT NULL,
    [SubjectLine]       VARCHAR (300)  NOT NULL,
    [ARC]               VARCHAR (5)    NULL,
    [CommentText]       VARCHAR (1000) NULL,
    [IncludeAcctNumber] BIT            NOT NULL,
    CONSTRAINT [PK_BatchEmail] PRIMARY KEY CLUSTERED ([BatchEmailId] ASC)
);

