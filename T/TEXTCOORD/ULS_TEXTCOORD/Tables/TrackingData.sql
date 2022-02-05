CREATE TABLE [textcoord].[TrackingData]
(
	[TrackingDataId] INT IDENTITY NOT NULL PRIMARY KEY,
	AccountNumber VARCHAR(10),
	FirstName VARCHAR(13),
	LastName VARCHAR(23),
	PerformanceCategory INT,
	PhoneNumber VARCHAR(10),
	Segment INT,
	StateCode VARCHAR(2),
	MaximumDelinquency INT,
	Age INT,
	PhoneType CHAR(1),
	ContentType VARCHAR(20),
	[ArcAddProcessingId] BIGINT,
	CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
	DeletedAt DATETIME,
	DeletedBy VARCHAR(50)
)
