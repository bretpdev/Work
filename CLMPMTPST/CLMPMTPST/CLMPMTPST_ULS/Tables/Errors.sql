CREATE TABLE [clmpmtpst].[Errors]
(
	[ErrorId] INT NOT NULL PRIMARY KEY IDENTITY,
	[ErrorDescriptionId] INT NOT NULL,
	[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(100) NULL,
	CONSTRAINT [FK_Errors_ErrorDescriptions] FOREIGN KEY (ErrorDescriptionId) REFERENCES clmpmtpst.ErrorDescriptions(DescriptionId),
)
