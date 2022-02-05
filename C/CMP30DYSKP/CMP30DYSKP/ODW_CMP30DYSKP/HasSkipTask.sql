CREATE PROCEDURE [cmp30dyskp].[HasSkipTask]
	@Ssn CHAR(9),
	@Task VARCHAR(9)
AS
	SELECT
	CASE WHEN COUNT(1) > 0 THEN CAST(1 AS BIT) --Indicates if the passed-in borrower already has a matching skip task
	ELSE CAST(0 AS BIT)
	END
FROM
	dbo.CT30_CALL_QUE
WHERE
	IF_WRK_GRP = @Task
	AND DF_PRS_ID_BR = @Ssn
		
RETURN 0
