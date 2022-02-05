CREATE TABLE [dbo].[SYSA_LST_Groups] (
    [GroupName]        NVARCHAR (10) NOT NULL,
    [System]           NVARCHAR (30) NOT NULL,
    [GroupDescription] NVARCHAR (50) NULL,
    CONSTRAINT [PK_SYSA_LST_Groups] PRIMARY KEY CLUSTERED ([GroupName] ASC),
    CONSTRAINT [FK_SYSA_LST_Groups_SYSA_LST_Systems] FOREIGN KEY ([System]) REFERENCES [dbo].[SYSA_LST_Systems] ([System]) ON UPDATE CASCADE
);

