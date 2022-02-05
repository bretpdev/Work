
CREATE PROCEDURE [dbo].[LN80]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

--### CONFIGURATION
DECLARE @LocalTableName VARCHAR(MAX) = 'dbo.LN80_LON_BIL_CRF_1'
DECLARE @RemoteTableName VARCHAR(MAX) = 'OLWHRM1.LN80_LON_BIL_CRF'
DECLARE @DateColumn VARCHAR(MAX) = 'LD_LST_DTS_LN80'
DECLARE @DaysPerCycle VARCHAR(MAX) = '15'


--EXEC ('DELETE FROM ' + @LocalTableName)

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

--### CALCULATE REMOTE MIN DATE ###
DECLARE @RemoteMinDate DATETIME
DECLARE @GetRemoteMinDateQuery VARCHAR(MAX) = 'SELECT MIN(' + @DateColumn + ') MINDATE FROM ' + @RemoteTableName
DECLARE @SetRemoteMinDateQuery NVARCHAR(MAX) = 'SELECT @RemoteMinDate = MINDATE FROM OPENQUERY(DUSTER,''' + @GetRemoteMinDateQuery + ''')'
EXEC sp_executesql @SetRemoteMinDateQuery, N'@RemoteMinDate DATETIME OUT', @RemoteMinDate out

--### REFRESH START AND MIN DATE ###
DECLARE @LocalMinDate VARCHAR(23)
DECLARE @StartDate VARCHAR(30)
DECLARE @RefreshMinQuery NVARCHAR(MAX) = 'SET @LocalMinDate = CONVERT(VARCHAR(30), (SELECT ISNULL(MIN(LOCAL.' + @DateColumn + '), GETDATE()) FROM ' + @LocalTableName + ' LOCAL), 21)'
DECLARE @RefreshStartQuery NVARCHAR(MAX) = 'SET @StartDate = CONVERT(VARCHAR(30), DATEADD(DAY, -' + @DaysPerCycle + ', (SELECT ISNULL(MIN(LOCAL.' + @DateColumn + '), GETDATE()) FROM ' + @LocalTableName + ' LOCAL)), 21)'

EXEC sp_executesql @RefreshMinQuery, N'@LocalMinDate VARCHAR(30) OUT', @LocalMinDate out
EXEC sp_executesql @RefreshStartQuery, N'@StartDate VARCHAR(30) OUT', @StartDate out

PRINT 'Start Date:  ' + @StartDate + '; Local Min Date:  ' + @LocalMinDate

PRINT @LocalMinDate
PRINT @RemoteMinDate

-- work back through time updating table
WHILE @LocalMinDate > @RemoteMinDate -- remote warehouse minimum date

BEGIN

	PRINT 'Start Date:  ' + @StartDate + '; Local Min Date:  ' + @LocalMinDate
	
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
						DUSTER,
						''
							SELECT
								REMOTE.*
							FROM
								' + @RemoteTableName + ' REMOTE
							WHERE
								REMOTE.' + @DateColumn + ' BETWEEN ''''' + @StartDate + ''''' AND ''''' + @LocalMinDate + '''''
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
BEGIN TRY
	EXEC (@SQLStatement)

	EXEC sp_executesql @RefreshMinQuery, N'@LocalMinDate VARCHAR(30) OUT', @LocalMinDate out
	EXEC sp_executesql @RefreshStartQuery, N'@StartDate VARCHAR(30) OUT', @StartDate out
END TRY
BEGIN CATCH
exec dbo.LN80
END CATCH

END
END