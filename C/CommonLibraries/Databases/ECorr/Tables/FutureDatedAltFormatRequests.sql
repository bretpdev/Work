CREATE TABLE [dbo].[FutureDatedAltFormatRequests]
(
	[FutureDatedAltFormatRequestId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountNumber] VARCHAR(10) NOT NULL, 
    [LetterId] VARCHAR(10) NOT NULL, 
	[CorrespondenceFormatId] INT NOT NULL,
    [ProcessedOn] DATETIME NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_FutureDatedAltFormatRequest_To CorrespondenceFormat] FOREIGN KEY (CorrespondenceFormatId) REFERENCES CorrespondenceFormats(CorrespondenceFormatId),

)
