CREATE TABLE [dbo].[GENR_DAT_Credentials] (
    [CredentialKey] VARCHAR (50)    NOT NULL,
    [Credential]    VARBINARY (128) NOT NULL,
    CONSTRAINT [PK_GENR_DAT_Credentials] PRIMARY KEY CLUSTERED ([CredentialKey] ASC)
);

