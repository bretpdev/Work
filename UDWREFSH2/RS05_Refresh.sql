USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.RS05_IBR_RPS
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.RS05_IBR_RPS
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(RS05.BF_LST_DTS_RS05), '1-1-1900 00:00:00'), 21) FROM RS05_IBR_RPS RS05)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.RS05_IBR_RPS RS05
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
							OLWHRM1.RS05_IBR_RPS RS05
						-- comment WHERE clause for full table refresh
						WHERE
							RS05.BF_LST_DTS_RS05 > ''''' + @LastRefresh + '''''
							OR
							(
								RS05.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON D.BF_SSN = RS05.BF_SSN AND D.BD_CRT_RS05 = RS05.BD_CRT_RS05 AND D.BN_IBR_SEQ = RS05.BN_IBR_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			RS05.BF_SSN = D.BF_SSN,
			RS05.BD_CRT_RS05 = D.BD_CRT_RS05,
			RS05.BN_IBR_SEQ = D.BN_IBR_SEQ,
			RS05.BF_CRT_USR_RS05 = D.BF_CRT_USR_RS05,
			RS05.BF_CRY_YR = D.BF_CRY_YR,
			RS05.BC_ST_IBR = D.BC_ST_IBR,
			RS05.BC_STA_RS05 = D.BC_STA_RS05,
			RS05.BA_AGI = D.BA_AGI,
			RS05.BN_MEM_HSE_HLD = D.BN_MEM_HSE_HLD,
			RS05.BA_PMN_STD_TOT_PAY = D.BA_PMN_STD_TOT_PAY,
			RS05.BC_IBR_INF_SRC_VER = D.BC_IBR_INF_SRC_VER,
			RS05.BF_LST_DTS_RS05 = D.BF_LST_DTS_RS05,
			RS05.BF_SSN_SPO = D.BF_SSN_SPO,
			RS05.BC_IRS_TAX_FIL_STA = D.BC_IRS_TAX_FIL_STA,
			RS05.BI_JNT_BR_SPO_RPY = D.BI_JNT_BR_SPO_RPY,
			RS05.BD_ANV_QLF_IBR = D.BD_ANV_QLF_IBR,
			RS05.BC_DOC_SNT_BR_IDR = D.BC_DOC_SNT_BR_IDR
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			BD_CRT_RS05,
			BN_IBR_SEQ,
			BF_CRT_USR_RS05,
			BF_CRY_YR,
			BC_ST_IBR,
			BC_STA_RS05,
			BA_AGI,
			BN_MEM_HSE_HLD,
			BA_PMN_STD_TOT_PAY,
			BC_IBR_INF_SRC_VER,
			BF_LST_DTS_RS05,
			BF_SSN_SPO,
			BC_IRS_TAX_FIL_STA,
			BI_JNT_BR_SPO_RPY,
			BD_ANV_QLF_IBR,
			BC_DOC_SNT_BR_IDR
		)
		VALUES 
		(
			D.BF_SSN,
			D.BD_CRT_RS05,
			D.BN_IBR_SEQ,
			D.BF_CRT_USR_RS05,
			D.BF_CRY_YR,
			D.BC_ST_IBR,
			D.BC_STA_RS05,
			D.BA_AGI,
			D.BN_MEM_HSE_HLD,
			D.BA_PMN_STD_TOT_PAY,
			D.BC_IBR_INF_SRC_VER,
			D.BF_LST_DTS_RS05,
			D.BF_SSN_SPO,
			D.BC_IRS_TAX_FIL_STA,
			D.BI_JNT_BR_SPO_RPY,
			D.BD_ANV_QLF_IBR,
			D.BC_DOC_SNT_BR_IDR
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
				OLWHRM1.RS05_IBR_RPS RS05
		'	 
	) R 
	FULL OUTER JOIN 
	( 
		SELECT 
			COUNT(*) [LocalCount] 
		FROM 
			RS05_IBR_RPS RS05
	) L ON 1 = 1 
	 
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('RS05_IBR_RPS - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						RS05.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.RS05_IBR_RPS RS05
					GROUP BY
						RS05.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					RS05.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..RS05_IBR_RPS RS05
				GROUP BY
					RS05.BF_SSN
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
			RS05
		FROM
			UDW..RS05_IBR_RPS RS05
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = RS05.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
