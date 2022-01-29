CREATE TABLE [dbo].[XDocID_Queue] (
    [DocID]       VARCHAR (50)  NOT NULL,
    [Queue]       VARCHAR (50)  NOT NULL,
    [CommentText] VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_XDocID_Queue] PRIMARY KEY CLUSTERED ([DocID] ASC)
);

