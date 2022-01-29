/****** Object:  UserDefinedTableType [textcoord].[Ranks]    Script Date: 10/31/2019 8:47:41 AM ******/
CREATE TYPE [textcoord].[SearchResults] AS TABLE(
	AccountNumber VARCHAR(10),
	FirstName VARCHAR(13),
	LastName VARCHAR(23),
	PerformanceCategory INT,
	PhoneNumber VARCHAR(10),
	Segment INT,
	StateCode VARCHAR(2),
	MaximumDelinquency INT,
	Age INT,
	PhoneType CHAR(1)
)