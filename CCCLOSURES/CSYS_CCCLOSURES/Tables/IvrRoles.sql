CREATE TABLE [ccclosures].[IvrRoles] (
    [IvrRolesId] INT NOT NULL IDENTITY,
    [RoleId] INT NOT NULL, 
    CONSTRAINT [PK_IvrRoles] PRIMARY KEY ([IvrRolesId]),
    CONSTRAINT [FK_IvrRoles_SYSA_LST_Role] FOREIGN KEY ([IvrRolesId]) REFERENCES [dbo].[SYSA_LST_Role] ([RoleID])
);