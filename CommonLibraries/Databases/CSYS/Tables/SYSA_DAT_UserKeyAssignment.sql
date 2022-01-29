CREATE TABLE [dbo].[SYSA_DAT_UserKeyAssignment] (
    [ID]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [SqlUserId]    INT           NOT NULL,
    [UserKey]      VARCHAR (100) NOT NULL,
    [BusinessUnit] INT           NULL,
    [StartDate]    DATETIME      CONSTRAINT [DF_SYSA_DAT_UserKeyAssignment_StartDate] DEFAULT (getdate()) NOT NULL,
    [EndDate]      DATETIME      NULL,
    [AddedBy]      INT           NOT NULL,
    [RemovedBy]    INT           NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[SYSA_DAT_UserKeyAssignment] TO [db_executor]
    AS [dbo];

