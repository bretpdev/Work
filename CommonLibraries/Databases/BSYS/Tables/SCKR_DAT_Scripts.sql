CREATE TABLE [dbo].[SCKR_DAT_Scripts] (
    [Script]      NVARCHAR (100) NOT NULL,
    [ID]          NVARCHAR (10)  NULL,
    [Description] NTEXT          NULL,
    [Status]      NVARCHAR (50)  CONSTRAINT [DF_SCKR_DAT_Scripts_Status] DEFAULT (N'Development') NULL,
    [Session]     NVARCHAR (50)  NULL,
    [Module]      NVARCHAR (50)  NULL,
    [Subroutine]  NVARCHAR (50)  NULL,
    [ScriptType]  NVARCHAR (50)  NULL,
    [ImpTasks]    NTEXT          NULL,
    [Troubles]    NTEXT          NULL,
    CONSTRAINT [PK_datScripts] PRIMARY KEY CLUSTERED ([Script] ASC) WITH (FILLFACTOR = 90)
);

