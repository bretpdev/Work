USE CDW
GO
DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(RS10.LF_LST_DTS_RS10)), '1-1-1900 00:00:00'), 21) FROM RS10_BR_RPD RS10)
DECLARE @LastRefreshStatusDate VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,CAST(MAX(RS10.LD_STA_RPST10) AS DATETIME)), '1-1-1900 00:00:00'), 21) FROM RS10_BR_RPD RS10)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh
PRINT 'Last Refreshed status date at: ' + @LastRefreshStatusDate


DECLARE @SQLStatement VARCHAR(MAX) = 
'
MERGE 
		dbo.RS10_BR_RPD RS10
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
							RS10.*
						FROM
							PKUB.RS10_BR_RPD RS10
						-- comment WHERE clause for full table refresh
						WHERE
							RS10.LF_LST_DTS_RS10 > ''''' + @LastRefresh + '''''
							OR RS10.LD_STA_RPST10 >= ''''' + @LastRefreshStatusDate + '''''
							OR
							(
								RS10.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
							FOR READ ONLY WITH UR
					''
				) 
		) L 
			ON L.BF_SSN = RS10.BF_SSN 
			AND L.LN_RPS_SEQ = RS10.LN_RPS_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			RS10.LD_STA_RPST10 = L.LD_STA_RPST10,
			RS10.LC_STA_RPST10 = L.LC_STA_RPST10,
			RS10.LC_FRQ_PAY = L.LC_FRQ_PAY,
			RS10.LI_SIG_RPD_DIS = L.LI_SIG_RPD_DIS,
			RS10.LD_RPS_1_PAY_DU = L.LD_RPS_1_PAY_DU,
			RS10.LC_RPD_DIS = L.LC_RPD_DIS,
			RS10.LD_SNT_RPD_DIS = L.LD_SNT_RPD_DIS,
			RS10.LD_RTN_RPD_DIS = L.LD_RTN_RPD_DIS,
			RS10.LF_LST_DTS_RS10 = L.LF_LST_DTS_RS10,
			RS10.LC_RPS_OPT_PRT = L.LC_RPS_OPT_PRT,
			RS10.LF_USR_RPS_REQ = L.LF_USR_RPS_REQ,
			RS10.LN_BR_REQ_DU_DAY = L.LN_BR_REQ_DU_DAY,
			RS10.BD_CRT_RS05 = L.BD_CRT_RS05,
			RS10.BN_IBR_SEQ = L.BN_IBR_SEQ,
			RS10.LC_RPY_FIX_TRM_AMT = L.LC_RPY_FIX_TRM_AMT,
			RS10.LC_CAP_TRG_LVE_PFH = L.LC_CAP_TRG_LVE_PFH
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_RPS_SEQ,
			LD_STA_RPST10,
			LC_STA_RPST10,
			LC_FRQ_PAY,
			LI_SIG_RPD_DIS,
			LD_RPS_1_PAY_DU,
			LC_RPD_DIS,
			LD_SNT_RPD_DIS,
			LD_RTN_RPD_DIS,
			LF_LST_DTS_RS10,
			LC_RPS_OPT_PRT,
			LF_USR_RPS_REQ,
			LN_BR_REQ_DU_DAY,
			BD_CRT_RS05,
			BN_IBR_SEQ,
			LC_RPY_FIX_TRM_AMT,
			LC_CAP_TRG_LVE_PFH
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_RPS_SEQ,
			L.LD_STA_RPST10,
			L.LC_STA_RPST10,
			L.LC_FRQ_PAY,
			L.LI_SIG_RPD_DIS,
			L.LD_RPS_1_PAY_DU,
			L.LC_RPD_DIS,
			L.LD_SNT_RPD_DIS,
			L.LD_RTN_RPD_DIS,
			L.LF_LST_DTS_RS10,
			L.LC_RPS_OPT_PRT,
			L.LF_USR_RPS_REQ,
			L.LN_BR_REQ_DU_DAY,
			L.BD_CRT_RS05,
			L.BN_IBR_SEQ,
			L.LC_RPY_FIX_TRM_AMT,
			L.LC_CAP_TRG_LVE_PFH
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'

--select @SQLStatement
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
				PKUB.RS10_BR_RPD RS10
		'	
	) R
	LEFT OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			RS10_BR_RPD RS10
	) L 
		ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('RS10_BR_RPD - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						RS10.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.RS10_BR_RPD RS10
					GROUP BY
						RS10.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					RS10.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..RS10_BR_RPD RS10
				GROUP BY
					RS10.BF_SSN
			) L 
				ON L.BF_SSN = R.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local RS10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			RS10
		FROM
			CDW..RS10_BR_RPD RS10
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = RS10.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END


GO
;
