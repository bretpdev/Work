﻿CREATE TABLE [dbo].[SYSA_REF_UserID_COMPASSSpecInfo] (
    [UserID]     NVARCHAR (7)  NOT NULL,
    [Supervisor] BIT           CONSTRAINT [DF_SYSA_REF_UserID_COMPASSSpecInfo_Supervisor] DEFAULT (0) NOT NULL,
    [Department] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SYSA_REF_UserID_COMPASSSpecInfo] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_SYSA_REF_UserID_COMPASSSpecInfo_SYSA_LST_Departments] FOREIGN KEY ([Department]) REFERENCES [dbo].[SYSA_LST_Departments] ([Department]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_UserID_COMPASSSpecInfo_SYSA_LST_UserIDInfo] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SYSA_LST_UserIDInfo] ([UserID]) ON DELETE CASCADE ON UPDATE CASCADE
);

