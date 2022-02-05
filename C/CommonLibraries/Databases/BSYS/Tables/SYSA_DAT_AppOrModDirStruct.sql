CREATE TABLE [dbo].[SYSA_DAT_AppOrModDirStruct] (
    [AppOrModName] VARCHAR (255) NOT NULL,
    [Source]       VARCHAR (300) NOT NULL,
    [Destination]  VARCHAR (300) NOT NULL,
    CONSTRAINT [PK_SYSA_DAT_AppOrModDirStruct] PRIMARY KEY CLUSTERED ([AppOrModName] ASC, [Source] ASC, [Destination] ASC),
    CONSTRAINT [FK_SYSA_DAT_AppOrModDirStruct_SYSA_LST_ApplicationsAndModules] FOREIGN KEY ([AppOrModName]) REFERENCES [dbo].[SYSA_LST_ApplicationsAndModules] ([Name]) ON UPDATE CASCADE
);

