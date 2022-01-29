CREATE PROCEDURE [monitor].[EndRunHistory]
	@RunHistoryId INT
AS

	UPDATE
		monitor.RunHistory
	SET
		EndedAt = GETDATE()
	WHERE
		RunHistoryId = @RunHistoryId

RETURN 0
