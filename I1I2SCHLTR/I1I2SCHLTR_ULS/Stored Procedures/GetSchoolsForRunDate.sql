CREATE PROCEDURE [i1i2schltr].[GetSchoolsForRunDate]
	@RunDateIds RunDateIds READONLY
AS
	SELECT
		CD.SSN,
		CD.RunDateId,
		PD.School,
		PD.SchoolStatus
	FROM
		i1i2schltr.CommentData CD
		INNER JOIN @RunDateIds RunDate --This is passed in from the script so that we only pull back data on the relevant sas files
			ON RunDate.RunDateId = CD.RunDateId
		INNER JOIN i1i2schltr.PrintData PD
			ON PD.SSN = CD.SSN
			AND PD.RunDateId = CD.RunDateId
			AND PD.DeletedAt IS NULL
			AND PD.DeletedBy IS NULL
	WHERE
		(CD.TaskProcessedAt IS NULL OR CD.CommentProcessedAt IS NULL)
		AND CD.DeletedAt IS NULL
		AND CD.DeletedBy IS NULL
RETURN 0
