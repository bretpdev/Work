CREATE TABLE [dbo].[SYSA_LST_Role] (
    [RoleID]    INT          IDENTITY (1, 1) NOT NULL,
    [RoleName]  VARCHAR (64) NOT NULL,
    [AddedBy]   INT          NOT NULL,
    [StartDate] DATETIME     NOT NULL,
    [RemovedBy] INT          NULL,
    [EndDate]   DATETIME     NULL,
    CONSTRAINT [PK_SYSA_DAT_Role] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

