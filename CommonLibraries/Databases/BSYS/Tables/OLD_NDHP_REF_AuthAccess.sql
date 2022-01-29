CREATE TABLE [dbo].[OLD_NDHP_REF_AuthAccess] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [WindowsUserID] NVARCHAR (50) NOT NULL,
    [BusinessUnit]  NVARCHAR (50) NULL,
    [Access]        VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_NDHP_REF_AuthAccess] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_NDHP_REF_AuthAccess_GENR_LST_BusinessUnits] FOREIGN KEY ([BusinessUnit]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

