CREATE TABLE [dbo].[GENR_LST_ComputerRoles] (
    [TypeKey]      VARCHAR (50) NOT NULL,
    [ComputerName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_LST_ComputerRoles] PRIMARY KEY CLUSTERED ([TypeKey] ASC, [ComputerName] ASC)
);

