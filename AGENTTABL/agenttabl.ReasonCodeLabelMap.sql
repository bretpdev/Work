USE NobleCalls
GO

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		agenttabl.ReasonCodeLabelMapUCCX RCLM
	USING
		(
			SELECT
				code,
				label,
				category,
				active,
				dateinactive
			FROM
				OPENQUERY
				(
					UCCX,
					''
						SELECT
							code,
							label,
							category,
							active,
							dateinactive
						FROM
							ReasonCodeLabelMap
					''
				) 
		) Remote 
			ON Remote.code = RCLM.code
			AND Remote.category = RCLM.category
	WHEN MATCHED THEN 
		UPDATE SET 
			RCLM.label = Remote.label,
			RCLM.active = Remote.active,
			RCLM.dateinactive = Remote.dateinactive
	WHEN NOT MATCHED THEN
		INSERT 
		(
			code,
			label,
			category,
			active,
			dateinactive
		)
		VALUES 
		(
			Remote.code,
			Remote.label,
			Remote.category,
			Remote.active,
			Remote.dateinactive
		)
	;
'
--select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

