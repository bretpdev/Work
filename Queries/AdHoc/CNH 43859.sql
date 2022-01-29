USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	DELETE
		CA
	FROM
		NeedHelpCornerStone..LST_CourtAssignment CA
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON CA.SqlUserId = U.SqlUserId
	WHERE
		U.FirstName = 'nghia'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		CA
	SET
		CA.Rotation = X
	FROM
		NeedHelpCornerStone..LST_CourtAssignment CA
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON CA.SqlUserId = U.SqlUserId
	WHERE
		u.WindowsUserName = 'jblair'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		CA
	SET
		CA.Rotation = X
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
		CA.Rotation = X
	FROM
		NeedHelpCornerStone..LST_CourtAssignment CA
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON CA.SqlUserId = U.SqlUserId
	WHERE
		u.WindowsUserName = 'ccole'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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