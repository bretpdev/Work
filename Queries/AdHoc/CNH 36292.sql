BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = X

	--SELECT * FROM [CLS].[dasforbfed].[Disasters]

	DECLARE @WrongDisasterId INT = (SELECT DisasterId FROM [CLS].[dasforbfed].[Disasters] WHERE AddedBy = 'CNH XXXXX');
	DECLARE @RightDisasterId INT = (SELECT DisasterId FROM [CLS].[dasforbfed].[Disasters] WHERE AddedBy = 'CNH XXXXX');

	--SELECT @WrongDisasterId,@RightDisasterId

	--SELECT * FROM [CLS].[dasforbfed].[Disasters] WHERE DisasterId = @RightDisasterId
	--SELECT * FROM [CLS].[dasforbfed].[Disasters] WHERE DisasterId = @WrongDisasterId

	--SELECT * FROM [CLS].[dasforbfed].[Zips] WHERE DisasterId = @RightDisasterId
	--SELECT * FROM [CLS].[dasforbfed].[Zips] WHERE DisasterId = @WrongDisasterId

	UPDATE 
		[CLS].[dasforbfed].[Zips]
	SET 
		DisasterId = @RightDisasterId
	WHERE
		DisasterId = @WrongDisasterId
	--X

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


	DELETE FROM
		[CLS].[dasforbfed].[Disasters] 
	WHERE
		DisasterId = @WrongDisasterId
	--X
	
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
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END