CREATE TABLE [dbo].[TRAN_LST_AppAndModVersions] (
    [AppOrModName] VARCHAR (255) NOT NULL,
    [VersionDate]  SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_TRAN_LST_AppAndModVersions] PRIMARY KEY CLUSTERED ([AppOrModName] ASC, [VersionDate] ASC),
    CONSTRAINT [FK_TRAN_LST_AppAndModVersions_SYSA_LST_ApplicationsAndModules] FOREIGN KEY ([AppOrModName]) REFERENCES [dbo].[SYSA_LST_ApplicationsAndModules] ([Name]) ON UPDATE CASCADE
);

