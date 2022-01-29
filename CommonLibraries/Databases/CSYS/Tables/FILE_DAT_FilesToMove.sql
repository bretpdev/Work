CREATE TABLE [dbo].[FILE_DAT_FilesToMove] (
    [FileID]              INT           IDENTITY (1, 1) NOT NULL,
    [FileNameDescription] VARCHAR (MAX) NOT NULL,
    [FilePathOriginal]    VARCHAR (MAX) NOT NULL,
    [FilePathArchiveTo]   VARCHAR (MAX) NOT NULL,
    [FilePathCopyTo]      VARCHAR (MAX) NOT NULL,
    [LastProcessed]       DATETIME      NULL,
    CONSTRAINT [PK_FILE_DAT_FilesToMove] PRIMARY KEY CLUSTERED ([FileID] ASC)
);

