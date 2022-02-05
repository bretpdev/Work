CREATE TABLE [ccclosures].[RegionAccess]
(
	[RegionAccessId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RoleId] INT NOT NULL, 
    [RegionsId] INT NOT NULL, 
    CONSTRAINT [FK_RegionAccess_IvrRoles] FOREIGN KEY ([RoleId]) REFERENCES [ccclosures].[IvrRoles]([IvrRolesId]), 
    CONSTRAINT [FK_RegionAccess_Regions] FOREIGN KEY ([RegionsId]) REFERENCES [ccclosures].[Regions]([RegionsId])
)