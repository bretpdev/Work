----keep for future reference
--USE ServicerInventoryMetrics
--GO
--SELECT * FROM [dbo].[ServicerCategory] A --main spreadsheet categories
--	INNER JOIN [dbo].[ServicerMetrics] B --subcategories
--	ON A.ServicerCategoryId = B.ServicerCategoryId
--	INNER JOIN [dbo].[MetricsSummary] C --specific monthly data
--	ON B.ServicerMetricsId = C.ServicerMetricsId
--WHERE C.ServicerMetricsId = XX --change to target metric from ServicerMetrics table


--run on UHEAASLDB
USE [ServicerInventoryMetrics]
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX

--X) BORROWER EMAIL
	UPDATE
		[dbo].[MetricsSummary]
	SET 
		[CompliantRecords] = XXXX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = X --borrower email
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= XXXX
		AND [MetricMonth]		= X --april
		AND [MetricYear]		= XXXX
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

--X) BORROWER EMAIL
	UPDATE 
		[dbo].[MetricsSummary]
	SET 
		[CompliantRecords] = XXXX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = X --borrower email
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= XXXX
		AND [MetricMonth]		= X --may
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) OTHER ESCALATED MAIL
	UPDATE 
		[dbo].[MetricsSummary]
	SET 
		[CompliantRecords] = XX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = X --other escalated mail
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= XX
		AND [MetricMonth]		= X --may
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) AGING +XXX & SENT TO DMCS TO PROCESS
	UPDATE 
		[dbo].[MetricsSummary]
	SET 
		[CompliantRecords] = XX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = XX --aging +XXX & sent to DMCS to process
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= XX
		AND [MetricMonth]		= X --june
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) PAYMENT SUSPENSE feb.XX
	INSERT INTO [dbo].[MetricsSummary]
	(  
		[ServicerMetricsId]
		,[CompliantRecords]
		,[TotalRecords]
		,[MetricMonth]
		,[MetricYear]
		,[AverageBacklogAge]
		,[UpdatedAt]
		,[UpdatedBy]
	)
	VALUES
	(XX,XXXXXX,XXXXXXX,X,XXXX,X,GETDATE(),SYSTEM_USER)
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) PAYMENT SUSPENSE march.XX
	INSERT INTO [dbo].[MetricsSummary]
	(  
		[ServicerMetricsId]
		,[CompliantRecords]
		,[TotalRecords]
		,[MetricMonth]
		,[MetricYear]
		,[AverageBacklogAge]
		,[UpdatedAt]
		,[UpdatedBy]
	)
	VALUES
	(XX,XXXXXX,XXXXXXX,X,XXXX,X,GETDATE(),SYSTEM_USER)
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) PAYMENT SUSPENSE april.XX
	UPDATE 
		[dbo].[MetricsSummary]
	SET 
		[CompliantRecords] = XXXXXX
		,[TotalRecords]	   = XXXXXXXX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = XX --payment suspense
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= X
		AND [MetricMonth]		= X --april
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) PAYMENT SUSPENSE may.XX
	UPDATE 
		[dbo].[MetricsSummary]
	SET 
		 [TotalRecords]	   = XXXXXXX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = XX --payment suspense
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= X
		AND [MetricMonth]		= X --may
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--X) PAYMENT SUSPENSE june.XX
	UPDATE 
		[dbo].[MetricsSummary]
	SET 
		 [TotalRecords]	   = XXXXXXX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = XX --payment suspense
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= X
		AND [MetricMonth]		= X --june
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--XX) PAYMENT SUSPENSE july.XX
	UPDATE 
		[dbo].[MetricsSummary]
	SET
		[CompliantRecords] = XXXXX
		,[TotalRecords]	   = XXXXXXX
		,[UpdatedAt]	   = GETDATE()
		,[UpdatedBy]	   = SYSTEM_USER
	WHERE
		[MetricsSummaryId]		= XXXXX
		AND [ServicerMetricsId] = XX --payment suspense
		AND [CompliantRecords]  = X
		AND [TotalRecords]		= X
		AND [MetricMonth]		= X --july
		AND [MetricYear]		= XXXX
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
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
