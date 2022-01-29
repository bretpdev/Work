CREATE TABLE [dbo].[SYSA_REF_User_Systems] (
    [WindowsUserName]         NVARCHAR (50) NOT NULL,
    [System]                  NVARCHAR (30) NOT NULL,
    [System Specific User ID] NVARCHAR (50) NULL,
    [Notes]                   NTEXT         NULL,
    [DtAccessRemoved]         DATETIME      NULL,
    CONSTRAINT [PK_SYSA_REF_User_Systems] PRIMARY KEY CLUSTERED ([WindowsUserName] ASC, [System] ASC),
    CONSTRAINT [FK_SYSA_REF_User_Systems_SYSA_LST_Systems] FOREIGN KEY ([System]) REFERENCES [dbo].[SYSA_LST_Systems] ([System]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_User_Systems_SYSA_LST_Users] FOREIGN KEY ([WindowsUserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON DELETE CASCADE ON UPDATE CASCADE
);

