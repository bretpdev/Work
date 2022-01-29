USE UDW
GO

--SELECT TOP 1
--	*
--INTO
--	PD01_PDM_INF
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.PD01_PDM_INF PD01
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(PD01.DF_LST_DTS_PD01), '1-1-1900 00:00:00'), 21) FROM PD01_PDM_INF PD01)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
	'

	MERGE 
		dbo.PD01_PDM_INF LOCAL
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
							REMOTE.*
						FROM
							OLWHRM1.PD01_PDM_INF REMOTE
						WHERE
							REMOTE.DF_LST_DTS_PD01 > ''''' + @LastRefresh + '''''
							OR
							(
								REMOTE.DF_PRS_ID IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
			) R ON R.DF_PRS_ID = LOCAL.DF_PRS_ID
			WHEN MATCHED THEN 
				UPDATE SET 
				LOCAL.DD_BRT = R.DD_BRT,
			WHEN NOT MATCHED THEN
				INSERT 
				(
					DF_PRS_ID,
				)
				VALUES 
				(
				R.DF_PRS_ID,
				)
		-- !!! uncomment lines below ONLY when doing a full table refresh 
		--WHEN NOT MATCHED BY SOURCE THEN
		--    DELETE
		;
	'
PRINT @SQLStatement
EXEC (@SQLStatement)


-- ###### VALIDATION
-- the DF_LST_DTS_PD01 date is unreliable when a person's SSN has been changed.
-- sum all SSNs in order to dentify missing/change/added SSNs
DECLARE 
	@SumDifference INT

SELECT
	@SumDifference = L.LocalSum - R.RemoteSum
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				SUM(CAST(DF_PRS_ID AS BIGINT)) AS "RemoteSum"
			FROM
				OLWHRM1.PD01_PDM_INF PD01
			WHERE
				PD01.DF_PRS_ID NOT LIKE ''P%''
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			SUM(CAST(DF_PRS_ID AS BIGINT)) [LocalSum]
		FROM
			UDW..PD01_PDM_INF PD01
		WHERE
			PD01.DF_PRS_ID NOT LIKE 'P%'
	) L ON 1 = 1


IF @SumDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD01_PDM_INF - The remote and local SSN sums do not match.  A full refresh of the table may be required.', 16, 11, @SumDifference)
	END
ELSE IF @SumDifference != 0 AND @LoopCount = 0
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
						PD01.DF_PRS_ID AS "BF_SSN",
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PD01_PDM_INF PD01
					GROUP BY
						PD01.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD01.DF_PRS_ID [BF_SSN],
					COUNT(*) [LocalCount]
				FROM
					UDW..PD01_PDM_INF PD01
				GROUP BY
					PD01.DF_PRS_ID
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count.  Deleting all local PD01 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD01
		FROM
			UDW..PD01_PDM_INF PD01
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PD01.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END