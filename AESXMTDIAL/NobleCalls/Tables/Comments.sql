CREATE TABLE [dbo].[Comments] (
    [CommentId] INT           IDENTITY (1, 1) NOT NULL,
    [Comment]   VARCHAR (900) NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC),
    CONSTRAINT [AK_Comments_Comment] UNIQUE NONCLUSTERED ([Comment] ASC)
);

