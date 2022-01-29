USE CDW
GO

DECLARE
	@TableName VARCHAR(50) = 'LN10_LON',
	@OverrideDateColumn VARCHAR(50) = 'LF_LST_DTS_LN10',
	@OverrideLocalTableName VARCHAR(50) = NULL,
	@SSNs VARCHAR(1000) = ' ''''011620772'''', ''''321654987'''', ''''789456123'''' ' 

	PRINT @SSNs
	DECLARE @ServerName VARCHAR(30) = 'LEGEND_TEST_VUK3'
	DECLARE @RemoteSchema VARCHAR(30) = 'PKUB'


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
								REMOTE.BF_SSN IN
								(
									' + @SSNs + '
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


	