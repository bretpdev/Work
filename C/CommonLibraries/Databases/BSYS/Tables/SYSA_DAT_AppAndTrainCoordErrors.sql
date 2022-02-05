CREATE TABLE [dbo].[SYSA_DAT_AppAndTrainCoordErrors] (
    [RecordNumber]    INT           IDENTITY (1, 1) NOT NULL,
    [WindowsUserName] VARCHAR (50)  NOT NULL,
    [TimeStamp]       DATETIME      NOT NULL,
    [FilePathName]    VARCHAR (MAX) NULL,
    [Action]          VARCHAR (50)  NULL,
    [Message]         VARCHAR (MAX) NULL,
    [StackTrace]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_SYSA_DAT_AppAndTrainCoordErrors] PRIMARY KEY CLUSTERED ([RecordNumber] ASC)
);

