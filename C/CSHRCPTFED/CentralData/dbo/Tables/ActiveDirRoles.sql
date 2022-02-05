CREATE TABLE [dbo].[ActiveDirRoles] (
    [ActiveDirRoleId] INT          IDENTITY (1, 1) NOT NULL,
    [ScriptId]        VARCHAR (24) NOT NULL,
    [RoleId]          VARCHAR (64) NOT NULL,
    [CreatedAt]       DATETIME     DEFAULT (getdate()) NOT NULL,
    [DeletedAt]       DATETIME     NULL,
    [DeletedBy]       VARCHAR (64) NULL
);

