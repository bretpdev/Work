CREATE PROCEDURE GetDefaultProgrammer
AS
BEGIN

	SELECT TOP 1
		URT.UserId
	FROM
		UserRequestTypes URT
		LEFT JOIN
		SackerCache SC ON SC.AssignedProgrammer = URT.UserId AND Court = AssignedProgrammer
	WHERE
		URT.Developer = 1
	GROUP BY
		URT.UserId
	ORDER BY ISNULL(SUM(DevEstimate), 0) ASC, URT.UserId

END