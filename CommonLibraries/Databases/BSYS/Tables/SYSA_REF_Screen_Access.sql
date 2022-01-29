CREATE TABLE [dbo].[SYSA_REF_Screen_Access] (
    [ScreenID]    NVARCHAR (4) NOT NULL,
    [AccessLevel] NVARCHAR (1) NOT NULL,
    CONSTRAINT [PK_SYSA_REF_Screen_Access] PRIMARY KEY CLUSTERED ([ScreenID] ASC, [AccessLevel] ASC),
    CONSTRAINT [FK_SYSA_REF_Screen_Access_SYSA_LST_AccessLevel] FOREIGN KEY ([AccessLevel]) REFERENCES [dbo].[SYSA_LST_AccessLevel] ([AccessLevel]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_Screen_Access_SYSA_LST_Screens] FOREIGN KEY ([ScreenID]) REFERENCES [dbo].[SYSA_LST_Screens] ([ScreenID]) ON DELETE CASCADE ON UPDATE CASCADE
);

