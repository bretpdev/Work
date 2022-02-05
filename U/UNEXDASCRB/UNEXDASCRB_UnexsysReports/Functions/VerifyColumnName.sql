CREATE FUNCTION [unexdascrb].[VerifyColumnName]
(
	@ColumnName VARCHAR(50)
)
RETURNS BIT
AS
BEGIN
	IF EXISTS(
		SELECT      
			c.[name] AS [ColumnName]
		FROM
			sys.columns c
			INNER JOIN sys.tables  t   
				ON c.object_id = t.object_id
		WHERE
			t.[name] = 'RPT_CALLACTIVITY'
			AND
			c.[name] = @ColumnName
	) RETURN 1
	RETURN 0
END
