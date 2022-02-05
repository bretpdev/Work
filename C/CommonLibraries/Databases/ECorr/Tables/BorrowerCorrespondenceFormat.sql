CREATE TABLE [dbo].[BorrowerCorrespondenceFormats]
(
	BorrowerCorrespondenceFormatId int NOT NULL PRIMARY KEY IDENTITY,
	[AccountNumber] CHAR(10) NOT NULL,
	[CorrespondenceFormatId] INT NOT NULL DEFAULT 1, 
)
