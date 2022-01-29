USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN50_BR_DFR_APV
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.LN50_BR_DFR_APV LN50
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN50.LF_LST_DTS_LN50), '1-1-1900 00:00:00'), 21) FROM LN50_BR_DFR_APV LN50)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN50_BR_DFR_APV LN50
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
							LN50.*
						FROM
							PKUB.LN50_BR_DFR_APV LN50
						-- comment WHERE clause for full table refresh
						WHERE
							LN50.LF_LST_DTS_LN50 > ''''' + @LastRefresh + '''''
							OR
							(
								LN50.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON 
			LN50.BF_SSN = D.BF_SSN 
			AND LN50.LN_SEQ = D.LN_SEQ
			AND LN50.LF_DFR_CTL_NUM = D.LF_DFR_CTL_NUM
			AND LN50.LN_DFR_OCC_SEQ = D.LN_DFR_OCC_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			LN50.LC_DFR_RSP = D.LC_DFR_RSP,
			LN50.LD_DFR_BEG = D.LD_DFR_BEG,
			LN50.LD_DFR_END = D.LD_DFR_END,
			LN50.LD_DFR_GRC_END = D.LD_DFR_GRC_END,
			LN50.LF_LST_DTS_LN50 = D.LF_LST_DTS_LN50,
			LN50.LC_STA_LON50 = D.LC_STA_LON50,
			LN50.LD_STA_LON50 = D.LD_STA_LON50,
			LN50.LD_DFR_APL = D.LD_DFR_APL,
			LN50.LC_LON_LEV_DFR_CAP = D.LC_LON_LEV_DFR_CAP,
			LN50.LI_DLQ_CAP = D.LI_DLQ_CAP
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LF_DFR_CTL_NUM,
			LN_DFR_OCC_SEQ,
			LC_DFR_RSP,
			LD_DFR_BEG,
			LD_DFR_END,
			LD_DFR_GRC_END,
			LF_LST_DTS_LN50,
			LC_STA_LON50,
			LD_STA_LON50,
			LD_DFR_APL,
			LC_LON_LEV_DFR_CAP,
			LI_DLQ_CAP
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ,
			D.LF_DFR_CTL_NUM,
			D.LN_DFR_OCC_SEQ,
			D.LC_DFR_RSP,
			D.LD_DFR_BEG,
			D.LD_DFR_END,
			D.LD_DFR_GRC_END,
			D.LF_LST_DTS_LN50,
			D.LC_STA_LON50,
			D.LD_STA_LON50,
			D.LD_DFR_APL,
			D.LC_LON_LEV_DFR_CAP,
			D.LI_DLQ_CAP
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
				PKUB.LN50_BR_DFR_APV LN50
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..LN50_BR_DFR_APV LN50
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN50_BR_DFR_APV - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
				LEGEND,	
				'
					SELECT
						LN50.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.LN50_BR_DFR_APV LN50
					GROUP BY
						LN50.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN50.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..LN50_BR_DFR_APV LN50
				GROUP BY
					LN50.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local LN50 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN50
		FROM
			CDW..LN50_BR_DFR_APV LN50
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN50.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END


