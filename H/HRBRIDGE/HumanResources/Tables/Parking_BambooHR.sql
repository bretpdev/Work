CREATE TABLE [hrbridge].[Parking_BambooHR]
(
	[ParkingId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeId] INT NOT NULL,
	[UpdatedAt] DATETIME NOT NULL,
	[Garage] VARCHAR(100),
	[Type] VARCHAR(100),
	[FobId] VARCHAR(200)
)
