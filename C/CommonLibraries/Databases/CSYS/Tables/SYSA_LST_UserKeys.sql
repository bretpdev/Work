CREATE TABLE [dbo].[SYSA_LST_UserKeys] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [UserKey]     VARCHAR (100)  NOT NULL,
    [Application] VARCHAR (100)  NOT NULL,
    [Type]        VARCHAR (20)   NULL,
    [Description] VARCHAR (8000) NULL,
    [AddedBy]     INT            NOT NULL,
    [StartDate]   DATETIME       NOT NULL,
    [RemovedBy]   INT            NULL,
    [EndDate]     DATETIME       NULL,
    CONSTRAINT [PK_SYSA_LST_UserKeys_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

