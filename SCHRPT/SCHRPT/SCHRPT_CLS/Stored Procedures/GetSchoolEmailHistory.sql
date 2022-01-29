CREATE PROCEDURE [schrpt].[GetSchoolEmailHistory]
AS

	SELECT
		s.SchoolEmailHistoryId, s.SchoolRecipientId, s.EmailSentAt
	FROM
		schrpt.SchoolEmailHistory s
	WHERE
		CONVERT(date, s.EmailSentAt) > CONVERT(date, DATEADD(day, -7, GETDATE()) )

RETURN 0