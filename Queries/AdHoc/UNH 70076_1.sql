USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		CA
	SET
		CA.Rotation = 1
	FROM
		NeedHelpCornerStone..LST_CourtAssignment CA
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON CA.SqlUserId = U.SqlUserId
	WHERE
		u.WindowsUserName = 'jblair'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		CA
	SET
		CA.Rotation = 2
	FROM
		NeedHelpCornerStone..LST_CourtAssignment CA
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON CA.SqlUserId = U.SqlUserId
	WHERE
		u.WindowsUserName = 'dhalladay'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		CA
	SET
		CA.Rotation = 3
	FROM
		NeedHelpCornerStone..LST_CourtAssignment CA
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON CA.SqlUserId = U.SqlUserId
	WHERE
		u.WindowsUserName = 'ccole'

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