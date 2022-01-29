CREATE PROCEDURE [unexdascrb].[GetScrubbableColumns]
AS

	DECLARE @Prefix NVARCHAR(20) = 'Bussfld' --currently scrubbing all columns that start with BussFld (default of 25 columns)

	SELECT      
		c.[name] AS [ColumnName]
	FROM
		sys.columns c
		INNER JOIN sys.tables  t   
			ON c.object_id = t.object_id
	WHERE
		t.[name] = 'RPT_CALLACTIVITY'
		AND
		c.[name] LIKE @Prefix + '%'
	ORDER BY
		RIGHT('000' + REPLACE(c.[name], @Prefix, ''), 3); --left-pad the numbers so the ordering makes sense

RETURN 0
