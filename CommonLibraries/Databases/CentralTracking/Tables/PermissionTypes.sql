CREATE TABLE [dbo].[PermissionTypes] (
    [PermissionTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [PermissionTypeDescription]   VARCHAR (50) NOT NULL,
    [CreatedAt]        DATETIME     CONSTRAINT [DF_PermissionTypes_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]        INT          NOT NULL,
    [InactivatedAt]    DATETIME     NULL,
    [InactivatedBy]    INT          NULL,
    [Active]           BIT          CONSTRAINT [DF_PermissionTypes_Active] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_PermissionTypes] PRIMARY KEY CLUSTERED ([PermissionTypeId] ASC),
    CONSTRAINT [FK_PermissionTypes_Entities] FOREIGN KEY ([InactivatedBy]) REFERENCES [dbo].[Entities] ([EntityId]),
    CONSTRAINT [FK_PermissionTypes_Entities_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Entities] ([EntityId])
);





