IF OBJECT_ID('RefreshTableWithValidation', 'P') IS NOT NULL
  DROP PROCEDURE RefreshTableWithValidation
GO
CREATE PROCEDURE RefreshTableWithValidation
	@TableName VARCHAR(50),
	@OverrideDateColumn VARCHAR(50) = NULL,
	@OverrideLocalTableName VARCHAR(50) = NULL
AS
BEGIN

	DECLARE @ServerName VARCHAR(30)
	DECLARE @RemoteSchema VARCHAR(30)
	IF DB_NAME() = 'CDW'
	BEGIN
		SET @ServerName = 'LEGEND'
		SET @RemoteSchema = 'PKUB'
	END
	ELSE IF DB_NAME() = 'UDW'
	BEGIN
		SET @ServerName = 'DUSTER'
		SET @RemoteSchema = 'OLWHRM1'	
	END
	ELSE
		RAISERROR('This procedure may only be run on UDW or CDW', 16, 11)

	--### CONFIGURATION
	DECLARE @LocalTableName VARCHAR(MAX) = ISNULL(@OverrideLocalTableName, @TableName)
	IF NOT @LocalTableName LIKE '%.%' SET @LocalTableName = 'dbo.' + @LocalTableName
	DECLARE @RemoteTableName VARCHAR(MAX) = @TableName
	IF NOT @RemoteTableName LIKE '%.%' SET @RemoteTableName = @RemoteSchema + '.' + @RemoteTableName
	DECLARE @DateColumn VARCHAR(MAX) = ISNULL(@OverrideDateColumn, 'LF_LST_DTS_')
	IF @DateColumn LIKE '%[_]' SET @DateColumn = @DateColumn + SUBSTRING(@TableName, 0, CHARINDEX('_', @TableName, 0))



	--### CALCULATE WHERE CLAUSE ###
	DECLARE @LocalTableNameWithoutSchema VARCHAR(MAX) = SUBSTRING(@LocalTableName, CHARINDEX('.', @LocalTableName, 0) + 1, LEN(@LocalTableName))
	DECLARE @WhereClause NVARCHAR(MAX) = ''
	SELECT 
		@WhereClause = @WhereClause + 'R.' + CCU.COLUMN_NAME + ' = LOCAL.' + CCU.COLUMN_NAME + ' AND '
	FROM 
		INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
		JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU ON TC.CONSTRAINT_NAME = CCU.Constraint_name
	WHERE 
		TC.CONSTRAINT_TYPE = 'Primary Key' and (@LocalTableNameWithoutSchema = TC.TABLE_NAME)
	SET @WhereClause = SUBSTRING(@WhereClause, 0, LEN(@WhereClause) - 3) --strip remaining AND

	--### CALCULATE UPDATE CLAUSE ###
	DECLARE @UpdateClause VARCHAR(MAX) = ''
	SELECT 
		@UpdateClause = @UpdateClause + 'LOCAL.' + C.COLUMN_NAME + ' = R.' + C.COLUMN_NAME + ',' + CHAR(13)
	FROM 
		INFORMATION_SCHEMA.COLUMNS C
	WHERE 
		(@LocalTableNameWithoutSchema = C.TABLE_NAME)
		AND
		C.COLUMN_NAME NOT IN 
		(
			SELECT 
				CCU.COLUMN_NAME 
			FROM 
				INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
				JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU ON TC.CONSTRAINT_NAME = CCU.Constraint_name
			WHERE 
				TC.CONSTRAINT_TYPE = 'Primary Key' and (@LocalTableNameWithoutSchema = TC.TABLE_NAME)
		)
	SET @UpdateClause = SUBSTRING(@UpdateClause, 0, LEN(@UpdateClause) - 1) --strip remaining comma

	--### CALCULATE INSERT CLAUSE ###
	DECLARE @InsertClauseTop NVARCHAR(MAX) = ''
	DECLARE @InsertClauseBottom VARCHAR(MAX) = ''
	SELECT 
		@InsertClauseTop = @InsertClauseTop + C.COLUMN_NAME + ',' + CHAR(13),
		@InsertClauseBottom = @InsertClauseBottom + 'R.' + C.COLUMN_NAME + ',' + CHAR(13)
	FROM 
		INFORMATION_SCHEMA.COLUMNS C
	WHERE 
		(@LocalTableNameWithoutSchema = C.TABLE_NAME)

	SET @InsertClauseTop = SUBSTRING(@InsertClauseTop, 0, LEN(@InsertClauseTop) - 1) --strip remaining comma
	SET @InsertClauseBottom = SUBSTRING(@InsertClauseBottom, 0, LEN(@InsertClauseBottom) - 1) --strip remaining comma


	DECLARE 
		@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
		@LoopCount TINYINT = 0

	RefreshStart:

	DECLARE @LastRefresh VARCHAR(30) 
	DECLARE @LastRefreshQuery NVARCHAR(MAX) = 'SELECT @LastRefresh = CONVERT(VARCHAR(30), ISNULL(MAX(LOCAL.' + @DateColumn + '), ''1-1-1900 00:00:00''), 21) FROM ' + @LocalTableName + ' LOCAL'
	EXEC sp_executesql @LastRefreshQuery, N'@LastRefresh VARCHAR(30) OUT', @LastRefresh out
	PRINT 'Last Refreshed at: ' + @LastRefresh

	DECLARE @SQLStatement VARCHAR(MAX) = 
	'
		MERGE 
				' + @LocalTableName + ' LOCAL
		USING
			(
				SELECT
					*
				FROM
					OPENQUERY
					(
						' + @ServerName + ',
						''
							SELECT
								REMOTE.*
							FROM
								' + @RemoteTableName + ' REMOTE
							WHERE
								REMOTE.' + @DateColumn + ' > ''''' + @LastRefresh + '''''
								OR
								(
									REMOTE.BF_SSN IN
									(
										' + @SSNs + '
									)
								)
						''
					) 
			) R ON ' + @WhereClause + '
			WHEN MATCHED THEN 
				UPDATE SET 
	' + @UpdateClause + '
			WHEN NOT MATCHED THEN
				INSERT 
				(
	' + @InsertClauseTop + '
				)
				VALUES 
				(
	' + @InsertClauseBottom + '
				)
		-- !!! uncomment lines below ONLY when doing a full table refresh 
		--WHEN NOT MATCHED BY SOURCE THEN
		--    DELETE
		;
	'

	PRINT @SQLStatement
	EXEC (@SQLStatement)


	-- ###### VALIDATION
	DECLARE 
		@CountDifference INT
	DECLARE @GetCountDifferenceQuery NVARCHAR(MAX) = 
	'
		SELECT
			@CountDifference = L.LocalCount - R.RemoteCount
		FROM
			OPENQUERY
			(
				' + @ServerName + ',
				''
					SELECT
						COUNT(*) AS "RemoteCount"
					FROM
						' + @RemoteTableName + '
				''
			) R
			FULL OUTER JOIN
			(
				SELECT
					COUNT(*) [LocalCount]
				FROM
					' + @LocalTableName + '
			) L ON 1 = 1
	'
	EXEC sp_executesql @GetCountDifferenceQuery, N'@CountDifference INT OUT', @CountDifference out
	IF @CountDifference != 0 AND @LoopCount > 0
		BEGIN
			RAISERROR('%s - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @RemoteTableName, @CountDifference)
		END
	ELSE IF @CountDifference != 0 AND @LoopCount = 0
		BEGIN

			SET @LoopCount = @LoopCount + 1
		
			DECLARE @SSN_LIST TABLE
			(
				R_BF_SSN CHAR(9),
				L_BF_SSN CHAR(9)
			)

			PRINT 'Insert SSN with inconsistent counts'
			DECLARE @SsnQuery NVARCHAR(MAX) = 
			'
				SELECT TOP 20
					R.BF_SSN,
					L.BF_SSN
				FROM
					OPENQUERY
					(
						' + @ServerName + ',	
						''
							SELECT
								REMOTE.BF_SSN,
								COUNT(*) AS "RemoteCount"
							FROM
								' + @RemoteTableName + ' REMOTE
							GROUP BY
								REMOTE.BF_SSN
						''
					) R
					FULL OUTER JOIN
					(
						SELECT
							LOCAL.BF_SSN,
							COUNT(*) [LocalCount]
						FROM
							' + @LocalTableName + ' LOCAL
						GROUP BY
							LOCAL.BF_SSN
					) L ON L.BF_SSN = R.BF_SSN
				WHERE
					ISNULL(L.LocalCount, 0) != ISNULL(R.RemoteCount, 0)
			'
			INSERT INTO
				@SSN_LIST
			EXEC (@SsnQuery)

			SELECT
				@SSNs = 
				(
					SELECT
						'''''' + COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) + ''''',' AS [text()]
					FROM
						@SSN_LIST SL
					ORDER BY
						COALESCE(SL.L_BF_SSN, SL.R_BF_SSN)
					FOR XML PATH ('')
				)

			SELECT	@SSNs = LEFT(@SSNs, LEN(@SSNs) -1)
			PRINT 'The local record count of some SSNs does not match the remote warehouse count. Deleting all local ' + @LocalTableName + ' records for these borrowers fullying refreshing from the remote warehouse.'

			DECLARE @DeleteBadQuery NVARCHAR(MAX) = 
			'
				DELETE
					LOCAL
				FROM
					' + @LocalTableName + ' LOCAL
				WHERE
					LOCAL.BF_SSN IN (' + REPLACE(@Ssns, '''', '') + ')
			'
			EXEC(@DeleteBadQuery)
			PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

			GOTO RefreshStart;

		END
END