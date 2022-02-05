CREATE TABLE [dbo].[QBLR_LST_QueueBuilderLists] (
    [System]    VARCHAR (7)  NOT NULL,
    [FileName]  VARCHAR (50) NOT NULL,
    [Empty]     CHAR (1)     NOT NULL,
    [NoFile]    CHAR (1)     NOT NULL,
    [MultiFile] CHAR (1)     NOT NULL,
    CONSTRAINT [PK_QBLR_LST_QueueBuilderLists] PRIMARY KEY CLUSTERED ([System] ASC, [FileName] ASC)
);

