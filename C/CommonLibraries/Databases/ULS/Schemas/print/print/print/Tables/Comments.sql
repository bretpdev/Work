CREATE TABLE [print].[Comments] (
    [CommentId] INT            IDENTITY (1, 1) NOT NULL,
    [Comment]   VARCHAR (1200) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC) WITH (FILLFACTOR = 95)
);

