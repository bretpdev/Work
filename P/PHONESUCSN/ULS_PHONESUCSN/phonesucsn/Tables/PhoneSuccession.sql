CREATE TABLE [phonesucsn].[PhoneSuccession]
(
	[PhoneSuccessionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Ssn] VARCHAR(9) NOT NULL,
	[HomePhone] VARCHAR(19) NULL, --Set to 19 to account for foreign numbers
	[HomeExt] VARCHAR(5) NULL,
	[HomeSrc] VARCHAR(2) NULL,
	[HomeInd] VARCHAR(1) NULL,
	[HomeConsent] VARCHAR(1) NULL,
	[HomeIsValid] BIT NULL,
	[HomeIsForeign] BIT NULL,
	[HomeVerifiedDate] DATE NULL,
    [AltPhone] VARCHAR(19) NULL, --Set to 19 to account for foreign numbers
	[AltExt] VARCHAR(5) NULL,
	[AltSrc] VARCHAR(2) NULL,
	[AltInd] VARCHAR(1) NULL,
	[AltConsent] VARCHAR(1) NULL,
	[AltIsValid] BIT NULL,
	[AltIsForeign] BIT NULL,
	[AltVerifiedDate] DATE NULL,
    [WorkPhone] VARCHAR(19) NULL, --Set to 19 to account for foreign numbers
	[WorkExt] VARCHAR(5) NULL,
	[WorkSrc] VARCHAR(2) NULL,
	[WorkInd] VARCHAR(1) NULL,
	[WorkConsent] VARCHAR(1) NULL,
	[WorkIsValid] BIT NULL,
	[WorkIsForeign] BIT NULL,
	[WorkVerifiedDate] DATE NULL,
    [ProcessedAt] DATETIME NULL,
	[InvalidatedAt] DATETIME NULL,
	[HadError] BIT NULL,
	[MorningRun] BIT NOT NULL DEFAULT 0,
	[IsEndorser] BIT NOT NULL DEFAULT 0,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)
