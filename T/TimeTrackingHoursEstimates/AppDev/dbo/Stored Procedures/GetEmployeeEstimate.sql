CREATE PROCEDURE [dbo].[GetEmployeeEstimate]
	@UserName VARCHAR(500)
	
AS
SELECT
	RequestType, 
	RequestNumber,
	EstimatedHours,
	SUM(AdditionalHrs) as AdditionalHrs,
	sum(TestingFixes) as TestHours
FROM
	Estimates
WHERE
	Employee = @UserName
GROUP BY
	RequestType, 
	RequestNumber,
	EstimatedHours
RETURN 0
