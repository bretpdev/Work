BEGIN TRAN
GO

--Select the database to use
USE CSYS
GO

DECLARE @Row  float
DECLARE @NumberShouldBeAffected  int

--Change this number if you know how many should be affected
SELECT
	 @NumberShouldBeAffected = COUNT(TEN.SqlUserId)
FROM
	 dbo.SYSA_DAT_UserKeyAssignment TEN
	 JOIN dbo.SYSA_DAT_UserKeyAssignment THREE
		ON 	TEN.[SqlUserId] = THREE.[SqlUserId]
		AND TEN.[UserKey] = THREE.[UserKey]
		AND TEN.[StartDate] = THREE.[StartDate]
WHERE 
	TEN.BusinessUnit = 10
	AND THREE.BusinessUnit = 3
	
PRINT @NumberShouldBeAffected

--SQL Statement goes here
DELETE
FROM
	dbo.SYSA_DAT_UserKeyAssignment
WHERE
	ID IN
	(
		SELECT
			TEN.ID
		FROM
			dbo.SYSA_DAT_UserKeyAssignment TEN
			JOIN dbo.SYSA_DAT_UserKeyAssignment THREE
				ON 	TEN.[SqlUserId] = THREE.[SqlUserId]
				AND TEN.[UserKey] = THREE.[UserKey]
				AND TEN.[StartDate] = THREE.[StartDate]
		WHERE 
			TEN.BusinessUnit = 10
			AND THREE.BusinessUnit = 3
	)

--This must be the first thing after the sql statement
SET @Row = @@ROWCOUNT

--This should never change
IF @Row <> @NumberShouldBeAffected
	BEGIN
		ROLLBACK
		PRINT 'Looking for ' + CAST(@NumberShouldBeAffected AS VARCHAR(5)) + ' rows. Rolled Back because there are ' + CAST(@Row as varchar(5)) + ' rows affected'
	END
ELSE
	BEGIN
		COMMIT
		PRINT 'Committed ' + CAST(@Row AS VARCHAR(5)) + ' rows'
	END