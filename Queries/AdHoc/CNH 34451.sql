--run on UHEAASQLDB
USE ServicerInventoryMetrics
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX
	INSERT INTO ServicerInventoryMetrics..MetricsSummary(ServicerMetricsId, CompliantRecords, TotalRecords, MetricMonth, MetricYear, AverageBacklogAge, UpdatedAt, UpdatedBy)
VALUES
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'), /*end foia*/
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,XX,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts'),
	(X,X,X,X,XXXX,NULL,GETDATE(),'UHEAA\Batchscripts') /*end account research*/

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END