USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.LN66_LON_RPS_SPF
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.LN66_LON_RPS_SPF
--		'
--	)


DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(LN66.LF_LST_DTS_LN66), '1-1-1900 00:00:00'), 21) FROM LN66_LON_RPS_SPF LN66)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN66_LON_RPS_SPF LN66
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
							LN66.*
						FROM
							OLWHRM1.LN66_LON_RPS_SPF LN66
						-- comment WHERE clause for full table refresh
						WHERE
							LN66.LF_LST_DTS_LN66 > ''''' + @LastRefresh + '''''
							OR
							(
								LN66.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON D.BF_SSN = LN66.BF_SSN AND D.LN_SEQ = LN66.LN_SEQ AND D.LN_RPS_SEQ = LN66.LN_RPS_SEQ AND D.LN_GRD_RPS_SEQ = LN66.LN_GRD_RPS_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			LN66.LA_RPS_ISL = D.LA_RPS_ISL,
			LN66.LD_CRT_LON66 = D.LD_CRT_LON66,
			LN66.LN_RPS_TRM = D.LN_RPS_TRM,
			LN66.LF_LST_DTS_LN66 = D.LF_LST_DTS_LN66,
			LN66.LA_PRI_RDC_GRD = D.LA_PRI_RDC_GRD,
			LN66.LN_PRI_RDC_GRD_TRM = D.LN_PRI_RDC_GRD_TRM,
			LN66.LA_PRI_ATU_PAY = D.LA_PRI_ATU_PAY,
			LN66.LD_RPYE_FGV = D.LD_RPYE_FGV,
			LN66.LA_RPYE_CTH_ISL = D.LA_RPYE_CTH_ISL
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ,
			LN_RPS_SEQ,
			LN_GRD_RPS_SEQ,
			LA_RPS_ISL,
			LD_CRT_LON66,
			LN_RPS_TRM,
			LF_LST_DTS_LN66,
			LA_PRI_RDC_GRD,
			LN_PRI_RDC_GRD_TRM,
			LA_PRI_ATU_PAY,
			LD_RPYE_FGV,
			LA_RPYE_CTH_ISL
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ,
			D.LN_RPS_SEQ,
			D.LN_GRD_RPS_SEQ,
			D.LA_RPS_ISL,
			D.LD_CRT_LON66,
			D.LN_RPS_TRM,
			D.LF_LST_DTS_LN66,
			D.LA_PRI_RDC_GRD,
			D.LN_PRI_RDC_GRD_TRM,
			D.LA_PRI_ATU_PAY,
			D.LD_RPYE_FGV,
			D.LA_RPYE_CTH_ISL
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
				OLWHRM1.LN66_LON_RPS_SPF LN66
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..LN66_LON_RPS_SPF LN66
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN66_LON_RPS_SPF - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
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
						LN66.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.LN66_LON_RPS_SPF LN66
					GROUP BY
						LN66.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN66.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..LN66_LON_RPS_SPF LN66
				GROUP BY
					LN66.BF_SSN
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

		PRINT 'The local record count of some SSNs does not match the remote warehouse count. Deleting all local LN66 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN66
		FROM
			UDW..LN66_LON_RPS_SPF LN66
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN66.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
