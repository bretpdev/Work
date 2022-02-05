CREATE TABLE [dbo].[PovertyGuidelines]
(
	[PovertyGuidelineId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FamilySize] INT NOT NULL, 
    [Income] DECIMAL(18, 2) NOT NULL
)
