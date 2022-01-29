CREATE PROCEDURE GetDefaultTester
AS
BEGIN

	SELECT TOP 1
		URT.UserId
	FROM
		UserRequestTypes URT
		LEFT JOIN
		SackerCache SC ON SC.AssignedTester = URT.UserId AND Court = AssignedTester
	WHERE
		URT.Developer = 0
	GROUP BY
		URT.UserId
	ORDER BY ISNULL(SUM(TestEstimate), 0) ASC, URT.UserId

END