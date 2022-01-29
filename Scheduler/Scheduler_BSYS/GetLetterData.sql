CREATE PROCEDURE [scheduler].[GetLetterData]
AS

	SELECT
		1 RequestTypeId,
		[Request] [Id],
		Title [Name],
		CurrentStatus [Status],
		CAST(CAST([Priority] AS DECIMAL(9, 2)) AS TINYINT) [Priority], 
		[Court],
		SOC [AssignedProgrammer],
		SSA [AssignedTester]
	FROM
		[LTDB_Dat_Requests]


RETURN 0
