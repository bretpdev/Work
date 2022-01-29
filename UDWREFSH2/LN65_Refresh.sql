USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN65_LON_RPS
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.LN65_LON_RPS
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN65.LF_LST_DTS_LN65), '1-1-1900 00:00:00'), 21) FROM LN65_LON_RPS LN65)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN65_LON_RPS LN65
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
							OLWHRM1.LN65_LON_RPS LN65
						-- comment WHERE clause for full table refresh
						WHERE
							LN65.LF_LST_DTS_LN65 > ''''' + @LastRefresh + '''''
							OR
							(
								LN65.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON D.BF_SSN = LN65.BF_SSN AND D.LN_SEQ = LN65.LN_SEQ AND D.LN_RPS_SEQ = LN65.LN_RPS_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN65.BF_SSN = D.BF_SSN,
			LN65.LN_SEQ = D.LN_SEQ,
			LN65.LN_RPS_SEQ = D.LN_RPS_SEQ,
			LN65.LA_RPD_INT_DIS = D.LA_RPD_INT_DIS,
			LN65.LR_APR_RPD_DIS = D.LR_APR_RPD_DIS,
			LN65.LA_TOT_RPD_DIS = D.LA_TOT_RPD_DIS,
			LN65.LA_CPI_RPD_DIS = D.LA_CPI_RPD_DIS,
			LN65.LR_INT_RPD_DIS = D.LR_INT_RPD_DIS,
			LN65.LA_ANT_CAP = D.LA_ANT_CAP,
			LN65.LD_GRC_PRD_END = D.LD_GRC_PRD_END,
			LN65.LD_CRT_LON65 = D.LD_CRT_LON65,
			LN65.LC_STA_LON65 = D.LC_STA_LON65,
			LN65.LF_LST_DTS_LN65 = D.LF_LST_DTS_LN65,
			LN65.LC_TYP_SCH_DIS = D.LC_TYP_SCH_DIS,
			LN65.LA_ACR_INT_RPD = D.LA_ACR_INT_RPD,
			LN65.LA_ANT_SUP_FEE = D.LA_ANT_SUP_FEE,
			LN65.LN_RPD_MAX_TRM_REQ = D.LN_RPD_MAX_TRM_REQ,
			LN65.LD_RPD_MAX_TRM_SR = D.LD_RPD_MAX_TRM_SR,
			LN65.LC_RPD_INA_REA = D.LC_RPD_INA_REA,
			LN65.LC_RPD_DIS = D.LC_RPD_DIS,
			LN65.LR_CLC_INC_SCH = D.LR_CLC_INC_SCH,
			LN65.LA_CLC_RPY_SCH = D.LA_CLC_RPY_SCH,
			LN65.LI_ICR_RPD_NEG_AMR = D.LI_ICR_RPD_NEG_AMR
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LN_RPS_SEQ,
			LA_RPD_INT_DIS,
			LR_APR_RPD_DIS,
			LA_TOT_RPD_DIS,
			LA_CPI_RPD_DIS,
			LR_INT_RPD_DIS,
			LA_ANT_CAP,
			LD_GRC_PRD_END,
			LD_CRT_LON65,
			LC_STA_LON65,
			LF_LST_DTS_LN65,
			LC_TYP_SCH_DIS,
			LA_ACR_INT_RPD,
			LA_ANT_SUP_FEE,
			LN_RPD_MAX_TRM_REQ,
			LD_RPD_MAX_TRM_SR,
			LC_RPD_INA_REA,
			LC_RPD_DIS,
			LR_CLC_INC_SCH,
			LA_CLC_RPY_SCH,
			LI_ICR_RPD_NEG_AMR
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ,
			D.LN_RPS_SEQ,
			D.LA_RPD_INT_DIS,
			D.LR_APR_RPD_DIS,
			D.LA_TOT_RPD_DIS,
			D.LA_CPI_RPD_DIS,
			D.LR_INT_RPD_DIS,
			D.LA_ANT_CAP,
			D.LD_GRC_PRD_END,
			D.LD_CRT_LON65,
			D.LC_STA_LON65,
			D.LF_LST_DTS_LN65,
			D.LC_TYP_SCH_DIS,
			D.LA_ACR_INT_RPD,
			D.LA_ANT_SUP_FEE,
			D.LN_RPD_MAX_TRM_REQ,
			D.LD_RPD_MAX_TRM_SR,
			D.LC_RPD_INA_REA,
			D.LC_RPD_DIS,
			D.LR_CLC_INC_SCH,
			D.LA_CLC_RPY_SCH,
			D.LI_ICR_RPD_NEG_AMR
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
				OLWHRM1.LN65_LON_RPS LN65
		'	 
	) R 
	FULL OUTER JOIN 
	( 
		SELECT 
			COUNT(*) [LocalCount] 
		FROM 
			LN65_LON_RPS LN65
	) L ON 1 = 1 
	 
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN65_LON_RPS - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						LN65.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.LN65_LON_RPS LN65
					GROUP BY
						LN65.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN65.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..LN65_LON_RPS LN65
				GROUP BY
					LN65.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local RS05 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN65
		FROM
			UDW..LN65_LON_RPS LN65
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN65.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END