CREATE TABLE [dbo].[SYSA_REF_Groups] (
    [GroupName]        NVARCHAR (10) NOT NULL,
    [ScreenID]         NVARCHAR (4)  NOT NULL,
    [AccessLevel]      NVARCHAR (1)  NOT NULL,
    [DefaultTierLever] NVARCHAR (10) NULL,
    CONSTRAINT [PK_SYSA_REF_Groups] PRIMARY KEY CLUSTERED ([GroupName] ASC, [ScreenID] ASC, [AccessLevel] ASC),
    CONSTRAINT [FK_SYSA_REF_Groups_SYSA_LST_Groups] FOREIGN KEY ([GroupName]) REFERENCES [dbo].[SYSA_LST_Groups] ([GroupName]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_REF_Groups_SYSA_REF_Screen_Access] FOREIGN KEY ([ScreenID], [AccessLevel]) REFERENCES [dbo].[SYSA_REF_Screen_Access] ([ScreenID], [AccessLevel]) ON DELETE CASCADE ON UPDATE CASCADE
);

