CREATE TABLE [dbo].[DEMS_DAT_SystemCodes] (
    [Source]            VARCHAR (50) NOT NULL,
    [LocateType]        VARCHAR (3)  NOT NULL,
    [OneLinkSourceCode] VARCHAR (1)  NOT NULL,
    [CompassSourceCode] VARCHAR (2)  NOT NULL,
    [ActivityType]      VARCHAR (2)  NOT NULL,
    [ContactType]       VARCHAR (2)  NOT NULL,
    CONSTRAINT [PK_DEMS_DAT_CommentCodes_1] PRIMARY KEY CLUSTERED ([Source] ASC)
);

