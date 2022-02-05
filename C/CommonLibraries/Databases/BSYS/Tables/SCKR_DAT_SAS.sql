CREATE TABLE [dbo].[SCKR_DAT_SAS] (
    [Job]         NVARCHAR (100) NOT NULL,
    [ID]          NVARCHAR (9)   NULL,
    [Description] NTEXT          NULL,
    [Status]      NVARCHAR (50)  CONSTRAINT [DF_SCKR_DAT_SAS_Status] DEFAULT (N'Development') NULL,
    [Type]        NVARCHAR (50)  NULL,
    [ImpTasks]    NTEXT          NULL,
    [Troubles]    NTEXT          NULL,
    CONSTRAINT [PK_datSAS] PRIMARY KEY CLUSTERED ([Job] ASC) WITH (FILLFACTOR = 90)
);

