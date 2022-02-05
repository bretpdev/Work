CREATE TABLE [dbo].[CorrespondenceFormats]
(
	[CorrespondenceFormatId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CorrespondenceFormat] VARCHAR(150) NOT NULL, 
    [NTISCode] CHAR NULL
)
