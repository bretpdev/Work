CREATE PROCEDURE [rcdialer].[SetProcessedAt]
	@OutboundCallsId INT
AS
	UPDATE
		rcdialer.OutboundCalls
	SET
		ProcessedAt = GETDATE()
	WHERE
		OutboundCallsId = @OutboundCallsId