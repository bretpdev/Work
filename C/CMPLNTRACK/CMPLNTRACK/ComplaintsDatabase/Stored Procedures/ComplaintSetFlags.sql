CREATE PROCEDURE [complaints].[ComplaintSetFlags]
	@ComplaintId INT,
	@FlagIds complaints.IntTable READONLY
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	DELETE FROM
		complaints.ComplaintFlags
	 WHERE
		ComplaintId = @ComplaintId 
		AND FlagId NOT IN (SELECT Id FROM @FlagIds)

	INSERT INTO complaints.ComplaintFlags(ComplaintId, FlagId)
	SELECT 
		@ComplaintId,
		F.FlagId
	FROM complaints.Flags F
		INNER JOIN @FlagIds FI 
			ON FI.Id = F.FlagId
		LEFT JOIN complaints.ComplaintFlags CF
			ON F.FlagId = CF.FlagId
			AND CF.ComplaintId = @ComplaintId
	WHERE
		CF.FlagId IS NULL

RETURN 0