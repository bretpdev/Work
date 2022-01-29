--SPROC  can't be used because table doesn't have a BF_SSN field
--EXEC CDW..RefreshTableWithValidation 'GX25_USR', 'XF_LST_DTS_GX25'


USE CDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(GX25.XF_LST_DTS_GX25)), '1-1-1900 00:00:00'), 21) FROM GX25_USR GX25)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.GX25_USR GX25
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					LEGEND,
					''
						SELECT
							GX25.*
						FROM
							PKUB.GX25_USR GX25
						-- comment WHERE clause for full table refresh
						WHERE
							GX25.XF_LST_DTS_GX25 > ''''' + @LastRefresh + '''''
							OR
							(
								GX25.XF_USR IN
								(
									' + @SSNs + '
								)
							)
							FOR READ ONLY WITH UR
					''
				) 
		) D 
			ON GX25.XF_USR = D.XF_USR 
	WHEN MATCHED THEN 
		UPDATE SET
			GX25.XF_LST_DTS_GX25 = D.XF_LST_DTS_GX25,
			GX25.XC_GRP = D.XC_GRP,
			GX25.XC_SUB_GRP = D.XC_SUB_GRP,
			GX25.XC_USR_TYP = D.XC_USR_TYP,
			GX25.XM_USR_LST = D.XM_USR_LST,
			GX25.XM_USR_1 = D.XM_USR_1,
			GX25.XM_USR_MID = D.XM_USR_MID
	WHEN NOT MATCHED THEN
		INSERT 
		(
			XF_USR,
			XF_LST_DTS_GX25,
			XC_GRP,
			XC_SUB_GRP,
			XC_USR_TYP,
			XM_USR_LST,
			XM_USR_1,
			XM_USR_MID
		)
		VALUES 
		(
			D.XF_USR,
			D.XF_LST_DTS_GX25,
			D.XC_GRP,
			D.XC_SUB_GRP,
			D.XC_USR_TYP,
			D.XM_USR_LST,
			D.XM_USR_1,
			D.XM_USR_MID
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--	WHEN NOT MATCHED BY SOURCE THEN
	--		DELETE
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
		LEGEND,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				PKUB.GX25_USR GX25
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..GX25_USR GX25
	) L 
		ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('GX25_USR - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(8),
			L_BF_SSN CHAR(8)
		)

		PRINT 'Insert SSN with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.XF_USR,
			L.XF_USR
		FROM
			OPENQUERY
			(
				LEGEND,	
				'
					SELECT
						GX25.XF_USR,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.GX25_USR GX25
					GROUP BY
						GX25.XF_USR
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					GX25.XF_USR,
					COUNT(*) [LocalCount]
				FROM
					CDW..GX25_USR GX25
				GROUP BY
					GX25.XF_USR
			) L 
				ON L.XF_USR = R.XF_USR
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local GX25 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			GX25
		FROM
			CDW..GX25_USR GX25
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = GX25.XF_USR

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END
