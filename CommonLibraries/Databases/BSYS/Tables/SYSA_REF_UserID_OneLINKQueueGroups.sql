﻿CREATE TABLE [dbo].[SYSA_REF_UserID_OneLINKQueueGroups] (
    [UserID]      NVARCHAR (7)  NOT NULL,
    [Queue Group] NVARCHAR (3)  NOT NULL,
    [SeqNum]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_SYSA_REF_UserID_OneLINKQueueGroups] PRIMARY KEY CLUSTERED ([UserID] ASC, [Queue Group] ASC),
    CONSTRAINT [FK_SYSA_REF_UserID_OneLINKQueueGroups_SYSA_LST_OneLINKQueueGroups] FOREIGN KEY ([Queue Group]) REFERENCES [dbo].[SYSA_LST_OneLINKQueueGroups] ([Queue Groups]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_UserID_OneLINKQueueGroups_SYSA_LST_UserIDInfo] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SYSA_LST_UserIDInfo] ([UserID]) ON DELETE CASCADE ON UPDATE CASCADE
);

