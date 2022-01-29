CREATE TYPE [hrbridge].[ParkingRecord_BambooHR] AS TABLE
(
	[EmployeeId] INT,
	[UpdatedAt] DATETIME,
	[Garage] VARCHAR(100),
	[Type] VARCHAR(100),
	[FobId] VARCHAR(200)
)
