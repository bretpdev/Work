USE ServicerInventoryMetrics

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedCount INT = X

UPDATE 
	MS
SET
	MS.CompliantRecords = XXX
FROM
	ServicerInventoryMetrics..MetricsSummary MS
	INNER JOIN ServicerInventoryMetrics..ServicerMetrics SM
		ON SM.ServicerMetricsId = MS.ServicerMetricsId
	INNER JOIN ServicerInventoryMetrics..ServicerCategory SC
		ON SC.ServicerCategoryId = SM.ServicerCategoryId
WHERE
	SM.ServicerMetric = 'Borrower Mail - Letters'
	AND SC.ServicerCategory = 'Borrower Communication'
	AND MS.MetricYear = XXXX
	AND MS.MetricMonth = XX

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

UPDATE 
	MS
SET
	MS.CompliantRecords = XX
FROM
	ServicerInventoryMetrics..MetricsSummary MS
	INNER JOIN ServicerInventoryMetrics..ServicerMetrics SM
		ON SM.ServicerMetricsId = MS.ServicerMetricsId
	INNER JOIN ServicerInventoryMetrics..ServicerCategory SC
		ON SC.ServicerCategoryId = SM.ServicerCategoryId
WHERE
	SM.ServicerMetric = 'Borrower Mail - Letters'
	AND SC.ServicerCategory = 'Borrower Communication'
	AND MS.MetricYear = XXXX
	AND MS.MetricMonth = X

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

UPDATE 
	MS
SET
	MS.CompliantRecords = XX
FROM
	ServicerInventoryMetrics..MetricsSummary MS
	INNER JOIN ServicerInventoryMetrics..ServicerMetrics SM
		ON SM.ServicerMetricsId = MS.ServicerMetricsId
	INNER JOIN ServicerInventoryMetrics..ServicerCategory SC
		ON SC.ServicerCategoryId = SM.ServicerCategoryId
WHERE
	SM.ServicerMetric = 'Borrower Mail - Letters'
	AND SC.ServicerCategory = 'Borrower Communication'
	AND MS.MetricYear = XXXX
	AND MS.MetricMonth = X

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

UPDATE 
	MS
SET
	MS.CompliantRecords = XX
FROM
	ServicerInventoryMetrics..MetricsSummary MS
	INNER JOIN ServicerInventoryMetrics..ServicerMetrics SM
		ON SM.ServicerMetricsId = MS.ServicerMetricsId
	INNER JOIN ServicerInventoryMetrics..ServicerCategory SC
		ON SC.ServicerCategoryId = SM.ServicerCategoryId
WHERE
	SM.ServicerMetric = 'Borrower Mail - Letters'
	AND SC.ServicerCategory = 'Borrower Communication'
	AND MS.MetricYear = XXXX
	AND MS.MetricMonth = X

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

UPDATE 
	MS
SET
	MS.CompliantRecords = XX
FROM
	ServicerInventoryMetrics..MetricsSummary MS
	INNER JOIN ServicerInventoryMetrics..ServicerMetrics SM
		ON SM.ServicerMetricsId = MS.ServicerMetricsId
	INNER JOIN ServicerInventoryMetrics..ServicerCategory SC
		ON SC.ServicerCategoryId = SM.ServicerCategoryId
WHERE
	SM.ServicerMetric = 'Borrower Mail - Letters'
	AND SC.ServicerCategory = 'Borrower Communication'
	AND MS.MetricYear = XXXX
	AND MS.MetricMonth = X

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	

IF @ROWCOUNT = @ExpectedCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END





  
  
 
  
