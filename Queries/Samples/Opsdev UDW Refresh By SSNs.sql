USE UDW
GO
--truncate table PD32_PRS_ADR_EML
DECLARE
	@TableName VARCHAR(50) = 'PD20_PRS_DTH', --paste in table name here
	@OverrideDateColumn VARCHAR(50) = 'N/A',
	@OverrideLocalTableName VARCHAR(50) = 'PD20_PRS_DTH',

	  @SSNs VARCHAR(1000) = ' ''''111488577'''', ''''185629159'''', ''''256712688'''', ''''272906206'''' ' --paste in ssns here

	DECLARE @ServerName VARCHAR(30) = 'QADBD004'
	DECLARE @RemoteSchema VARCHAR(30) = 'OLWHRM1'
	DECLARE @FilterField VARCHAR(50) = 'BF_SSN' -----CHANGE BF_SSN/DF_PRS_ID AS NEEDED
	DECLARE @RemoteWhere varchar(MAX) =  CHAR(13) + 'WHERE' + CHAR(13)+' REMOTE.' + @FilterField + ' IN (' + @SSNs + ')'

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
								' + @RemoteTableName + ' REMOTE'
							+ @RemoteWhere + '
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
		;
	'

	PRINT @SQLStatement
	EXEC (@SQLStatement)


	
