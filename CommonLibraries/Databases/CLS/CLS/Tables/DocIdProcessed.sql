CREATE TABLE [dbo].[DocIdProcessed] (
    [DateTimeStamp] DATETIME     NOT NULL,
    [UserName]      VARCHAR (50) NOT NULL,
    [DocId]         VARCHAR (5)  NOT NULL,
    [Source]        CHAR (2)     NOT NULL,
    CONSTRAINT [PK_DocIdProcessed] PRIMARY KEY CLUSTERED ([DateTimeStamp] ASC, [UserName] ASC)
);

