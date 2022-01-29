USE UDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(PH01.DF_LST_DTS_PH01)), '1-1-1900 00:00:00'), 21) FROM PH01_SUPER_ID PH01)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PH01_SUPER_ID PH01
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					Duster,
					''
						SELECT
							RIGHT(CONCAT(''''0000000000'''', CAST(DF_SPE_ID AS VARCHAR(10))), 10) AS "DF_SPE_ID",  -- add in DF_SPE_ID leading zeros; PH01 data type is decimal
							DF_SBS,
							DF_RGN,
							DF_PRS_ID,
							DF_CRT_USR_PH01,
							DF_CRT_DTS_PH01,
							DC_STA_PH01,
							DF_LST_USR_PH01,
							DF_LST_DTS_PH01
						FROM
							OLWHRM1.PH01_SUPER_ID PH01
						-- comment WHERE clause for full table refresh
						WHERE
							PH01.DF_LST_DTS_PH01 > ''''' + @LastRefresh + '''''
							OR
							(
								RIGHT(CONCAT(''''0000000000'''', CAST(DF_SPE_ID AS VARCHAR(10))), 10) IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L 
			ON L.DF_SBS = PH01.DF_SBS 
			AND L.DF_RGN = PH01.DF_RGN 
			AND L.DF_SPE_ID = PH01.DF_SPE_ID -- add in DF_SPE_ID leading zeros; PH01 data type is decimal
	WHEN MATCHED THEN 
		UPDATE SET 
			PH01.DF_PRS_ID = L.DF_PRS_ID,
			PH01.DF_CRT_USR_PH01 = L.DF_CRT_USR_PH01,
			PH01.DF_CRT_DTS_PH01 = L.DF_CRT_DTS_PH01,
			PH01.DC_STA_PH01 = L.DC_STA_PH01,
			PH01.DF_LST_USR_PH01 = L.DF_LST_USR_PH01,
			PH01.DF_LST_DTS_PH01 = L.DF_LST_DTS_PH01
	WHEN NOT MATCHED THEN
		INSERT 
		(
			DF_SPE_ID,
			DF_SBS,
			DF_RGN,
			DF_PRS_ID,
			DF_CRT_USR_PH01,
			DF_CRT_DTS_PH01,
			DC_STA_PH01,
			DF_LST_USR_PH01,
			DF_LST_DTS_PH01
		)
		VALUES 
		(
			L.DF_SPE_ID,
			L.DF_SBS,
			L.DF_RGN,
			L.DF_PRS_ID,
			L.DF_CRT_USR_PH01,
			L.DF_CRT_DTS_PH01,
			L.DC_STA_PH01,
			L.DF_LST_USR_PH01,
			L.DF_LST_DTS_PH01
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--   DELETE
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
		Duster,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.PH01_SUPER_ID
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			PH01_SUPER_ID
	) L 
		ON 1 = 1
	

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('OLWHRM1.PH01_SUPER_ID - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(10),
			L_BF_SSN CHAR(10)
		)

		PRINT 'Insert SSN (DF_SPE_ID) with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.DF_SPE_ID,
			L.DF_SPE_ID
		FROM
			OPENQUERY
			(
				Duster,	
				'
					SELECT
						RIGHT(CONCAT(''0000000000'', CAST(PH01.DF_SPE_ID AS VARCHAR(10))), 10) AS "DF_SPE_ID",
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PH01_SUPER_ID PH01
					GROUP BY
						PH01.DF_SPE_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PH01.DF_SPE_ID,
					COUNT(*) [LocalCount]
				FROM
					UDW..PH01_SUPER_ID PH01
				GROUP BY
					PH01.DF_SPE_ID
			) L 
				ON L.DF_SPE_ID = R.DF_SPE_ID
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

		PRINT 'The local record count for these SSNs (DF_SPE_ID) does not match the remote warehouse count: ' + @SSNs + '  Deleting all local PH05 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PH01
		FROM
			UDW..PH01_SUPER_ID PH01
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PH01.DF_SPE_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END