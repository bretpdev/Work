CREATE TABLE [dbo].[OLD_NDHP_REF_DirectorEmailCopying] (
    [Director] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NDHP_REF_DirectorEmailCopying] PRIMARY KEY CLUSTERED ([Director] ASC),
    CONSTRAINT [FK_NDHP_REF_DirectorEmailCopying_SYSA_LST_Users] FOREIGN KEY ([Director]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

