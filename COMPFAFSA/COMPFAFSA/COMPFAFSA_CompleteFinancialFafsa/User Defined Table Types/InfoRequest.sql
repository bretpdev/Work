CREATE TYPE [compfafsa].[InfoRequest] AS TABLE
(
	[FirstName] VARCHAR(200) NOT NULL,
	[LastName] VARCHAR(200) NOT NULL,
	[EmailAddress] VARCHAR(1000) NOT NULL,
	[PhoneNumber] VARCHAR(20) NOT NULL,
	[School] VARCHAR(200) NOT NULL,
	[Classes] VARCHAR(1000) NOT NULL,
	[DataType] VARCHAR(100) NOT NULL,
	[AdditionalInformation] VARCHAR(2000) NULL
)
