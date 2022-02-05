CREATE TABLE [monitor].[ExemptForbearanceTypes]
(
	[ExemptForbearanceTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ForbearanceTypeCode] CHAR(2) NOT NULL, 
    [ForbearanceTypeDescription] VARCHAR(50) NOT NULL
)
