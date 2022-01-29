USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 206

UPDATE 
	AAP
SET 
	AAP.ProcessedAt = NULL
FROM
	ULS.dbo.ArcAddProcessing AAP
	INNER JOIN
	(
		SELECT
			ArcAddProcessingId
		FROM 
			NobleCalls..NobleCallHistory 
		WHERE 
			IsReconciled = 0 
			AND Deleted = 0 
			AND CallCampaign NOT IN ('VABU','VABC') 
			AND DATEDIFF(DAY,CreatedAt,'2017-02-22') = 0 
			AND IsInbound = 0
			AND RegionId = 2
	) Arcs ON Arcs.ArcAddProcessingId = AAP.ArcAddProcessingId

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END


