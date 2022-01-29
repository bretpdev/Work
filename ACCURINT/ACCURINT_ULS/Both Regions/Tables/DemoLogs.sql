CREATE TABLE [accurint].[DemoLogs]
(
	[DemoLogId] INT IDENTITY(1,1) NOT NULL,
	[RegionId] INT NOT NULL,
	[DemosId] INT,
	[AccountNumber] CHAR(10) NULL,
	[SentAddress1] VARCHAR(45) NULL,
	[SentAddress2] VARCHAR(30) NULL,
	[SentCity] VARCHAR(20) NULL,
	[SentState] VARCHAR(2) NULL,
	[SentZipCode] VARCHAR(17) NULL,
	[SentPhoneNumber] VARCHAR(27) NULL,
	[SentValidity] BIT NULL,
	[SentAt] DATETIME NULL,
	[ReceivedAddress1] VARCHAR(45) NULL,
	[ReceivedAddress2] VARCHAR(30) NULL,
	[ReceivedCity] VARCHAR(20) NULL,
	[ReceivedState] VARCHAR(2) NULL,
	[ReceivedZipCode] VARCHAR(17) NULL,
	[ReceivedPhoneNumber] VARCHAR(27) NULL,
	[ReceivedAt] DATETIME NULL,
	CONSTRAINT [FK_DemoLogs_Regions] FOREIGN KEY ([RegionId]) REFERENCES [accurint].[Regions] ([RegionId])
)
