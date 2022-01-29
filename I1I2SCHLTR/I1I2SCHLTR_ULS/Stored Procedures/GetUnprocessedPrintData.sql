CREATE PROCEDURE [i1i2schltr].[GetUnprocessedPrintData]
AS
	--This maps to the PrintData object in c#
	SELECT
		PD.PrintDataId,
		PD.SSN,
		PD.[Queue],
		PD.Firstname,
		PD.MiddleInitial,
		PD.LastName,
		PD.Address1,
		PD.Address2,
		PD.City,
		PD.[State],
		'' AS Country, --The object contrains this but it is not used
		PD.Zip,
		PD.Phone,
		PD.AlternatePhone,
		PD.Email,
		PD.School,
		PD.SchoolStatus,
		PD.RunDateId
	FROM
		i1i2schltr.PrintData PD
	WHERE
		PD.ProcessedAt IS NULL
		AND PD.DeletedAt IS NULL
		AND PD.DeletedBy IS NULL
RETURN 0
