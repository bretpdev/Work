USE ServicerInventoryMetrics
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		MS
	SET
		CompliantRecords = XXXX,
		TotalRecords = XXXX
	FROM
		ServicerInventoryMetrics..MetricsSummary MS
		LEFT JOIN ServicerInventoryMetrics..ServicerMetrics SM
			ON MS.ServicerMetricsId = SM.ServicerMetricsId
	WHERE
		MS.MetricYear = XXXX
		AND
		MS.MetricMonth = XX
		AND
		SM.ServicerMetric = 'Borrower Email'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END