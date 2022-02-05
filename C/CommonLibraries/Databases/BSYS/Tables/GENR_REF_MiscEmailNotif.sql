CREATE TABLE [dbo].[GENR_REF_MiscEmailNotif] (
    [TypeKey]   NVARCHAR (100) NOT NULL,
    [WinUName]  NVARCHAR (50)  NOT NULL,
    [ExcldTest] BIT            CONSTRAINT [DF_GENR_REF_MiscEmailNotif_ExcldTest] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_GENR_REF_MiscEmailNotif] PRIMARY KEY CLUSTERED ([TypeKey] ASC, [WinUName] ASC),
    CONSTRAINT [FK_GENR_REF_MiscEmailNotif_SYSA_LST_Users] FOREIGN KEY ([WinUName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON DELETE CASCADE ON UPDATE CASCADE
);

