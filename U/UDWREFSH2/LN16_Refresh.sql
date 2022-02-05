USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN16_LON_DLQ_HST
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.LN16_LON_DLQ_HST
--		'
--	)
DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN16.LF_LST_DTS_LN16), '1-1-1900 00:00:00'), 21) FROM LN16_LON_DLQ_HST LN16)
PRINT 'Last Refreshed at: ' + @LastRefresh


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN16_LON_DLQ_HST LN16
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
							*
						FROM
							OLWHRM1.LN16_LON_DLQ_HST LN16
						-- comment WHERE clause for full table refresh
						WHERE
							LN16.LF_LST_DTS_LN16 > ''''' + @LastRefresh + '''''
							OR
							(
								LN16.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L ON L.BF_SSN = LN16.BF_SSN AND L.LN_SEQ = LN16.LN_SEQ AND L.LN_DLQ_SEQ = LN16.LN_DLQ_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN16.LC_DLQ_TYP = L.LC_DLQ_TYP,
			LN16.LC_STA_LON16 = L.LC_STA_LON16,
			LN16.LD_STA_LON16 = L.LD_STA_LON16,
			LN16.LD_DLQ_OCC = L.LD_DLQ_OCC,
			LN16.LN_DLQ_MAX = L.LN_DLQ_MAX,
			LN16.LN_DLQ_ITL = L.LN_DLQ_ITL,
			LN16.LD_DLQ_MAX = L.LD_DLQ_MAX,
			LN16.LD_DLQ_ITL = L.LD_DLQ_ITL,
			LN16.LF_LST_DTS_LN16 = L.LF_LST_DTS_LN16,
			LN16.LI_RSM_DUN_NXT_BKT = L.LI_RSM_DUN_NXT_BKT,
			LN16.LI_NO_DLQ_IC = L.LI_NO_DLQ_IC
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LN_DLQ_SEQ,
			LC_DLQ_TYP,
			LC_STA_LON16,
			LD_STA_LON16,
			LD_DLQ_OCC,
			LN_DLQ_MAX,
			LN_DLQ_ITL,
			LD_DLQ_MAX,
			LD_DLQ_ITL,
			LF_LST_DTS_LN16,
			LI_RSM_DUN_NXT_BKT,
			LI_NO_DLQ_IC
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_SEQ,
			L.LN_DLQ_SEQ,
			L.LC_DLQ_TYP,
			L.LC_STA_LON16,
			L.LD_STA_LON16,
			L.LD_DLQ_OCC,
			L.LN_DLQ_MAX,
			L.LN_DLQ_ITL,
			L.LD_DLQ_MAX,
			L.LD_DLQ_ITL,
			L.LF_LST_DTS_LN16,
			L.LI_RSM_DUN_NXT_BKT,
			L.LI_NO_DLQ_IC
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--    DELETE
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
				OLWHRM1.LN16_LON_DLQ_HST
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			LN16_LON_DLQ_HST
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN16_LON_DLQ_HST - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						LN16.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.LN16_LON_DLQ_HST LN16
					GROUP BY
						LN16.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN16.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..LN16_LON_DLQ_HST LN16
				GROUP BY
					LN16.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local LN16 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN16
		FROM
			UDW..LN16_LON_DLQ_HST LN16
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN16.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END