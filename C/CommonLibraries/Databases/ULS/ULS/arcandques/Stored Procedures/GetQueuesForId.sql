CREATE PROCEDURE [arcandques].[GetQueuesForId]
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
			WF_QUE || WF_SUB_QUE || '''' - '''' || WC_TYP_USR_QUE AS Queue
		FROM
			OLWHRM1.WQ60_TSK_PL_ASN
		WHERE
			PF_USR = ''''' + CAST(@UtId AS CHAR(8)) + '''''
		ORDER BY
			Queue
	'')'
EXEC(@QUERY)
GO
GRANT EXECUTE
    ON OBJECT::[arcandques].[GetQueuesForId] TO [db_executor]
    AS [dbo];

