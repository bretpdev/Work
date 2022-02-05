CREATE TABLE [dbo].[LTDB_LST_Statuses] (
    [Status]      NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (100) NULL,
    [Order]       INT            NULL,
    [StdDays]     NVARCHAR (50)  NULL,
    CONSTRAINT [PK_LTDB_LST_Statuses] PRIMARY KEY CLUSTERED ([Status] ASC) WITH (FILLFACTOR = 90)
);

