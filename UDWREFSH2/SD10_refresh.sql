USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.SD10_STU_SPR
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.SD10_STU_SPR SD10
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(SD10.LF_LST_DTS_SD10), '1-1-1900 00:00:00'), 21) FROM SD10_STU_SPR SD10)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.SD10_STU_SPR SD10
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
							SD10.*
						FROM
							OLWHRM1.SD10_STU_SPR SD10
						--comment WHERE clause for full table refresh
						WHERE
							SD10.LF_LST_DTS_SD10 > ''''' + @LastRefresh + '''''
							OR
							(
								SD10.LF_STU_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON 
			SD10.LF_STU_SSN = D.LF_STU_SSN 
			AND SD10.LN_STU_SPR_SEQ = D.LN_STU_SPR_SEQ
	WHEN MATCHED THEN 
		UPDATE SET
			SD10.LD_STA_STU10 = D.LD_STA_STU10,
			SD10.LC_STA_STU10 = D.LC_STA_STU10,
			SD10.LD_NTF_SCL_SPR = D.LD_NTF_SCL_SPR,
			SD10.LC_SCR_SCL_SPR = D.LC_SCR_SCL_SPR,
			SD10.LC_REA_SCL_SPR = D.LC_REA_SCL_SPR,
			SD10.LD_SCL_SPR = D.LD_SCL_SPR,
			SD10.LF_DOE_SCL_ENR_CUR = D.LF_DOE_SCL_ENR_CUR,
			SD10.LF_LST_DTS_SD10 = D.LF_LST_DTS_SD10,
			SD10.IF_HSP = D.IF_HSP,
			SD10.LD_SCL_CER_STU_STA = D.LD_SCL_CER_STU_STA,
			SD10.LD_ENR_STA_EFF_CAM = D.LD_ENR_STA_EFF_CAM,
			SD10.LC_SEP_DTE_TRT_SRC = D.LC_SEP_DTE_TRT_SRC,
			SD10.LF_LST_USR_SD10 = D.LF_LST_USR_SD10,
			SD10.LX_GTR_ID_CHG_SRC = D.LX_GTR_ID_CHG_SRC
	WHEN NOT MATCHED THEN
		INSERT 
		(
			LF_STU_SSN,
			LN_STU_SPR_SEQ,
			LD_STA_STU10,
			LC_STA_STU10,
			LD_NTF_SCL_SPR,
			LC_SCR_SCL_SPR,
			LC_REA_SCL_SPR,
			LD_SCL_SPR,
			LF_DOE_SCL_ENR_CUR,
			LF_LST_DTS_SD10,
			IF_HSP,
			LD_SCL_CER_STU_STA,
			LD_ENR_STA_EFF_CAM,
			LC_SEP_DTE_TRT_SRC,
			LF_LST_USR_SD10,
			LX_GTR_ID_CHG_SRC
		)
		VALUES 
		(
			D.LF_STU_SSN,
			D.LN_STU_SPR_SEQ,
			D.LD_STA_STU10,
			D.LC_STA_STU10,
			D.LD_NTF_SCL_SPR,
			D.LC_SCR_SCL_SPR,
			D.LC_REA_SCL_SPR,
			D.LD_SCL_SPR,
			D.LF_DOE_SCL_ENR_CUR,
			D.LF_LST_DTS_SD10,
			D.IF_HSP,
			D.LD_SCL_CER_STU_STA,
			D.LD_ENR_STA_EFF_CAM,
			D.LC_SEP_DTE_TRT_SRC,
			D.LF_LST_USR_SD10,
			D.LX_GTR_ID_CHG_SRC
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
				OLWHRM1.SD10_STU_SPR SD10
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..SD10_STU_SPR SD10
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('SD10_STU_SPR - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
			R.LF_STU_SSN,
			L.LF_STU_SSN
		FROM
			OPENQUERY
			(
				DUSTER,	
				'
					SELECT
						SD10.LF_STU_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.SD10_STU_SPR SD10
					GROUP BY
						SD10.LF_STU_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					SD10.LF_STU_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..SD10_STU_SPR SD10
				GROUP BY
					SD10.LF_STU_SSN
			) L ON L.LF_STU_SSN = R.LF_STU_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local SD10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			SD10
		FROM
			UDW..SD10_STU_SPR SD10
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = SD10.LF_STU_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END

