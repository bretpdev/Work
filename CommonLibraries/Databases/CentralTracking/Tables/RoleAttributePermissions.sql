CREATE TABLE [dbo].[RoleAttributePermissions] (
    [RoleAttributePermissionId] INT      IDENTITY (1, 1) NOT NULL,
    [RoleId]                    INT      NOT NULL,
    [AttributeId]               INT      NOT NULL,
    [PermissionTypeId]          INT      NOT NULL,
    [CreatedAt]                 DATETIME CONSTRAINT [DF_RoleAttributePermissions_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                 INT      NOT NULL,
    [InactivatedAt]             DATETIME NULL,
    [InactivatedBy]             INT      NULL,
    [Active]                    BIT      CONSTRAINT [DF_RoleAttributePermissions_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_RoleAttributePermissions] PRIMARY KEY CLUSTERED ([RoleAttributePermissionId] ASC),
    CONSTRAINT [FK_RoleAttributePermissions_Attributes] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId]),
    CONSTRAINT [FK_RoleAttributePermissions_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_RoleAttributePermissions_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_RoleAttributePermissions_PermissionTypes] FOREIGN KEY ([PermissionTypeId]) REFERENCES [dbo].[PermissionTypes] ([PermissionTypeId])
);





