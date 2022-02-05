CREATE TABLE [dbo].[SYSA_LST_ApplicationsAndModules] (
    [Name]        VARCHAR (255) NOT NULL,
    [Type]        NVARCHAR (50) NULL,
    [Description] NTEXT         NULL,
    [Status]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_SYSA_LST_ApplicationsAndModules] PRIMARY KEY CLUSTERED ([Name] ASC),
    CONSTRAINT [FK_SYSA_LST_ApplicationsAndModules_SYSA_LST_AppAndModuleStatus] FOREIGN KEY ([Status]) REFERENCES [dbo].[SYSA_LST_AppAndModuleStatus] ([Status]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_LST_ApplicationsAndModules_SYSA_LST_AppAndModuleTypes] FOREIGN KEY ([Type]) REFERENCES [dbo].[SYSA_LST_AppAndModuleTypes] ([Type]) ON UPDATE CASCADE
);

