USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN60_BR_FOR_APV
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.LN60_BR_FOR_APV LN60
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN60.LF_LST_DTS_LN60), '1-1-1900 00:00:00'), 21) FROM LN60_BR_FOR_APV LN60)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN60_BR_FOR_APV LN60
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
							LN60.*
						FROM
							OLWHRM1.LN60_BR_FOR_APV LN60
						-- comment WHERE clause for full table refresh
						WHERE
							LN60.LF_LST_DTS_LN60 > ''''' + @LastRefresh + '''''
							OR
							(
								LN60.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON 
			LN60.BF_SSN = D.BF_SSN 
			AND LN60.LN_SEQ = D.LN_SEQ
			AND LN60.LF_FOR_CTL_NUM = D.LF_FOR_CTL_NUM
			AND LN60.LN_FOR_OCC_SEQ = D.LN_FOR_OCC_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			LN60.LC_FOR_RSP = D.LC_FOR_RSP,
			LN60.LD_FOR_BEG = D.LD_FOR_BEG,
			LN60.LD_FOR_END = D.LD_FOR_END,
			LN60.LC_STA_LON60 = D.LC_STA_LON60,
			LN60.LD_STA_LON60 = D.LD_STA_LON60,
			LN60.LD_FOR_APL = D.LD_FOR_APL,
			LN60.LF_LST_DTS_LN60 = D.LF_LST_DTS_LN60,
			LN60.LI_FOR_20_RPT = D.LI_FOR_20_RPT,
			LN60.LC_LON_LEV_FOR_CAP = D.LC_LON_LEV_FOR_CAP,
			LN60.LA_FOR_20_INT_ACR = D.LA_FOR_20_INT_ACR,
			LN60.LA_ACL_RDC_PAY = D.LA_ACL_RDC_PAY,
			LN60.LI_FOR_VRB_DFL_RUL = D.LI_FOR_VRB_DFL_RUL
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LF_FOR_CTL_NUM,
			LN_FOR_OCC_SEQ,
			LC_FOR_RSP,
			LD_FOR_BEG,
			LD_FOR_END,
			LD_STA_LON60,
			LC_STA_LON60,
			LD_FOR_APL,
			LF_LST_DTS_LN60,
			LI_FOR_20_RPT,
			LC_LON_LEV_FOR_CAP,
			LA_FOR_20_INT_ACR,
			LA_ACL_RDC_PAY,
			LI_FOR_VRB_DFL_RUL
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ,
			D.LF_FOR_CTL_NUM,
			D.LN_FOR_OCC_SEQ,
			D.LC_FOR_RSP,
			D.LD_FOR_BEG,
			D.LD_FOR_END,
			D.LD_STA_LON60,
			D.LC_STA_LON60,
			D.LD_FOR_APL,
			D.LF_LST_DTS_LN60,
			D.LI_FOR_20_RPT,
			D.LC_LON_LEV_FOR_CAP,
			D.LA_FOR_20_INT_ACR,
			D.LA_ACL_RDC_PAY,
			D.LI_FOR_VRB_DFL_RUL
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
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.LN60_BR_FOR_APV LN60
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..LN60_BR_FOR_APV LN60
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN60_BR_FOR_APV - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						LN60.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.LN60_BR_FOR_APV LN60
					GROUP BY
						LN60.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN60.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..LN60_BR_FOR_APV LN60
				GROUP BY
					LN60.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local LN60 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN60
		FROM
			UDW..LN60_BR_FOR_APV LN60
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN60.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END

