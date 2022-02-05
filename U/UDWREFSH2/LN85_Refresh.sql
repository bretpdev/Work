USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN85_LON_ATY
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.LN85_LON_ATY
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN85.LF_LST_DTS_LN85), '1-1-1900 00:00:00'), 21) FROM LN85_LON_ATY LN85)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN85_LON_ATY LN85
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
							LN85.*
						FROM
							OLWHRM1.LN85_LON_ATY LN85
						-- comment WHERE clause for full table refresh
						WHERE
							LN85.LF_LST_DTS_LN85 > ''''' + @LastRefresh + '''''
							OR
							(
								LN85.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) D 
		) D ON  LN85.BF_SSN = D.BF_SSN AND LN85.LN_SEQ = D.LN_SEQ AND LN85.LN_ATY_SEQ = D.LN_ATY_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN85.LF_LST_DTS_LN85 = D.LF_LST_DTS_LN85
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LN_ATY_SEQ,
			LF_LST_DTS_LN85
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ,
			D.LN_ATY_SEQ,
			D.LF_LST_DTS_LN85
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'

PRINT @SQLStatement
EXEC (@SQLStatement)


-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.LN85_LON_ATY LN85
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..LN85_LON_ATY LN85
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
BEGIN
	RAISERROR('LN85_LON_ATY LN85 - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(9),
			L_BF_SSN CHAR(9)
		)

		PRINT 'Insert SSN with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.BF_SSN,
			L.BF_SSN
		FROM
			OPENQUERY
			(
				DUSTER,	
				'
					SELECT
						LN85.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.LN85_LON_ATY LN85
					GROUP BY
						LN85.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN85.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..LN85_LON_ATY LN85
				GROUP BY
					LN85.BF_SSN
			) L ON L.BF_SSN = R.BF_SSN
		WHERE
			ISNULL(L.LocalCount, 0) != ISNULL(R.RemoteCount, 0)

		SELECT
			@SSNs = 
			(
				SELECT
					'''''' + COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) + ''''',' AS [text()]
				FROM
					@SSN_LIST SL
				ORDER BY
					COALESCE(SL.L_BF_SSN, SL.R_BF_SSN)
				FOR XML PATH ('')
			)

		SELECT	@SSNs = LEFT(@SSNs, LEN(@SSNs) -1)

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local LN85 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN85
		FROM
			UDW..LN85_LON_ATY LN85
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN85.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END