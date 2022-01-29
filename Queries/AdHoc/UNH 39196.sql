BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DELETE 
		A
	FROM 
		[NeedHelpCornerStone].[dbo].[LST_CourtAssignment] A
		INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] B 
			ON A.SqlUserId = B.SqlUserId
	WHERE 
		B.LastName = 'Hack' and B.FirstName = 'Wendy' --id:1152
	;
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DELETE 
		Y
	FROM 
		[NeedHelpUheaa].[dbo].[LST_CourtAssignment] Y
		INNER JOIN [CSYS].[dbo].[SYSA_DAT_Users] Z
			ON Y.SqlUserId = Z.SqlUserId
	WHERE 
		Z.LastName = 'Hack' and Z.FirstName = 'Wendy' --id:1152
	;

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--moving subsequent rotation up one
	UPDATE [NeedHelpUheaa].[dbo].[LST_CourtAssignment]
	SET Rotation = 3
	WHERE Rotation = 4
	;

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 3 AND @ERROR = 0
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
