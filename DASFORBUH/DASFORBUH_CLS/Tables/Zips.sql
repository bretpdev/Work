CREATE TABLE [dasforbuh].[Zips]
(
	[ZipId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ZipCode] VARCHAR(5) NOT NULL,
	[DisasterId] INT NOT NULL, 
    CONSTRAINT [FK_Zips_Disasters] FOREIGN KEY ([DisasterId]) REFERENCES [dasforbuh].[Disasters]([DisasterId])
)
