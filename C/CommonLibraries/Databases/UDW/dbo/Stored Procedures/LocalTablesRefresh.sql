

CREATE PROCEDURE [dbo].[LocalTablesRefresh]
 
	@TableName VARCHAR(50) , 
	@OverrideAuditTableName VARCHAR(50) = NULL
AS
BEGIN

	DECLARE @DatabaseName VARCHAR(30)
	DECLARE @DatabaseOrigin VARCHAR(30)
	IF DB_NAME() = 'CDW'
	BEGIN
		SET @DatabaseName = 'AuditCDW'
		SET @DatabaseOrigin =DB_NAME()
	END 
	ELSE IF DB_NAME() = 'UDW'
	BEGIN
		SET @DatabaseName = 'AuditUDW'
		SET @DatabaseOrigin = DB_NAME()	
	END
	ELSE
		RAISERROR('This procedure may only be run on UDW or CDW', 16, 11)

	/* ### CONFIGURATION */
	DECLARE @AuditTableName VARCHAR(MAX) = ISNULL(@OverrideAuditTableName, @TableName)
		IF NOT @AuditTableName LIKE '%.%' SET @AuditTableName = @DatabaseName + '.dbo.' + @AuditTableName
	DECLARE @OriginTableName VARCHAR(MAX) = @TableName
		IF NOT @OriginTableName LIKE '%.%' SET @OriginTableName = @DatabaseOrigin + '.dbo.' + @OriginTableName


	/* Delete Statement*/
	DECLARE @SQLStatement VARCHAR(MAX) = 'DELETE FROM ' + @AuditTableName 
	
	PRINT @SQLStatement
		EXEC (@SQLStatement)

	/*Insert Statement*/
	DECLARE @SQLInsert VARCHAR(MAX) = 'INSERT INTO ' + @AuditTableName + ' SELECT * FROM ' +  @OriginTableName

	PRINT @SQLInsert
		EXEC(@SQLInsert)

	END