﻿CREATE PROCEDURE billing.[SetPrintedAt]
	@PrintProcessingId int
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		billing.PrintProcessing
	SET
		PrintedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId
END;

	SELECT
		@Time
RETURN 0;