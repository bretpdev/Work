CREATE PROCEDURE [arcandques].[GetARCsForId]
	@TestMode bit,
	@UtId CHAR(8)
AS
DECLARE @Query VARCHAR(Max)
IF @TestMode = 1
	SET @Query = 'SELECT * FROM OPENQUERY(QADBD004,'
ELSE 
	SET @Query = 'SELECT * FROM OPENQUERY(DUSTER,'

SELECT @Query = @Query + 
	'''
		SELECT
			PF_REQ_ACT
		FROM
			OLWHRM1.US50_USR_ACT_VLD
		WHERE
			PF_USR = ''''' + CAST(@UtId AS CHAR(8)) + '''''
		ORDER BY
			PF_REQ_ACT
	'')'
EXEC(@QUERY)
GO
GRANT EXECUTE
    ON OBJECT::[arcandques].[GetARCsForId] TO [db_executor]
    AS [dbo];

